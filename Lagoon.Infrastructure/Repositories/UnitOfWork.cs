using Lagoon.Application.Common.Interfaces;
using Lagoon.Infrastructure.Data;

namespace Lagoon.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IVillaRepository Villa { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Villa = new VillaRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
