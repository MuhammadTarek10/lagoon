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

        public async Task<IActionResult> Index()
        {
            IEnumerable<VillaNumber> villaNumbers = await _unitOfWork.VillaNumber.GetAllAsync(includeProperties: "Villa");
            return View(villaNumbers);
        }

        public async Task<IActionResult> Create()
        {
            VillaNumberVM villaNumberVM = new VillaNumberVM
            {
                VillaList = (await _unitOfWork.Villa.GetAllAsync()).Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                })
            };

            return View(villaNumberVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VillaNumber villaNumber)
        {
            if (!ModelState.IsValid) return BadRequest();

            await _unitOfWork.VillaNumber.AddAsync(villaNumber);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int number)
        {
            VillaNumber? villaNumber = await _unitOfWork.VillaNumber.GetAsync(u => u.Number == number);
            if (villaNumber == null) return NotFound();

            VillaNumberVM villaNumberVM = new VillaNumberVM
            {
                VillaNumber = villaNumber,
                VillaList = (await _unitOfWork.Villa.GetAllAsync()).Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                })
            };

            return View(villaNumberVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VillaNumber villaNumber)
        {
            if (!ModelState.IsValid) return BadRequest();

            _unitOfWork.VillaNumber.Update(villaNumber);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int number)
        {
            VillaNumber? villaNumber = await _unitOfWork.VillaNumber.GetAsync(u => u.Number == number);
            if (villaNumber == null) return NotFound();

            _unitOfWork.VillaNumber.Remove(villaNumber);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(VillaNumber villaNumber)
        {
            _unitOfWork.VillaNumber.Remove(villaNumber);
            await _unitOfWork.SaveAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
