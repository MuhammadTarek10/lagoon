using Lagoon.Domain.Entities;

namespace Lagoon.Application.Services.Interfaces
{
    public interface IVillaNumberService
    {
        Task<IEnumerable<VillaNumber>> GetAllVillaNumbersAsync();
        Task<VillaNumber?> GetVillaNumberByNumberAsync(int number);
        Task CreateVillaNumberAsync(VillaNumber villaNumber);
        Task UpdateVillaNumber(VillaNumber villaNumber);
        Task<bool> DeleteVillaNumberAsync(int number);

        Task<bool> CheckVillaNumberExists(int number);
    }
}
