using Microsoft.AspNetCore.Mvc;

namespace MyFinanceFy.Controllers
{
    public class PainelUsuarioController : Controller
    {
        private readonly ILogger<PainelUsuarioController> _logger;

        public PainelUsuarioController(ILogger<PainelUsuarioController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
