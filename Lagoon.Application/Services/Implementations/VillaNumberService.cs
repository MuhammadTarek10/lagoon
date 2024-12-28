using Lagoon.Application.Common.Interfaces;
using Lagoon.Application.Services.Interfaces;
using Lagoon.Domain.Entities;

namespace Lagoon.Application.Services.Implementations
{
    public class VillaNumberService : IVillaNumberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> CheckVillaNumberExists(int number)
        {
            return await _unitOfWork.VillaNumber.AnyAsync(u => u.Number == number);
        }

        public async Task CreateVillaNumberAsync(VillaNumber villaNumber)
        {
            await _unitOfWork.VillaNumber.AddAsync(villaNumber);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> DeleteVillaNumberAsync(int number)
        {
            try
            {
                VillaNumber? villaNumber = await _unitOfWork.VillaNumber.GetAsync(u => u.Number == number);
                if (villaNumber != null)
                {
                    _unitOfWork.VillaNumber.Remove(villaNumber);
                    await _unitOfWork.SaveAsync();
                    return true;
                }
                return false;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<VillaNumber>> GetAllVillaNumbersAsync()
        {
            return await _unitOfWork.VillaNumber.GetAllAsync(includeProperties: "Villa");
        }

        public async Task<VillaNumber?> GetVillaNumberByNumberAsync(int number)
        {
            return await _unitOfWork.VillaNumber.GetAsync(u => u.Number == number, includeProperties: "Villa");
        }

        public async Task UpdateVillaNumber(VillaNumber villaNumber)
        {
            _unitOfWork.VillaNumber.Update(villaNumber);
            await _unitOfWork.SaveAsync();
        }
    }
}
