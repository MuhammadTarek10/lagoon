using Lagoon.Application.Common.Interfaces;
using Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Web.ViewModels;

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
            IEnumerable<Amenity> ameneties = _unitOfWork.Amenity.GetAll();
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
        public IActionResult Create(Amenity amnety)
        {
            if (!ModelState.IsValid) return NotFound();

            _unitOfWork.Amenity.Add(amnety);
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
        public IActionResult Edit(Amenity amnety)
        {
            if (!ModelState.IsValid) return NotFound();

            _unitOfWork.Amenity.Update(amnety);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid id)
        {
            Amenity? amnety = _unitOfWork.Amenity.Get(u => u.Id == id);

            if (amnety == null) return NotFound();

            _unitOfWork.Amenity.Remove(amnety);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(Guid id)
        {
            Amenity? amnety = _unitOfWork.Amenity.Get(u => u.Id == id);

            if (amnety == null) return NotFound();

            return View(amnety);
        }
    }
}
