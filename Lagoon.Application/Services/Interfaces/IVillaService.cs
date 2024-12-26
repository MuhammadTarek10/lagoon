using Lagoon.Domain.Entities;

namespace Lagoon.Application.Services.Interfaces
{
    public interface IVillaService
    {
        IEnumerable<Villa> GetAllVillas();
        Villa? GetVillaById(Guid id);
        void AddVilla(Villa villa);
        void UpdateVilla(Villa villa);
        bool DeleteVilla(Guid id);

        IEnumerable<Villa> GetVillasAvailabilityByDate(int nights, DateOnly checkInDate);
        bool IsVillaAvailableByDate(int villaId, int nights, DateOnly checkInDate);
    }

}
