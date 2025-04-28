using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectommstoreConsumido.Models
{
    public class Pedidos
    {
        public int PedidoID { get; set; }
        public int ClienteID { get; set; }
        public DateTime FechaPedido { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }

        public Pedidos()
        {

        }

        public Pedidos(int pedidoID, int clienteID, DateTime fechaPedido, string estado, decimal total)
        {
            PedidoID = pedidoID;
            ClienteID = clienteID;
            FechaPedido = fechaPedido;
            Estado = estado;
            Total = total;
        }
    }
}