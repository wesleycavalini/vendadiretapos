using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendaDireta.Models;

namespace VendaDireta.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            new Autenticacao().Sair();

            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            try
            {
                using (var model = new Model())
                {
                    var obj = model.Usuario.FirstOrDefault(x => x.Email == usuario.Email && x.Senha == usuario.Senha);
                    if (obj != null)
                    {
                        new Autenticacao().Autenticar(obj);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        throw new Exception("Usuário/senha inválidos");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("erro", ex.Message);
                usuario.Produto = null;
                return View(usuario);
            }
        }
    }
}