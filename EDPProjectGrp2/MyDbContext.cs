﻿using EDPProjectGrp2.Models;
using Microsoft.EntityFrameworkCore;
namespace LearningAPI
{
    public class MyDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public MyDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder
        optionsBuilder)
        {
            string? connectionString = _configuration.GetConnectionString(
            "MyConnection");
            if (connectionString != null)
            {
                optionsBuilder.UseMySQL(connectionString);
            }
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderLog> OrderLogs { get; set; } 
        public DbSet<Order> Orders { get; set; } 
        public DbSet<OrderItem> OrderItems { get; set; }

    }
}