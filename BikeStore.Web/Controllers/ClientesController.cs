using Microsoft.AspNetCore.Mvc;

namespace BikeStore.Controllers
{
    public class ClientesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
