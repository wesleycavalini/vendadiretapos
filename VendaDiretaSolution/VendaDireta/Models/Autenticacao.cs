using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace VendaDireta.Models
{
    public class Autenticacao
    {

        public void Sair()
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("usuario", ""));
        }

        public void Autenticar(Usuario usuario)
        {
            string data = new JavaScriptSerializer().Serialize(new Usuario
            {
                UsuarioId = usuario.UsuarioId,
                Nome = usuario.Nome,
                Receita = usuario.Receita
            });

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, usuario.UsuarioId.ToString(), DateTime.Now, DateTime.Now.AddYears(1), true, data);

            string cookieData = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieData)
            {
                HttpOnly = true,
                Expires = ticket.Expiration
            };

            HttpContext.Current.Response.Cookies.Add(cookie);

            cookie = new HttpCookie("usuario", data);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public Usuario GetUsuarioLogado()
        {
            Usuario usuario = null;

            try
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                    usuario = new JavaScriptSerializer().Deserialize(ticket.UserData, typeof(Usuario)) as Usuario;
                }
            }
            catch
            {

            }
            return usuario;
        }
    }
}