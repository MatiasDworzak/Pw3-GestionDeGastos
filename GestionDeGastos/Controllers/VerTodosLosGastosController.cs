using Microsoft.AspNetCore.Mvc;

namespace GestionDeGastos.Controllers
{
    public class VerTodosLosGastosController : Controller
    {
        public IActionResult VerTodosLosGastos()
        {
            return View();
        }
    }
}
