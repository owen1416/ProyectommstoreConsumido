
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
using System.Web.Razor.Generator;

namespace ProyectommstoreConsumido.Controllers
{
    public class ProductoController : Controller
    {
        // GET: Producto

        [HttpGet]
        public ActionResult PRODUCTO_LISTADO()
        {
            List<Productos> lista = null;
            string url = "https://localhost:44380/api/pro/getall";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = respuesta.Content.ReadAsStringAsync().Result;
                lista = JsonConvert.DeserializeObject<List<Productos>>(contenido);
                Debug.WriteLine(contenido);

            }
            else
            {
                Debug.WriteLine("Error.......");
                lista = new List<Productos>();

            }

            return View(lista);
        }

        [HttpGet]
        public ActionResult PRODUCTO_INSERTADO()
        {


            return View();
        }


        [HttpPost]

        public async  Task<ActionResult> PRODUCTO_INSERTADO(Productos producto)
        {
            if (ModelState.IsValid)
            {
                string url = "https://localhost:44380/api/pro";
                HttpClient client = new HttpClient();
                string jsonProducto = JsonConvert.SerializeObject(producto);
                var content = new StringContent(jsonProducto, System.Text.Encoding.UTF8, "application/json");
                var respuesta = await client.PostAsync(url, content);

                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction("PRODUCTO_LISTADO");
                }
                else
                {
                    Debug.WriteLine($"Error al registrar producto: {respuesta.StatusCode}");

                    ViewBag.ErrorMessage = "Error al registrar el producto. Inténtelo de nuevo.";

                }
            }
            return View("_CrearProductoModal", producto);
        }


        [HttpPost] // Mantenemos HttpPost para el formulario
        public async Task<ActionResult> PRODUCTO_ELIMINADO(int id)
        {
            if (ModelState.IsValid)
            {
                string url = $"https://localhost:44380/api/pro/delete?id={id}";
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
            return RedirectToAction("PRODUCTO_LISTADO");
        }
        
        [HttpGet]
        public  async Task<ActionResult> MostrarDetallesProducto(int id)
        {
            Productos producto = null;
            string url = $"https://localhost:44380/api/pro/{id}";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                producto = JsonConvert.DeserializeObject<Productos>(contenido);
                Debug.WriteLine(contenido);
            }
            else
            {
                Debug.WriteLine("Error al obtener detalles del producto.");
                producto = new Productos();
            }

            // Asumiendo que solo esperas un producto, toma el primero de la lista
            return PartialView("_DetallesProductoPartial", producto);
        }














    }











}
