using GestionDeGastos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeGastos.Controllers
{
   public class IngresoController : Controller
   {
      public ActionResult Login()
      {
         return View();
      }


      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Login(CredencialViewModel model)
      {
         return View();
      }

      // GET: IngresoController/Create
      public ActionResult Create()
      {
         return View();
      }

      // POST: IngresoController/Create
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create(IFormCollection collection)
      {
         try
         {
            return RedirectToAction(nameof(Index));
         }
         catch
         {
            return View();
         }
      }

      // GET: IngresoController/Edit/5
      public ActionResult Edit(int id)
      {
         return View();
      }

      // POST: IngresoController/Edit/5
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit(int id, IFormCollection collection)
      {
         try
         {
            return RedirectToAction(nameof(Index));
         }
         catch
         {
            return View();
         }
      }

      // GET: IngresoController/Delete/5
      public ActionResult Delete(int id)
      {
         return View();
      }

      // POST: IngresoController/Delete/5
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Delete(int id, IFormCollection collection)
      {
         try
         {
            return RedirectToAction(nameof(Index));
         }
         catch
         {
            return View();
         }
      }
   }
}
