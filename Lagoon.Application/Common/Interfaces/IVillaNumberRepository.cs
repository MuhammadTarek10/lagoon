
using Lagoon.Domain.Entities;

namespace Lagoon.Application.Common.Interfaces
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        void Update(VillaNumber entity);
    }
}
