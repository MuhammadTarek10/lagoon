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

        public async Task<IActionResult> Index()
        {
            IEnumerable<Villa> villas = await _villaService.GetAllVillasAsync();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Villa villa)
        {
            if (!ModelState.IsValid) return View(villa);

            await _villaService.AddVillaAsync(villa);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            Villa? villa = await _villaService.GetVillaByIdAsync(id);

            if (villa == null) return NotFound();

            return View(villa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Villa villa)
        {
            if (!ModelState.IsValid) return View(villa);

            await _villaService.UpdateVillaAsync(villa);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            Villa? villa = await _villaService.GetVillaByIdAsync(id);

            if (villa == null) return NotFound();

            return View(villa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            bool result = await _villaService.DeleteVillaAsync(id);
            if (!result) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
