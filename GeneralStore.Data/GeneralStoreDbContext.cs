using System.Data.Entity;
using GeneralStore.Data.Models;

namespace GeneralStore.Data
{
    public class GeneralStoreDbContext:DbContext
    {
         public DbSet<Product> Products { get; set; } 

     
    }
}