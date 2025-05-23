﻿using Newtonsoft.Json;
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
    public class PedidoController : Controller
    {


        [HttpGet]
        public ActionResult PEDIDO_LISTADO()
        {
            List<Pedidos> lista = null;
            string url = "https://localhost:44380/api/pedido/getall";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = respuesta.Content.ReadAsStringAsync().Result;
                lista = JsonConvert.DeserializeObject<List<Pedidos>>(contenido);
                Debug.WriteLine(contenido);

            }
            else
            {
                Debug.WriteLine("Error.......");
                lista = new List<Pedidos>();

            }

            return View(lista);
        }


        [HttpPost]
        public async Task<ActionResult> PEDIDO_ELIMINADO(int id)
        {
            if (ModelState.IsValid)
            {
                string url = $"https://localhost:44380/api/pedido/{id}";
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
            return RedirectToAction("PEDIDO_LISTADO");
        }



        [HttpGet]
        public async Task<ActionResult> MostrarDetallesPedido(int id)
        {
            Pedidos pedidos = null;
            string url = $"https://localhost:44380/api/pedido/{id}";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;
            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();
                pedidos = JsonConvert.DeserializeObject<Pedidos>(contenido);
                Debug.WriteLine(contenido);
            }
            else
            {
                Debug.WriteLine("Error al obtener detalles del producto.");
                pedidos = new Pedidos();
            }

            // Asumiendo que solo esperas un producto, toma el primero de la lista
            return PartialView("_DetallesPedidoPartial", pedidos);
        }
    }
}