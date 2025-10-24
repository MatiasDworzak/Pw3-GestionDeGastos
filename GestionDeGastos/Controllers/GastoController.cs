using Microsoft.AspNetCore.Mvc;

namespace GestionDeGastos.Controllers
{
    public class GastoController : Controller
    {
        public IActionResult Agregar()
        {
            return View();
        }
    }
}
