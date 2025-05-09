using Newtonsoft.Json;
using ProyectommstoreConsumido.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
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

        // Tamaño de página fijo (puedes configurarlo)
        private const int PageSize = 2;
   
        [HttpGet]
        public ActionResult USUARIO_LISTADO(int? page)
        {
            int pageNumber = (page ?? 1);
            List<Usuarios> todosLosUsuarios = null;

            string url = "https://localhost:44380/api/usuario/getall";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;

            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = respuesta.Content.ReadAsStringAsync().Result;
                todosLosUsuarios = JsonConvert.DeserializeObject<List<Usuarios>>(contenido);
                Debug.WriteLine(contenido);
            }
            else
            {
                Debug.WriteLine("Error al obtener los usuarios de la API.");
                todosLosUsuarios = new List<Usuarios>();
            }

            // Paginación en el cliente
            int totalUsuarios = todosLosUsuarios.Count();
            int totalPages = (int)Math.Ceiling((double)totalUsuarios / PageSize);
            int skip = (pageNumber - 1) * PageSize;
            var usuariosPaginados = todosLosUsuarios
            .Skip(skip)
                                    .Take(PageSize)
                                    .ToList();

            var viewModel = new UsuarioListaViewModel
            {
                Usuarios = usuariosPaginados,
                PaginaActual = pageNumber,
                TotalPaginas = totalPages
            };

            return View(viewModel);
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
        [HttpGet]
        [HttpPost]
        public async Task<ActionResult> USUARIO_ELIMINADO(int id)
        {
            if (ModelState.IsValid)
            {
                string url = $"https://localhost:44380/api/usuario/{id}";
                using (HttpClient client = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
                    try
                    {
                        await client.SendAsync(request);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Excepción al llamar a la API de eliminación: {ex.Message}");
                    }
                }
            }
            return RedirectToAction("USUARIO_LISTADO");
        }


        [HttpGet]
        public async Task<ActionResult> MostrarDetallesUsuario(int id)
        {
            Usuarios usuarios = null;
            string url = $"https://localhost:44380/api/usuario/{id}";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                usuarios = JsonConvert.DeserializeObject<Usuarios>(contenido);
                Debug.WriteLine(contenido);
            }
            else
            {
                Debug.WriteLine("Error al obtener detalles del producto.");
                usuarios = new Usuarios();
            }

            // Asumiendo que solo esperas un producto, toma el primero de la lista
            return PartialView("_DetallesUsuarioPartial", usuarios);
        }

        [HttpGet]
        public async Task<ActionResult> USUARIO_EDITADO(int id)
        {
            Usuarios usuario = null;
            string url = $"https://localhost:44380/api/usuario/{id}";
            using (var client = new HttpClient())
            {
                var respuesta = await client.GetAsync(url);
                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    usuario = JsonConvert.DeserializeObject<Usuarios>(contenido);
                    Debug.WriteLine(contenido);
                }
                else
                {
                    Debug.WriteLine("Error al obtener usuario para editar.");
                    return RedirectToAction("USUARIO_LISTADO");
                }
            }
            return View(usuario);
        }

        // POST: USUARIO_EDITADO (envía el PUT)
        [HttpPost]
        public async Task<ActionResult> USUARIO_EDITADO(Usuarios model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string url = $"https://localhost:44380/api/usuario/{model.UsuarioID}";
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var respuesta = await client.PutAsync(url, content);
                if (respuesta.IsSuccessStatusCode)
                {
                    TempData["Mensaje"] = "Usuario actualizado correctamente.";
                    TempData["Tipo"] = "success";
                    return RedirectToAction("USUARIO_LISTADO");
                }
                else
                {
                    var error = await respuesta.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Error al actualizar: {error}");
                    TempData["Tipo"] = "error";
                }
            }

            return View(model);
        }




    }
}