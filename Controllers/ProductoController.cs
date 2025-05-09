
using Newtonsoft.Json;
using ProyectommstoreConsumido.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
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

        // Tamaño de página fijo (puedes configurarlo)
        private const int PageSize = 10;

        [HttpGet]
        public ActionResult PRODUCTO_LISTADO(int? page)
        {
            int pageNumber = (page ?? 1);
            List<Productos> todosLosProductos = null;

            string url = "https://localhost:44380/api/pro/getall";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;

            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = respuesta.Content.ReadAsStringAsync().Result;
                todosLosProductos = JsonConvert.DeserializeObject<List<Productos>>(contenido);
                Debug.WriteLine(contenido);
            }
            else
            {
                Debug.WriteLine("Error al obtener los productos de la API.");
                todosLosProductos = new List<Productos>();
            }

            // Paginación en el cliente
            int totalProductos = todosLosProductos.Count();
            int totalPages = (int)Math.Ceiling((double)totalProductos / PageSize);
            int skip = (pageNumber - 1) * PageSize;
            var productosPaginados = todosLosProductos
            .Skip(skip)
                                       .Take(PageSize)
                                       .ToList();

            var viewModel = new ProductoListaViewModel
            {
                Productos = productosPaginados,
                PaginaActual = pageNumber,
                TotalPaginas = totalPages
            };

            return View(viewModel);
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


        [HttpPost]
        public async Task<ActionResult> PRODUCTO_ELIMINADO(int id)
        {
            if (ModelState.IsValid)
            {
                string url = $"https://localhost:44380/api/pro/delete?id={id}";
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

        [HttpGet]
        public async Task<ActionResult> PRODUCTO_EDITADO(int id)
        {
            Productos producto = null;
            string url = $"https://localhost:44380/api/pro/{id}";
            HttpClient client = new HttpClient();
            var respuesta = await client.GetAsync(url);
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                producto = JsonConvert.DeserializeObject<Productos>(contenido);
            }
            else
            {
                ViewBag.ErrorMessage = "No se pudo cargar el producto.";
                producto = new Productos();
            }

            return View(producto);
        }

        [HttpPost]
        public async Task<ActionResult> PRODUCTO_EDITADO(Productos producto)
        {
            if (ModelState.IsValid)
            {
                string url = "https://localhost:44380/api/pro/editar";
                HttpClient client = new HttpClient();
                string jsonProducto = JsonConvert.SerializeObject(producto);
                var content = new StringContent(jsonProducto, System.Text.Encoding.UTF8, "application/json");

                var respuesta = await client.PutAsync(url, content);

                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction("PRODUCTO_LISTADO");
                }
                else
                {
                    Debug.WriteLine($"Error al editar producto: {respuesta.StatusCode}");
                    ViewBag.ErrorMessage = "Error al editar el producto. Inténtelo de nuevo.";
                }
            }

            return View(producto);
        }














    }











}
