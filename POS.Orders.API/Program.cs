using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POS.Orders.Business.Mappings;
using POS.Orders.Business.Services;
using POS.Orders.Business.Services.IServices;
using POS.Orders.Business.Services.IServices.IServiceMappings;
using POS.Orders.Business.Services.ServiceMappings;
using POS.Orders.Data;
using POS.Orders.Data.Interfaces;
using POS.Orders.Data.Repositories;
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
                          policy.WithOrigins("http://localhost:5100",
                                              "http://127.0.0.1:5100",
                                              "https://localhost:5100")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Read JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");

// Read the Authority from the environment variable or fallback to .NET secrets
var identityApiUrl = Environment.GetEnvironmentVariable("IDENTITY_API_URL")
                    ?? builder.Configuration["IDENTITY_API_URL"]
                    ?? throw new InvalidOperationException("Environment variable or secret 'IDENTITY_API_URL' not found.");

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
    });

builder.Services.AddAuthorization();

// Database Configuration
builder.Services.AddScoped<OrderDbContext>();

// Read the environment variable for the database password or fallback to .NET secrets
var orderDbPassword = Environment.GetEnvironmentVariable("OrderDBPassword")
                      ?? builder.Configuration["OrderDBPassword"]
                      ?? throw new InvalidOperationException("Environment variable or secret 'OrderDBPassword' not found.");

// Get the connection string and replace the placeholder with the actual password
string connectionString = builder.Configuration.GetConnectionString("OrderDB")
                         ?.Replace("{OrderDBPassword}", orderDbPassword)
                         ?? throw new InvalidOperationException("Connection string 'OrderDB' not found.");

builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseSqlServer(
        connectionString,
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("POS.Orders.Data");
        });
});

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Generic Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Specific Repositories
builder.Services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));

// Generic Services
builder.Services.AddScoped(typeof(IReadServiceAsync<,>), typeof(ReadServiceAsync<,>));
builder.Services.AddScoped(typeof(IGenericServiceAsync<,>), typeof(GenericServiceAsync<,>));

// Specific Services
builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));

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
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    context.Database.Migrate();
}

app.Run();
