﻿using Microsoft.EntityFrameworkCore;

namespace LineTenTest.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}