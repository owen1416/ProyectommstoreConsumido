using Newtonsoft.Json;
using ProyectommstoreConsumido.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProyectommstoreConsumido.Controllers
{
    public class LoginController : Controller
    {

        [HttpGet]
        public ActionResult LOGIN()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> LOGIN(Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {

                string url = "https://localhost:44380/api/auth/login";
                HttpClient client = new HttpClient();
                var loginData = new { NombreUsuario = usuarios.NombreUsuario, password = usuarios.password };
                string jsonLogin = JsonConvert.SerializeObject(loginData);

                var content = new StringContent(jsonLogin, System.Text.Encoding.UTF8, "application/json");
                var respuesta = client.PostAsync(url, content).Result;

                if (respuesta.IsSuccessStatusCode)
                {
                    var respuestaContent = await respuesta.Content.ReadAsStringAsync();
                    var resultadoLogin = JsonConvert.DeserializeObject<Usuarios>(respuestaContent);

                    if (resultadoLogin != null && resultadoLogin.IsAuthenticated)
                    {
                        Session["UserId"] = resultadoLogin.UsuarioID;
                        Session["AuthToken"] = resultadoLogin.Token;
                        FormsAuthentication.SetAuthCookie(usuarios.NombreUsuario, false);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", resultadoLogin?.ErrorMessage ?? "Nombre de usuario o password incorrectos");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Error al intentar iniciar sesion, por fabor intente denuebo");
                    System.Diagnostics.Debug.WriteLine($"Error al llamar a la api de login: {respuesta.StatusCode}");
                }
            }

            return View(usuarios);
        }


            
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }

    
   

}







    
                
         
    

        

