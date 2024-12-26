using Lagoon.Domain.Entities;

namespace Lagoon.Application.Common.Interfaces
{
    public interface IAmenityRepository : IRepository<Amenity>
    {
        void Update(Amenity entity);
    }

}
