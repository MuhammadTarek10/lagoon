using Lagoon.Application.Common.Interfaces;
using Lagoon.Infrastructure.Data;

namespace Lagoon.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IVillaRepository Villa { get; private set; }
        public IVillaNumberRepository VillaNumber { get; private set; }
        public IAmenityRepository Amenity { get; private set; }
        public IBookingRepository Booking { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Villa = new VillaRepository(_context);
            VillaNumber = new VillaNumberRepository(_context);
            Amenity = new AmenityRepository(_context);
            Booking = new BookingRepository(_context);
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
