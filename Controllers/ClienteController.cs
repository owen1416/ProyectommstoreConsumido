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

namespace ProyectommstoreConsumido.Controllers
{
    public class ClienteController : Controller
    {

        [HttpGet]
        public ActionResult CLIENTE_LISTADO()
        {
            List<Clientes> lista = null;
            string url = "https://localhost:44380/api/cliente/getall";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = respuesta.Content.ReadAsStringAsync().Result;
                lista = JsonConvert.DeserializeObject<List<Clientes>>(contenido);
                Debug.WriteLine(contenido);

            }
            else
            {
                Debug.WriteLine("Error.......");
                lista = new List<Clientes>();

            }

            return View(lista);
        }

        [HttpGet]
        public ActionResult CLIENTE_INSERTADO()
        {
            return View();
        }


        [HttpPost]

        public async Task<ActionResult> CLIENTE_INSERTADO(Clientes ciente)
        {
            if (ModelState.IsValid)
            {


                string url = "https://localhost:44380/api/cliente";
                HttpClient client = new HttpClient();
                string jsonCliente = JsonConvert.SerializeObject(ciente);
                var content = new StringContent(jsonCliente, System.Text.Encoding.UTF8, "application/json");
                var respuesta = await client.PostAsync(url, content);

                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction("CLIENTE_LISTADO");
                }
                else
                {
                    Debug.WriteLine($"Error al registrar producto: {respuesta.StatusCode}");

                    ViewBag.ErrorMessage = "Error al registrar el producto. Inténtelo de nuevo.";

                }
            }


            return View("_CrearClienteModal", ciente);
        }



        [HttpPost] // Mantenemos HttpPost para el formulario
        public async Task<ActionResult> CLIENTE_ELIMINADO(int id)
        {
            if (ModelState.IsValid)
            {
                string url = $"https://localhost:44380/api/cliente/{id}";
                HttpClient client = new HttpClient();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);

                try
                {
                    HttpResponseMessage respuesta = await client.SendAsync(request);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        TempData["MensajeEliminacion"] = "El producto se eliminó correctamente.";
                        TempData["Tipo"] = "success";
                    }
                    else
                    {
                        TempData["Mensaje"] = "No se pudo eliminar el producto";
                        TempData["Tipo"] = "error";
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Excepción al llamar a la API de eliminación: {ex.Message}");
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar eliminar el producto.";
                }
            }
            return RedirectToAction("CLIENTE_LISTADO");
        }


        [HttpGet]
        public async Task<ActionResult> MostrarDetallesCliente(int id)
        {
            Clientes clientes = null;
            string url = $"https://localhost:44380/api/cliente/{id}";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                clientes = JsonConvert.DeserializeObject<Clientes>(contenido);
                Debug.WriteLine(contenido);
            }
            else
            {
                Debug.WriteLine("Error al obtener detalles del producto.");
                clientes = new Clientes();
            }

            // Asumiendo que solo esperas un producto, toma el primero de la lista
            return PartialView("_DetallesClientesPartial", clientes);
        }

    }
}