using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClientApp.Models;

namespace ClientApp.Data
{
    public class ClientAppContext : DbContext
    {
        public ClientAppContext (DbContextOptions<ClientAppContext> options)
            : base(options)
        {
        }

        public DbSet<ClientApp.Models.Customer> Customer { get; set; }

        public DbSet<ClientApp.Models.Product> Product { get; set; }

        public DbSet<ClientApp.Models.Category> Category { get; set; }

        public DbSet<ClientApp.Models.Cart> Cart { get; set; }

        public DbSet<ClientApp.Models.Order> Order { get; set; }

        public DbSet<ClientApp.Models.OrderedProducts> OrderedProducts { get; set; }
    }
}
