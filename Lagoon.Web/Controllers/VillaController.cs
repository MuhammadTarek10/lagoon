using Lagoon.Application.Common.Interfaces;
using Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Lagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VillaController> _logger;

        public VillaController(IUnitOfWork unitOfWork, ILogger<VillaController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<Villa> villas = _unitOfWork.Villa.GetAll();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Villa villa)
        {
            if (!ModelState.IsValid) return NotFound();

            _unitOfWork.Villa.Add(villa);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid id)
        {
            Villa? villa = _unitOfWork.Villa.Get(u => u.Id == id);

            if (villa == null) return NotFound();

            return View(villa);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Villa villa)
        {
            if (!ModelState.IsValid) return NotFound();

            _unitOfWork.Villa.Update(villa);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid id)
        {
            Villa? villa = _unitOfWork.Villa.Get(u => u.Id == id);

            if (villa == null) return NotFound();

            return View(villa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Villa villa)
        {
            if (villa == null) return NotFound();

            _unitOfWork.Villa.Remove(villa);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}
