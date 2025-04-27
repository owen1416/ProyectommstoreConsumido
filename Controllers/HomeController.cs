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
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            List<Productos> lista = null;
            String url = "https://localhost:44380/api/pro/getall";
            HttpClient client = new HttpClient();
            var respuesta = client.GetAsync(url).Result;

            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = respuesta.Content.ReadAsStringAsync().Result;
                lista = JsonConvert.DeserializeObject<List<Productos>>(contenido);
                Debug.WriteLine(contenido);


                int totalproductos = lista != null ? lista.Count : 0;
                int totalstock = lista != null ? lista.Sum(p => p.Stock ) : 0;

                ViewBag.totalStock = totalstock;
                ViewBag.totalproductos = totalproductos;
            }
            else
            {
                Debug.WriteLine("Error al obtener lista de productos.......");
                ViewBag.totalproductos = 0;
            
            }

            return View(lista);
        }

       
    }
}