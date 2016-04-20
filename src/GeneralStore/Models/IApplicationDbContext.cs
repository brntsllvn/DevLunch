using Microsoft.Data.Entity;

namespace GeneralStore.Models
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; set; }
    }
}