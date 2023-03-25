using System;
using System.Collections.Generic;
using BlazorAppEditTable.Services;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppEditTable.Data;

public partial class MyDbContext : DbContext
{
    private readonly IConfiguration? _configuration;
    public virtual DbSet<TableStructure> TableStructures { get; set; } = null!;

    public MyDbContext(DbContextOptions<MyDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            if (_configuration != null)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
