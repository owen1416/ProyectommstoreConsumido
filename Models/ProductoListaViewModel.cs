using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectommstoreConsumido.Models
{
    public class ProductoListaViewModel
    {
        public List<Productos> Productos { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
    }
}