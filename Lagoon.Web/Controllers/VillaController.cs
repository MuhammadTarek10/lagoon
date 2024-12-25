using Lagoon.Infrastructure.Data;
using Lagoon.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Lagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VillaController> _logger;

        public VillaController(ApplicationDbContext context, ILogger<VillaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            IEnumerable<Villa> villas = _context.Villas.ToList();
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

            _context.Villas.Add(villa);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(Guid id)
        {

            Villa? villa = _context.Villas.Find(id);

            if (villa == null) return NotFound();

            return View(villa);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Villa villa)
        {
            if (!ModelState.IsValid) return NotFound();

            _context.Villas.Update(villa);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(Guid id)
        {
            Villa? villa = _context.Villas.Find(id);

            if (villa == null) return NotFound();

            return View(villa);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Villa villa)
        {
            if (villa == null) return NotFound();

            _context.Villas.Remove(villa);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
