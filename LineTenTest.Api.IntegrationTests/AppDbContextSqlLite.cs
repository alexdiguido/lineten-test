using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineTenTest.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LineTenTest.Api.IntegrationTests
{
    public class AppDbContextSqlLite : AppDbContext
    {
        public AppDbContextSqlLite() { }
        public AppDbContextSqlLite(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(IntTestWebApplicationFactory<Program>.ConnectionString);
        }
    }
}
