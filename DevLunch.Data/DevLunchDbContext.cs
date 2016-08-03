using System.Data.Common;
using System.Data.Entity;
using DevLunch.Data.Models;

namespace DevLunch.Data
{
    public class DevLunchDbContext:DbContext
    {
        public DevLunchDbContext():base("DefaultConnection")
        {
            
        }

        public DevLunchDbContext(DbConnection connection) : base(connection,contextOwnsConnection:true)
        {
            
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Lunch> Lunches { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}