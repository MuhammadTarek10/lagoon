using Lagoon.Application.Common.Interfaces;
using Lagoon.Application.Services.Interfaces;
using Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Lagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly ILogger<VillaController> _logger;

        public VillaController(IVillaService villaService, ILogger<VillaController> logger)
        {
            _villaService = villaService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<Villa> villas = _villaService.GetAllVillas();

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

            _villaService.AddVilla(villa);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid id)
        {
            Villa? villa = _villaService.GetVillaById(id);

            if (villa == null) return NotFound();

            return View(villa);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Villa villa)
        {
            if (!ModelState.IsValid) return NotFound();

            _villaService.UpdateVilla(villa);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid id)
        {
            Villa? villa = _villaService.GetVillaById(id);

            if (villa == null) return NotFound();

            return View(villa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Villa villa)
        {
            if (villa == null) return NotFound();

            _villaService.DeleteVilla(villa.Id);

            return RedirectToAction(nameof(Index));
        }
    }
}
