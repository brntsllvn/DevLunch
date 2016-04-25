using GeneralStore.Models;
using Microsoft.Data.Entity;

namespace GeneralStore.Test.Fakes
{
    public class FakeApplicationDbContext:ApplicationDbContext
    {
        public FakeApplicationDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./test-database.db");
        }
    }
}