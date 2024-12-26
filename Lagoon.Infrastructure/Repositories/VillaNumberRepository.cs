using Lagoon.Application.Common.Interfaces;
using Lagoon.Domain.Entities;
using Lagoon.Infrastructure.Data;

namespace Lagoon.Infrastructure.Repositories
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _context;
        public VillaNumberRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(VillaNumber entity)
        {
            _context.VillaNumbers.Update(entity);
        }
    }
}
