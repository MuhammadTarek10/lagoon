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
    }
}
