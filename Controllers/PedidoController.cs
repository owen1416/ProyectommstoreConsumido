using Newtonsoft.Json;
using ProyectommstoreConsumido.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
    }
}