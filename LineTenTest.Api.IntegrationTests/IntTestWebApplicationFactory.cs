using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LineTenTest.Domain.Entities;
using LineTenTest.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LineTenTest.Api.IntegrationTests
{
    public class IntTestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> 
        where TProgram : class

    {
        public static readonly string ConnectionString = "Data Source=TestDb.db";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                RemoveAppDbContextsFromServices(services);

                services.AddDbContext<AppDbContext>(options =>
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile($"appsettings.IntegrationTest.json", optional: false, reloadOnChange: true)
                        .Build();

                    var connectionString = configuration.GetConnectionString("OrderConnectionString");

                    var projectAssemblyName = Assembly.GetAssembly(typeof(IntTestWebApplicationFactory<>)).GetName().Name;
                    options.UseSqlite(connectionString, x => x.MigrationsAssembly(projectAssemblyName));
                });

                services.AddDbContext<AppDbContextSqlLite>();

                MigrateDbContext(services);
            });
            
            
        }

        private void RemoveAppDbContextsFromServices(IServiceCollection services)
        {
            var descriptors = services.Where(d => d.ServiceType.BaseType == typeof(DbContextOptions)).ToList();
            descriptors.ForEach(d => services.Remove(d));

            var dbContextDescriptors = services.Where(d => d.ServiceType.BaseType == typeof(DbContext)).ToList();
            dbContextDescriptors.ForEach(d => services.Remove(d));
        }

        public void MigrateDbContext(IServiceCollection serviceCollection)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();

            var services = scope.ServiceProvider;
            var context = services.GetService<AppDbContextSqlLite>();

            if (context.Database.IsSqlServer())
            {
                throw new Exception("Use Sqlite instead of sql server!");
            }

            context.Database.EnsureDeleted();
            context.Database.Migrate();
            var customers = ArrangeCustomers();
            context.Customers.AddRange(customers);
            var products = ArrangeProducts();
            context.Products.AddRange(products);
            var orders = ArrangeOrders(customers, products);
            context.Orders.AddRange(orders);
            context.SaveChanges();
        }

        private static List<Domain.Entities.Order> Arrange()
        {
            var customers = ArrangeCustomers();
            var products = ArrangeProducts();
            return ArrangeOrders(customers,products);
        }

        private static IEnumerable<Domain.Entities.Product> ArrangeProducts()
        {
            var products = new List<Domain.Entities.Product>();
            for (int i = 0; i < 10; i++)
            {
                var product = new Domain.Entities.Product()
                {
                    SKU = $"sku{i}",
                    Description = $"productDescription{i}",
                    Name = $"ProductName{i}"
                };
                products.Add(product);
            }

            return products;
        }

        private static IEnumerable<Domain.Entities.Customer> ArrangeCustomers()
        {
            var customers = new List<Domain.Entities.Customer>();
            for (int i = 0; i < 10; i++)
            {
                var customer = new Domain.Entities.Customer()
                {
                    FirstName = $"CustomerFirstName{i}",
                    LastName = $"CustomerLastName{i}",
                    Email = $"customerEmail{i}",
                    Phone = $"customerPhone{i}"
                };
                customers.Add(customer);
            }

            return customers;
        }

        private static List<Domain.Entities.Order> ArrangeOrders(IEnumerable<Domain.Entities.Customer> customers, IEnumerable<Domain.Entities.Product> products)
        {
            var orders = new List<Domain.Entities.Order>();
            var date = new DateTime(2023, 11, 25, 11, 12, 13);
            var customerList = customers.ToList();
            var productsList = products.ToList();
            for (int i = 0; i < 10; i++)
            {
                var order = new Domain.Entities.Order()
                {
                    CreatedDate = date.AddDays(i),
                    UpdatedDate = date.AddDays(i + 1),
                    Status = (int)EOrderStatus.Pending,
                    Customer = customerList[i],
                    Product = productsList[i]
                };

                orders.Add(order);
            }

            return orders;
        }
    }
}
