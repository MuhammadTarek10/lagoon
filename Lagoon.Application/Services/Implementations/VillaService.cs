using Microsoft.AspNetCore.Hosting;
using Lagoon.Application.Common.Interfaces;
using Lagoon.Application.Services.Interfaces;
using Lagoon.Domain.Entities;

namespace Lagoon.Application.Services.Implementation
{
    public class VillaService : IVillaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }


        public void AddVilla(Villa villa)
        {
            if (villa.Image is not null)
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images/villas");


                using var filestream = new FileStream(Path.Combine(uploadPath, filename), FileMode.Create);
                villa.Image.CopyTo(filestream);

                villa.ImageUrl = $"/images/villas/{filename}";

            }
            else villa.ImageUrl = "https://placehold.co/600x400";

            _unitOfWork.Villa.Add(villa);
            _unitOfWork.Save();
        }

        public bool DeleteVilla(Guid id)
        {
            try
            {
                Villa? objFromDb = _unitOfWork.Villa.Get(u => u.Id == id);
                if (objFromDb is not null)
                {
                    if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                    }
                    _unitOfWork.Villa.Remove(objFromDb);
                    _unitOfWork.Save();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Villa> GetAllVillas()
        {
            return _unitOfWork.Villa.GetAll(includeProperties: "Amenities");
        }

        public Villa? GetVillaById(Guid id)
        {
            return _unitOfWork.Villa.Get(u => u.Id == id, includeProperties: "Amenities");
        }

        public IEnumerable<Villa> GetVillasAvailabilityByDate(int nights, DateOnly checkInDate)
        {
            // WARN: Implement this method
            throw new NotImplementedException();
        }

        public bool IsVillaAvailableByDate(int villaId, int nights, DateOnly checkInDate)
        {
            // WARN: Implement this method
            throw new NotImplementedException();
        }

        public void UpdateVilla(Villa villa)
        {
            if (villa.Image is not null)
            {
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images/villas");

                if (!string.IsNullOrEmpty(villa.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                }

                using var filestream = new FileStream(Path.Combine(uploadPath, filename), FileMode.Create);
                villa.Image.CopyTo(filestream);

                villa.ImageUrl = $"/images/villas/{filename}";
            }

            _unitOfWork.Villa.Update(villa);
            _unitOfWork.Save();
        }
    }
}
