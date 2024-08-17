using Microsoft.EntityFrameworkCore;
using POS.Customers.Business.Mappings;
using POS.Customers.Business.Services.IServices;
using POS.Customers.Business.Services;
using POS.Customers.Data;
using POS.Customers.Data.Repositories;
using POS.Customers.Business.Services.ServiceMappings;
using POS.Customers.Business.Services.IServices.IServiceMappings;
using POS.Customers.Data.Interfaces;

var MyAllowSpecificOrigins = "_myAllowLocalOrigins";

var builder = WebApplication.CreateBuilder(args);

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

// Database Configuration
builder.Services.AddScoped<CustomerDbContext>();


// Read the environment variable for the database password
var customerDbPassword = Environment.GetEnvironmentVariable("CustomerDBPassword")
                      ?? throw new InvalidOperationException("Environment variable 'CustomerDBPassword' not found.");

// Get the connection string and replace the placeholder with the actual password
string connectionString = builder.Configuration.GetConnectionString("CustomerDB")
                         ?.Replace("{CustomerDBPassword}", customerDbPassword)
                         ?? throw new InvalidOperationException("Connection string 'CustomerDB' not found.");

builder.Services.AddDbContext<CustomerDbContext>(options =>
{
    options.UseSqlServer(
        connectionString,
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("POS.Customers.Data");
        });
});

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Generic Repository
builder.Services.AddScoped(typeof(ICustomerRepository), typeof(CustomerRepository));

// Generic Services
builder.Services.AddScoped(typeof(IReadServiceAsync<,>), typeof(ReadServiceAsync<,>));
builder.Services.AddScoped(typeof(IGenericServiceAsync<,>), typeof(GenericServiceAsync<,>));

// Specific Services
builder.Services.AddScoped(typeof(ICustomerService), typeof(CustomerService));

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

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
    context.Database.Migrate();
}

app.Run();
