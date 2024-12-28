using Lagoon.Domain.Entities;

namespace Lagoon.Application.Services.Interfaces
{
    public interface IVillaService
    {
        Task<IEnumerable<Villa>> GetAllVillasAsync();
        Task<Villa?> GetVillaByIdAsync(Guid id);
        Task CreateVillaAsync(Villa villa);
        Task UpdateVillaAsync(Villa villa);
        Task<bool> DeleteVillaAsync(Guid id);

        Task<IEnumerable<Villa>> GetVillasAvailabilityByDateAsync(int nights, DateOnly checkInDate);
        Task<bool> IsVillaAvailableByDateAsync(Guid villaId, int nights, DateOnly checkInDate);
    }
}

