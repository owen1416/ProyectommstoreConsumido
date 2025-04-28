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
        //obtener el contador de productos y numero de stock

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
                int totalstock = lista != null ? lista.Sum(p => p.Stock) : 0;

                ViewBag.totalStock = totalstock;
                ViewBag.totalproductos = totalproductos;
            }
            else
            {
                Debug.WriteLine("Error al obtener lista de productos.......");
                ViewBag.totalproductos = 0;

            }

            //obtener el contador de usuarios

            List<Usuarios> listaus = null;
            string urlus = "https://localhost:44380/api/usuario/getall";
            HttpClient clientus = new HttpClient();
            var respuestaus = client.GetAsync(urlus).Result;
            if (respuestaus.IsSuccessStatusCode)
            {
                var contenido = respuestaus.Content.ReadAsStringAsync().Result;
                listaus = JsonConvert.DeserializeObject<List<Usuarios>>(contenido);
                Debug.WriteLine(contenido);

                int totalUsuarios = listaus != null ? listaus.Count : 0;
                ViewBag.totalUsuarios = totalUsuarios;
            }
            else
            {
                Debug.WriteLine("Error al obtener lista de usuarios.......");
                listaus = new List<Usuarios>();

            }
            
            //obtener el contador de clientes

            List<Clientes> listacli = null;
            string urlcli = "https://localhost:44380/api/cliente/getall";
            HttpClient clientcli = new HttpClient();
            var respuestacli = clientcli.GetAsync(urlcli).Result;
            if (respuestacli.IsSuccessStatusCode)
            {
                var contenido = respuestacli.Content.ReadAsStringAsync().Result;
                listacli = JsonConvert.DeserializeObject<List<Clientes>>(contenido);
                Debug.WriteLine(contenido);

                int totalClientes = listacli != null ? listacli.Count : 0;
                ViewBag.totalClientes = totalClientes;
            }
            else
            {
                Debug.WriteLine("Error al obtener lista de clientes....... ");
                listacli = new List<Clientes>();

            }

            return View("Index");

        }
         

    }
}