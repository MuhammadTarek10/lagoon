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

    }
}
