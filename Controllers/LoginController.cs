using Newtonsoft.Json;
using ProyectommstoreConsumido.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
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
                        FormsAuthentication.SetAuthCookie(usuarios.NombreUsuario, false);

                        return RedirectToAction("Index", "Home");
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
            return RedirectToAction("LOGIN", "Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var cliente = new HttpClient())
                {
                    // Definir la URL dentro del método
                    string url = "https://localhost:44380/api/usuario";

                    // Crear el objeto anónimo para enviar a la API
                    var registroDtoApi = new RegistroDTOApi
                    {
                        NombreUsuario = model.NombreUsuario,
                        Contraseña = model.Contraseña,
                        Email = model.Email,

                    };

                    // Serializar el objeto a JSON
                    var json = JsonConvert.SerializeObject(registroDtoApi);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Hacer la petición POST a la API
                    var respuesta = await cliente.PostAsync(url, content);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Usuario registrado correctamente.";
                        return RedirectToAction("LOGIN");

                    }
                    else
                    {
                        // La API respondió con un error
                        var errorContent = await respuesta.Content.ReadAsStringAsync();
                        ModelState.AddModelError("", $"Error al registrar: {errorContent}");
                        return View(model);


                    }
                }

            }
            return View(model);
        }




    }

}










    
                
         
    

        

