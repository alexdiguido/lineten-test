using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineTenTest.Domain.Entities;
using LineTenTest.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LineTenTest.Api.IntegrationTests
{
    public class IntTestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> 
        where TProgram : class

    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<AppDbContext>));

                services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbConnection));

                services.Remove(dbConnectionDescriptor);

                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();

                    return connection;
                });


                services.AddDbContext<AppDbContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });

                SeedInMemoryDatabase(services);
            });
        }

        private void SeedInMemoryDatabase(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<AppDbContext>();

                dbContext.Database.OpenConnection();
                dbContext.Database.EnsureCreated();

                try
                {
                    var customers = ArrangeCustomers();
                    dbContext.Customers.AddRange(customers);
                    var products = ArrangeProducts();
                    dbContext.Products.AddRange(products);
                    var orders = ArrangeOrders(customers,products);
                    dbContext.Orders.AddRange(orders);
                    dbContext.SaveChanges();
                }
                finally
                {
                    dbContext.Database.CloseConnection();
                }
            }
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
