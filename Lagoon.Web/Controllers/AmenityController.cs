using Lagoon.Application.Common.Interfaces;
using Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lagoon.Web.ViewModels;

namespace Lagoon.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AmenityController> _logger;

        public AmenityController(IUnitOfWork unitOfWork, ILogger<AmenityController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<Amenity> ameneties = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(ameneties);
        }

        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(amenityVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Amenity amenity)
        {
            if (!ModelState.IsValid) return BadRequest();

            _unitOfWork.Amenity.Add(amenity);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid id)
        {
            AmenityVM amenityVM = new()
            {
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == id),
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            return View(amenityVM);

        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Amenity amenity)
        {
            if (!ModelState.IsValid) return BadRequest();

            _unitOfWork.Amenity.Update(amenity);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid id)
        {
            Amenity? amenity = _unitOfWork.Amenity.Get(u => u.Id == id);

            if (amenity == null) return NotFound();

            _unitOfWork.Amenity.Remove(amenity);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(Guid id)
        {
            Amenity? amenity = _unitOfWork.Amenity.Get(u => u.Id == id);

            if (amenity == null) return NotFound();

            return View(amenity);
        }
    }
}
