using Microsoft.EntityFrameworkCore;
using POS.Products.Business.Mappings;
using POS.Products.Business.Services;
using POS.Products.Business.Services.IServices;
using POS.Products.Business.Services.IServices.IServiceMappings;
using POS.Products.Business.Services.ServiceMappings;
using POS.Products.Data.Interfaces;
using POS.Products.Data.Models;
using POS.Products.Data.Repositories;
using System.Text.Json.Serialization;

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

builder.Services.AddControllers()
    //.AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//builder.Services.AddDbContext<ProductDbContext>(optionsAction:options => options.UseSqlServer());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DataBase Configuration
builder.Services.AddScoped<ProductDbContext>();

// Read the environment variable for the database password
var productDbPassword = Environment.GetEnvironmentVariable("ProductDBPassword")
                      ?? throw new InvalidOperationException("Environment variable 'ProductDBPassword' not found.");

// Get the connection string and replace the placeholder with the actual password
string connectionString = builder.Configuration.GetConnectionString("ProductDB")
                         ?.Replace("{ProductDBPassword}", productDbPassword)
                         ?? throw new InvalidOperationException("Connection string 'ProductDB' not found.");

builder.Services.AddDbContext<ProductDbContext>(options =>
{
    options.UseSqlServer(
        connectionString,
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("POS.Products.Data");
        });
});

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Generic Repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Specific Repositories
builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
builder.Services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));

// Generic Services
builder.Services.AddScoped(typeof(IReadServiceAsync<,>), typeof(ReadServiceAsync<,>));
builder.Services.AddScoped(typeof(IGenericServiceAsync<,>), typeof(GenericServiceAsync<,>));
//////////////////////////////////// Services ////////////////////////////////////

// Asset Mappings
builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));
builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));

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
    ProductDbContext context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    context.Database.Migrate();
}

app.Run();
