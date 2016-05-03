using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DevLunch.Data.Models;

namespace DevLunch.Data
{
    public class GeneralStoreDbContext:DbContext
    {
        public GeneralStoreDbContext()
        {
            
        }

        public GeneralStoreDbContext(DbConnection connection):base(connection,contextOwnsConnection:true)
        {
            
        }
         public DbSet<Product> Products { get; set; } 

     
    }
}