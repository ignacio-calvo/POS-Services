using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using POS.CustomerRegistration.API.Mappings;
using POS.CustomerRegistration.API.IServices;
using POS.CustomerRegistration.API.Services;
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
                          policy.WithOrigins("http://localhost:*",
                                             "https://localhost:*")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var identityApiUrl = Environment.GetEnvironmentVariable("IdentityApiUrl")
             ?? builder.Configuration["IdentityApiUrl"]
             ?? throw new InvalidOperationException("Environment variable or secret 'IdentityApiUrl' not found.");

var customersApiUrl = Environment.GetEnvironmentVariable("CustomersApiUrl")
             ?? builder.Configuration["CustomersApiUrl"]
             ?? throw new InvalidOperationException("Environment variable or secret 'CustomersApiUrl' not found.");

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
       });

// Register HttpClient services for CustomerService and IdentityService
builder.Services.AddHttpClient<ICustomerService, CustomerService>(client =>
{
    client.BaseAddress = new Uri(customersApiUrl);
});

builder.Services.AddHttpClient<IIdentityService, IdentityService>(client =>
{
    client.BaseAddress = new Uri(identityApiUrl);
});

// Register CustomerRegistrationService
builder.Services.AddScoped<ICustomerRegistrationService, CustomerRegistrationService>();

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(MappingProfile));

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
//app.UseStaticFiles();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<ApplicationDbContext>();
//    var logger = services.GetRequiredService<ILogger<Program>>();

//    context.Database.Migrate();
//}

app.Run();
