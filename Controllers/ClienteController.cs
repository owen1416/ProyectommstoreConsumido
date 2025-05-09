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
        // Tamaño de página fijo (puedes configurarlo)
        private const int PageSize = 10;

        [HttpGet]
        public ActionResult CLIENTE_LISTADO(int? page)
        {
            int pageNumber = (page ?? 1);
            List<Clientes> todosLosClientes = null;

            string url = "https://localhost:44380/api/cliente/getall";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;

            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = respuesta.Content.ReadAsStringAsync().Result;
                todosLosClientes = JsonConvert.DeserializeObject<List<Clientes>>(contenido);
                Debug.WriteLine(contenido);
            }
            else
            {
                Debug.WriteLine("Error al obtener los clientes de la API.");
                todosLosClientes = new List<Clientes>();
            }

            // Paginación en el cliente
            int totalClientes = todosLosClientes.Count();
            int totalPages = (int)Math.Ceiling((double)totalClientes / PageSize);
            int skip = (pageNumber - 1) * PageSize;
            var clientesPaginados = todosLosClientes
                                    .Skip(skip)
                                    .Take(PageSize)
                                    .ToList();

            var viewModel = new ClienteListaViewModel
            {
                Clientes = clientesPaginados,
                PaginaActual = pageNumber,
                TotalPaginas = totalPages
            };

            return View(viewModel);
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

        [HttpPost]
        public async Task<ActionResult> CLIENTE_ELIMINADO(int id)
        {
            if (ModelState.IsValid)
            {
                string url = $"https://localhost:44380/api/cliente/{id}";
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

        [HttpGet]
        public async Task<ActionResult> CLIENTE_EDITADO(int id)
        {
            Clientes  clientes = null;
            string url = $"https://localhost:44380/api/cliente/{id}";
            HttpClient client = new HttpClient();
            var respuesta = await client.GetAsync(url);
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                clientes = JsonConvert.DeserializeObject<Clientes>(contenido);
            }
            else
            {
                ViewBag.ErrorMessage = "No se pudo cargar el cliente.";
                clientes = new Clientes();
            }

            return View(clientes);
        }


        [HttpPost]
        public async Task<ActionResult> CLIENTE_EDITADO(Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                string url = "https://localhost:44380/api/cliente/editar";
                HttpClient client = new HttpClient();
                string jsonCliente = JsonConvert.SerializeObject(clientes);
                var content = new StringContent(jsonCliente, System.Text.Encoding.UTF8, "application/json");

                var respuesta = await client.PutAsync(url, content);

                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction("CLIENTE_LISTADO");
                }
                else
                {
                    Debug.WriteLine($"Error al editar cliente: {respuesta.StatusCode}");
                    ViewBag.ErrorMessage = "Error al editar el cliente. Inténtelo de nuevo.";
                }
            }

            return View(clientes);
        }


    }
}