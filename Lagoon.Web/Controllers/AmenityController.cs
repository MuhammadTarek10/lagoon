using Lagoon.Application.Common.Interfaces;
using Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lagoon.Web.ViewModels;
using Lagoon.Application.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace Lagoon.Web.Controllers
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AmenityController> _logger;

        public AmenityController(IUnitOfWork unitOfWork, ILogger<AmenityController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Amenity> amenities = await _unitOfWork.Amenity.GetAllAsync(includeProperties: "Villa");
            return View(amenities);
        }

        public async Task<IActionResult> Create()
        {
            AmenityVM amenityVM = new AmenityVM
            {
                VillaList = (await _unitOfWork.Villa.GetAllAsync()).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(amenityVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Amenity amenity)
        {
            if (!ModelState.IsValid) return BadRequest();

            await _unitOfWork.Amenity.AddAsync(amenity);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            Amenity? amenity = await _unitOfWork.Amenity.GetAsync(u => u.Id == id);
            if (amenity == null) return NotFound();

            AmenityVM amenityVM = new()
            {
                Amenity = amenity,
                VillaList = (await _unitOfWork.Villa.GetAllAsync()).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            return View(amenityVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Amenity amenity)
        {
            if (!ModelState.IsValid) return BadRequest();

            _unitOfWork.Amenity.Update(amenity);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            Amenity? amenity = await _unitOfWork.Amenity.GetAsync(u => u.Id == id);

            if (amenity == null) return NotFound();

            _unitOfWork.Amenity.Remove(amenity);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(Guid id)
        {
            Amenity? amenity = await _unitOfWork.Amenity.GetAsync(u => u.Id == id);

            if (amenity == null) return NotFound();

            return View(amenity);
        }
    }
}
