using Microsoft.AspNetCore.Mvc;

namespace BikeStore.Controllers
{
    public class VentasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
