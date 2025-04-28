using Newtonsoft.Json;
using ProyectommstoreConsumido.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;

namespace ProyectommstoreConsumido.Controllers
{
    public class CompraController : Controller
    {
        HttpClient _httpClient = new HttpClient();
       


        [HttpGet]
        public ActionResult CrearPedido()
        {
            var model = new PedidoViewModel
            {
                ItemsCompra = new List<ItemCompraViewModel>() // Inicializamos la lista vacía
            };
            return View(model); // Muestra el formulario
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearPedido(PedidoViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string apiUrl = $"https://localhost:44380/api/compra/generar?clienteId={model.ClienteId}";

                    // Solo serializamos ItemsCompra, no el modelo completo
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(model.ItemsCompra), Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync(apiUrl, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();

                        // Aquí, debes deserializar la respuesta correcta (igual como viene del API)
                        var respuestaApi = JsonConvert.DeserializeObject<PedidoViewModel>(jsonString);

                        if (respuestaApi != null && respuestaApi.Exito)
                        {
                            ViewBag.MensajeExito = respuestaApi.Mensaje;
                            ViewBag.PedidoId = respuestaApi.Datos.PedidoID;
                           
                        }
                        else
                        {
                            ViewBag.ErrorMessage = respuestaApi?.Mensaje ?? "Error al crear el pedido.";
                            return View("CrearPedido", model);
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = $"Error en la API: {response.StatusCode}";
                        return View("CrearPedido", model);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Excepción: {ex.Message}";
                    return View("CrearPedido", model);
                }
            }

            return RedirectToAction("CrearPedido");
        }
    }
}