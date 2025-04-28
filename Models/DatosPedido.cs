using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectommstoreConsumido.Models
{
    public class DatosPedido
    {
        public int PedidoID { get; set; }
        public int ClienteID { get; set; }
        public DateTime FechaPedido { get; set; }
        public string Estado { get; set; }  
        public decimal Total { get; set; }

   
    }
}