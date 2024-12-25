using Lagoon.Infrastructure.Data;
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
            var villas = _context.Villas.ToList();
            _logger.LogInformation("Villas fetched successfully");
            return View(villas);
        }
    }
}
