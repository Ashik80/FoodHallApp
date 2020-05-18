using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FoodHallApp.Models;

namespace FoodHallApp.Data
{
    public class FoodHallAppContext : DbContext
    {
        public FoodHallAppContext (DbContextOptions<FoodHallAppContext> options)
            : base(options)
        {
        }

        public DbSet<FoodHallApp.Models.User> User { get; set; }

        public DbSet<FoodHallApp.Models.Category> Category { get; set; }

        public DbSet<FoodHallApp.Models.Product> Product { get; set; }

        public DbSet<FoodHallApp.Models.Order> Order { get; set; }
        
        public DbSet<FoodHallApp.Models.OrderedProducts> OrderedProducts { get; set; }

        public DbSet<FoodHallApp.Models.Customer> Customer { get; set; }
    }
}
