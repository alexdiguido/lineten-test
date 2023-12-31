﻿using LineTenTest.Domain.Entities;
using LineTenTest.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace LineTenTest.Domain.Tests.Services;

public class DatabaseFixture : IDisposable
{
    public AppDbContext DbContext { get; private set; }

    public DatabaseFixture()
    {
        DbContext = SetupOrderContext();
        var orders = Arrange();
        DbContext.Orders.AddRange(orders);
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Dispose();
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

        private AppDbContext SetupOrderContext()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connectionString)
                .Options;

            var dbContext = new AppDbContext(options);

            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();

            return dbContext;
        }
}