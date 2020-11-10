using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KendamaShop.Models
{
    public class AppContext : DbContext
    {
        public AppContext() : base("DBConnectionString")
        {
        }

        public DbSet<Product> Products { get; set; }
        // public DbSet<Article> Articles { get; set; }
        // public DbSet<Category> Categories { get; set; }
        // public DbSet<Comment> Comments { get; set; }
    }
}