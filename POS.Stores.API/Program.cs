using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POS.Stores.Business.Mappings;
using POS.Stores.Business.Services;
using POS.Stores.Business.Services.IServices;
using POS.Stores.Business.Services.IServices.IServiceMappings;
using POS.Stores.Business.Services.ServiceMappings;
using POS.Stores.Data;
using POS.Stores.Data.Interfaces;
using POS.Stores.Data.Repositories;
using System.Text;

var MyAllowSpecificOrigins = "_myAllowLocalOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add user secrets to the configuration
builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Read JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");

// Read the Authority from the environment variable or fallback to .NET secrets
var identityApiUrl = Environment.GetEnvironmentVariable("IdentityApiUrl")
                    ?? builder.Configuration["IdentityApiUrl"]
                    ?? throw new InvalidOperationException("Environment variable or secret 'IdentityApiUrl' not found.");


// Read the JWT key from the environment variable or fallback to .NET secrets
var jwtKey = Environment.GetEnvironmentVariable("IdentityJwtKey")
             ?? builder.Configuration["IdentityJwtKey"]
             ?? throw new InvalidOperationException("Environment variable or secret 'IdentityJwtKey' not found.");

// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = identityApiUrl;
        options.Audience = jwtSettings["Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };

        if (builder.Environment.IsDevelopment())
        {
            options.RequireHttpsMetadata = false;
        }
    });

builder.Services.AddAuthorization();

// Database Configuration
builder.Services.AddScoped<StoreDbContext>();

// Read the environment variable for the database password or fallback to .NET secrets
var StoreDbPassword = Environment.GetEnvironmentVariable("StoreDBPassword")
                      ?? builder.Configuration["StoreDBPassword"]
                      ?? throw new InvalidOperationException("Environment variable or secret 'StoreDBPassword' not found.");

// Get the connection string and replace the placeholder with the actual password
string connectionString = builder.Configuration.GetConnectionString("StoreDB")
                         ?.Replace("{StoreDBPassword}", StoreDbPassword)
                         ?? throw new InvalidOperationException("Connection string 'StoreDB' not found.");

builder.Services.AddDbContext<StoreDbContext>(options =>
{
    options.UseSqlServer(
        connectionString,
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("POS.Stores.Data");
        });
});

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Generic Repository
builder.Services.AddScoped(typeof(IStoreRepository), typeof(StoreRepository));

// Generic Services
builder.Services.AddScoped(typeof(IReadServiceAsync<,>), typeof(ReadServiceAsync<,>));
builder.Services.AddScoped(typeof(IGenericServiceAsync<,>), typeof(GenericServiceAsync<,>));

// Specific Services
builder.Services.AddScoped(typeof(IStoreService), typeof(StoreService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection(); // Only force HTTPs in production
}

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication(); // Ensure this is added before UseAuthorization
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
    context.Database.Migrate();
}

app.Run();
