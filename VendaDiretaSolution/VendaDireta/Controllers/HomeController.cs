using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendaDireta.Models;

namespace VendaDireta.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Produto> produtos = null;

            using (var model = new Model())
            {
                produtos = model.Produto.Where(x => !x.Vendido).ToList();
            }

            return View(produtos);
        }

        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(Produto produto)
        {
            try
            {
                if (string.IsNullOrEmpty(produto.Nome))
                    throw new Exception("Nome do produto não informado");

                if (produto.Preco <= 0)
                    throw new Exception("Preço do produto não informado");

                var usuario = new Autenticacao().GetUsuarioLogado();

                using (var model = new Model())
                {
                    produto.Vendido = false;
                    produto.UsuarioId = usuario.UsuarioId;
                    model.Produto.Add(produto);
                    model.SaveChanges();
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("erro", ex.Message);

                return View(produto);
            }
        }

        public ActionResult Detalhe(int id)
        {
            Produto retorno = null;

            using (var model = new Model())
            {
                retorno = model.Produto.Find(id);
                retorno.Usuario = model.Usuario.Find(retorno.UsuarioId);
                retorno.Usuario.Produto = null;
            }

            return View(retorno);
        }

        [HttpPost]
        public ActionResult Detalhe(Produto produto)
        {
            try
            {
                using (var model = new Model())
                {
                    var obj = model.Produto.Find(produto.ProdutoId);

                    if (obj.Vendido)
                        throw new Exception("Produto já foi vendido!");

                    obj.Vendido = true;
                    obj.Usuario.Receita = obj.Usuario.Receita + obj.Preco;

                    model.SaveChanges();

                    new Autenticacao().Autenticar(obj.Usuario);
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("erro", ex.Message);

                return View(produto);
            }
        }

    }
}