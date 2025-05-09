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
    public class DetallePedidoController : Controller
    {

        [HttpGet]
        public ActionResult DETALLE_PEDIDO_LISTADO()
        {
            List<DetallePedido> lista = null;
            string url = "https://localhost:44380/api/detalle/getall";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = respuesta.Content.ReadAsStringAsync().Result;
                lista = JsonConvert.DeserializeObject<List<DetallePedido>>(contenido);
                Debug.WriteLine(contenido);

            }
            else
            {
                Debug.WriteLine("Error.......");
                lista = new List<DetallePedido>();

            }

            return View(lista);
        }

        [HttpPost]
        public async Task<ActionResult> DETALLE_PEDIDO_ELIMINADO(int id)
        {
            if (ModelState.IsValid)
            {
                string url = $"https://localhost:44380/api/detalle/{id}";
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
            return RedirectToAction("DETALLE_PEDIDO_LISTADO");
        }

        [HttpGet]
        public async Task<ActionResult> MostrarDetallesDetallePedido(int id)
        {
            DetallePedido detallePedido = null;
            string url = $"https://localhost:44380/api/detalle/{id}";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                detallePedido = JsonConvert.DeserializeObject<DetallePedido>(contenido);
                Debug.WriteLine(contenido);
            }
            else
            {
                Debug.WriteLine("Error al obtener detalles del producto.");
                detallePedido = new DetallePedido();
            }

            // Asumiendo que solo esperas un producto, toma el primero de la lista
            return PartialView("_DetallesDetallePedidoPartial", detallePedido);
        }

    }
}
