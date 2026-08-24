using Microsoft.AspNetCore.Mvc;

namespace BikeStore.Controllers
{
    public class CategoriasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
