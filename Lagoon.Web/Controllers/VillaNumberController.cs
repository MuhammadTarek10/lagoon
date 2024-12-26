using Lagoon.Application.Common.Interfaces;
using Lagoon.Domain.Entities;
using Lagoon.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            IEnumerable<VillaNumber> villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new VillaNumberVM
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                })
            };

            return View(villaNumberVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VillaNumber villaNumber)
        {
            if (!ModelState.IsValid) return BadRequest();

            _unitOfWork.VillaNumber.Add(villaNumber);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int number)
        {

            VillaNumberVM villaNumberVM = new VillaNumberVM
            {
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Number == number),
                VillaList = _unitOfWork.Villa.GetAll().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                })
            };

            return View(villaNumberVM);

        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(VillaNumber villaNumber)
        {
            if (!ModelState.IsValid) return BadRequest();

            _unitOfWork.VillaNumber.Update(villaNumber);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int number)
        {
            VillaNumber? villaNumber = _unitOfWork.VillaNumber.Get(u => u.Number == number);

            if (villaNumber == null) return NotFound();

            _unitOfWork.VillaNumber.Remove(villaNumber);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(VillaNumber villaNumber)
        {
            _unitOfWork.VillaNumber.Remove(villaNumber);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
