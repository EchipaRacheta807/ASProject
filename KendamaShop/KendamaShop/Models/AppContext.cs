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
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppContext, 
                                    KendamaShop.Migrations.Configuration>("DBConnectionString"));
        }

        public DbSet<Product> Products { get; set; }
        // public DbSet<Article> Articles { get; set; }
        // public DbSet<Category> Categories { get; set; }
        // public DbSet<Comment> Comments { get; set; }
    }
}