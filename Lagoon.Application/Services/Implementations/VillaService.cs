using Microsoft.AspNetCore.Hosting;
using Lagoon.Application.Common.Interfaces;
using Lagoon.Application.Services.Interfaces;
using Lagoon.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Lagoon.Application.Utilities;

namespace Lagoon.Application.Services.Implementations
{
    public class VillaService : IVillaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<VillaService> _logger;

        public VillaService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, ILogger<VillaService> logger)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task CreateVillaAsync(Villa villa)
        {
            try
            {
                if (villa.Image != null) villa.ImageUrl = SaveImage(villa.Image);
                else villa.ImageUrl = DefaultImageUrl;

                await _unitOfWork.Villa.AddAsync(villa);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding a villa");
                throw;
            }
        }

        public async Task<bool> DeleteVillaAsync(Guid id)
        {
            try
            {
                Villa? objFromDb = await _unitOfWork.Villa.GetAsync(u => u.Id == id);
                if (objFromDb != null)
                {
                    DeleteImage(objFromDb.ImageUrl);
                    _unitOfWork.Villa.Remove(objFromDb);
                    await _unitOfWork.SaveAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting the villa with ID {id}");
                return false;
            }
        }

        public async Task<IEnumerable<Villa>> GetAllVillasAsync()
        {
            return await _unitOfWork.Villa.GetAllAsync(includeProperties: "Amenities");
        }

        public async Task<Villa?> GetVillaByIdAsync(Guid id)
        {
            return await _unitOfWork.Villa.GetAsync(u => u.Id == id, includeProperties: "Amenities");
        }

        public async Task<IEnumerable<Villa>> GetVillasAvailabilityByDateAsync(int nights, DateOnly checkInDate)
        {
            IEnumerable<Villa> villaList = await _unitOfWork.Villa.GetAllAsync(includeProperties: "Amenities");
            IEnumerable<VillaNumber> villaNumbersList = await _unitOfWork.VillaNumber.GetAllAsync();
            IEnumerable<Booking> bookedVillas = await _unitOfWork.Booking.GetAllAsync(u => u.Status == SD.StatusApproved ||
            u.Status == SD.StatusCheckedIn);

            foreach (Villa villa in villaList)
            {
                int roomAvailable = SD.VillaRoomsAvailable_Count
                    (villa.Id,
                     villaNumbersList,
                     checkInDate,
                     nights,
                     bookedVillas);

                villa.IsAvailable = roomAvailable > 0 ? true : false;
            }

            return villaList;
        }

        public async Task<bool> IsVillaAvailableByDateAsync(Guid villaId, int nights, DateOnly checkInDate)
        {
            IEnumerable<VillaNumber> villaNumbersList = await _unitOfWork.VillaNumber.GetAllAsync();

            IEnumerable<Booking> bookedVillas = await _unitOfWork.Booking.GetAllAsync(u => u.Status == SD.StatusApproved ||
            u.Status == SD.StatusCheckedIn);

            int roomAvailable = SD.VillaRoomsAvailable_Count
                (villaId,
                 villaNumbersList,
                 checkInDate,
                 nights,
                 bookedVillas);

            return roomAvailable > 0;
        }

        public async Task UpdateVillaAsync(Villa villa)
        {
            try
            {
                if (villa.Image != null)
                {
                    DeleteImage(villa.ImageUrl);
                    villa.ImageUrl = SaveImage(villa.Image);
                }

                _unitOfWork.Villa.Update(villa);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating the villa with ID {villa.Id}");
                throw;
            }
        }

        private string SaveImage(IFormFile image)
        {
            string filename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images/villas");

            Directory.CreateDirectory(uploadPath); // Ensure the directory exists
            using var fileStream = new FileStream(Path.Combine(uploadPath, filename), FileMode.Create);
            image.CopyTo(fileStream);

            return $"/images/villas/{filename}";
        }

        private void DeleteImage(string? imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('\\'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        private string DefaultImageUrl => "https://placehold.co/600x400";
    }
}
