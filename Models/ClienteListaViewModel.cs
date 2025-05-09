using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectommstoreConsumido.Models
{
    public class ClienteListaViewModel
    {
        public List<Clientes> Clientes { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
    }
}