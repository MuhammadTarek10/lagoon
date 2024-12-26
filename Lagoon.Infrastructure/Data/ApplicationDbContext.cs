using Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lagoon.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public required DbSet<Villa> Villas { get; set; }
        public required DbSet<VillaNumber> VillaNumbers { get; set; }
        public required DbSet<Amenity> Amenities { get; set; }

    }
}
