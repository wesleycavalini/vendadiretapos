using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendaDireta.Models;

namespace VendaDireta.Controllers
{
    public class CadastroController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            try
            {
                using (var model = new Model())
                {
                    if (model.Usuario.Any(x => x.Email == usuario.Email))
                    {
                        throw new Exception("E-mail já cadastrado!");
                    }
                    else
                    {
                        model.Usuario.Add(usuario);
                        model.SaveChanges();
                    }
                }
                return RedirectToAction("Index", "Login");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("erro", e.Message);
                return View(usuario);
            }
        }


    }
}