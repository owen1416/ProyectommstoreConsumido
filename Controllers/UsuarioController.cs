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

namespace ProyectommstoreConsumido.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        [HttpGet]
        public ActionResult USUARIO_LISTADO()
        {
            List<Usuarios> lista = null;
            string url = "https://localhost:44380/api/usuario/getall";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = respuesta.Content.ReadAsStringAsync().Result;
                lista = JsonConvert.DeserializeObject<List<Usuarios>>(contenido);
                Debug.WriteLine(contenido);

            }
            else
            {
                Debug.WriteLine("Error.......");
                lista = new List<Usuarios>();

            }

            return View(lista);
        }


        [HttpGet]
        public ActionResult USUARIO_INSERTADO()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> USUARIO_INSERTADO(RegistroViewModel model)
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
                        return RedirectToAction("USUARIO_LISTADO");

                    }
                    else
                    {
                        // La API respondió con un error
                        var errorContent = await respuesta.Content.ReadAsStringAsync();
                        ModelState.AddModelError("USUARIO_LISTADO", $"Error al registrar: {errorContent}");
                        


                    }
                }

            }
            return PartialView("_CrearUsuarioModal", model);
        }

    }
}