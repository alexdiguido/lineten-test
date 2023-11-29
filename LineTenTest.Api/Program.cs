using System.Reflection;
using Asp.Versioning;
using LineTenTest.Domain.Entities;
using LineTenTest.Domain.Services.Customer;
using LineTenTest.Domain.Services.Order;
using LineTenTest.Domain.Services.Product;
using LineTenTest.Infrastructure;
using LineTenTest.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGetCustomerService, GetCustomerService>();
builder.Services.AddScoped<IUpdateCustomerService, UpdateCustomerService>();
builder.Services.AddScoped<ICreateCustomerService, CreateCustomerService>();
builder.Services.AddScoped<IDeleteCustomerService, DeleteCustomerService>();

builder.Services.AddScoped<IDeleteProductService, DeleteProductService>();
builder.Services.AddScoped<IGetProductService, GetProductService>();
builder.Services.AddScoped<ICreateProductService, CreateProductService>();
builder.Services.AddScoped<IUpdateProductService, UpdateProductService>();

builder.Services.AddScoped<IGetOrderService, GetOrderService>();
builder.Services.AddScoped<ICreateOrderService, CreateOrderService>();
builder.Services.AddScoped<IUpdateOrderService, UpdateOrderService>();
builder.Services.AddScoped<IDeleteOrderService, DeleteOrderService>();

builder.Services.AddTransient<IRepository<Order>, EfRepository<Order>>();
builder.Services.AddTransient<IRepository<Product>, EfRepository<Product>>();
builder.Services.AddTransient<IRepository<Customer>, EfRepository<Customer>>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderConnectionString"), builder =>
        builder.EnableRetryOnFailure());
});

var app = builder.Build();

if (!string.Equals(app.Environment.EnvironmentName, "IntegrationTest", StringComparison.OrdinalIgnoreCase))
{
    using var serviceScope = app.Services.CreateScope();
    var provider = serviceScope.ServiceProvider;
    var dbContext = provider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
