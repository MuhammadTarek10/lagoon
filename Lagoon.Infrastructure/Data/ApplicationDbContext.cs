using Lagoon.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lagoon.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public required DbSet<Villa> Villas { get; set; }
        public required DbSet<VillaNumber> VillaNumbers { get; set; }
        public required DbSet<Amenity> Amenities { get; set; }

    }
}
