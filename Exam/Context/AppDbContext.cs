using Exam.DataSeed;
using Exam.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Context
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PickupPoint> PickupPoints { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Data Source=DESKTOP-VJHT493;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;Packet Size=4096;Command Timeout=0;Database=ObuvZZZ";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
