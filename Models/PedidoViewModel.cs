
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectommstoreConsumido.Models
{
    public class PedidoViewModel
    {

        // Para la creación del pedido
        public int ClienteId { get; set; }
        public List<ItemCompraViewModel> ItemsCompra { get; set; }

        // Para la respuesta del pedido
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public DatosPedido Datos { get; set; }
    }
}