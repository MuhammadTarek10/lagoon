using Lagoon.Domain.Entities;

namespace Lagoon.Application.Common.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        void Update(Booking entity);
    }

}
