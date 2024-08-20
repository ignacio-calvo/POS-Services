using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using POS.Identity.API.Models;
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

// Read the value for the database password. First try reading from within Environment Variable, if that fails fall back to read from .NET Secret Manager
var identityDbPassword = Environment.GetEnvironmentVariable("IdentityDBPassword")
                      ?? builder.Configuration["IdentityDBPassword"]
                      ?? throw new InvalidOperationException("Environment variable or secret 'IdentityDBPassword' not found.");

// Get the connection string and replace the placeholder with the actual password
string connectionString = builder.Configuration.GetConnectionString("IdentityDB")
                         ?.Replace("{IdentityDBPassword}", identityDbPassword)
                         ?? throw new InvalidOperationException("Connection string 'IdentityDB' not found.");

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        connectionString,
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("POS.Identity.API");
        });
});

// Read the secret for the JWT key
var jwtKey = Environment.GetEnvironmentVariable("IdentityJwtKey")
             ?? builder.Configuration["IdentityJwtKey"]
             ?? throw new InvalidOperationException("Environment variable or secret 'IdentityJwtKey' not found.");

// Read the issuer for the JWT
var jwtIssuer = Environment.GetEnvironmentVariable("JwtIssuer")
                ?? builder.Configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("Environment variable or secret 'Jwt:Issuer' not found.");

// Read the audience for the JWT
var jwtAudience = Environment.GetEnvironmentVariable("JwtAudience")
                 ?? builder.Configuration["Jwt:Audience"]
                 ?? throw new InvalidOperationException("Environment variable or secret 'Jwt:Audience' not found.");

// Explicitly set the configuration values for Jwt:Key, Jwt:Issuer, and Jwt:Audience
builder.Configuration["Jwt:Key"] = jwtKey;
builder.Configuration["Jwt:Issuer"] = jwtIssuer;
builder.Configuration["Jwt:Audience"] = jwtAudience;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = builder.Configuration["Jwt:Issuer"],
               ValidAudience = builder.Configuration["Jwt:Audience"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.")))
           };

           if (builder.Environment.IsDevelopment())
           {
               options.RequireHttpsMetadata = false;
           }
       });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    context.Database.Migrate();
}

app.Run();
