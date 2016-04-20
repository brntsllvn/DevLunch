using System;
using Microsoft.Data.Entity;

namespace GeneralStore.Models
{
    public class FakeApplicationDbContext : IApplicationDbContext
    {
        public DbSet<Product> Products
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
