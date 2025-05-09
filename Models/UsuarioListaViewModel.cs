using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectommstoreConsumido.Models
{
    public class UsuarioListaViewModel
    {
        public List<Usuarios> Usuarios { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
    }
}