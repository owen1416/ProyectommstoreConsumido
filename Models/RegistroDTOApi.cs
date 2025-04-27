using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectommstoreConsumido.Models
{
    public class RegistroDTOApi
    {
      
        public string NombreUsuario { get; set; }

        public string Contraseña { get; set; }

        public string ConfirmarContraseña { get; set; }

        public string Email { get; set; }
    }
}