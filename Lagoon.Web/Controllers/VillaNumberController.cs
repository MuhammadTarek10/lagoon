using Lagoon.Application.Common.Interfaces;
using Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Lagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VillaNumberController> _logger;

        public VillaNumberController(IUnitOfWork unitOfWork, ILogger<VillaNumberController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<VillaNumber> villaNumbers = _unitOfWork.VillaNumber.GetAll();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VillaNumber villaNumber)
        {
            if (!ModelState.IsValid) return NotFound();

            _unitOfWork.VillaNumber.Add(villaNumber);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int number)
        {
            VillaNumber? villaNumber = _unitOfWork.VillaNumber.Get(u => u.Number == number);

            if (villaNumber == null) return NotFound();

            return View(villaNumber);

        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(VillaNumber villaNumber)
        {
            if (!ModelState.IsValid) return NotFound();

            _unitOfWork.VillaNumber.Update(villaNumber);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int number)
        {
            VillaNumber? villaNumber = _unitOfWork.VillaNumber.Get(u => u.Number == number);

            if (villaNumber == null) return NotFound();

            return View(villaNumber);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(VillaNumber villaNumber)
        {
            if (villaNumber == null) return NotFound();

            _unitOfWork.VillaNumber.Remove(villaNumber);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
