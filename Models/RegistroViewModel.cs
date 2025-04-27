using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectommstoreConsumido.Models
{
    public class RegistroViewModel
    {
       
      
            [Required(ErrorMessage = "El nombre de usuario es requerido.")]
            [MaxLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 50 caracteres.")]
            public string NombreUsuario { get; set; }

            [Required(ErrorMessage = "La contraseña es requerida.")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
            public string Contraseña { get; set; }

            [Required(ErrorMessage = "La confirmación de la contraseña es requerida.")]
            [DataType(DataType.Password)]
            [Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden.")]
            public string ConfirmarContraseña { get; set; }

            [Required(ErrorMessage = "El correo electrónico es requerido.")]
            [EmailAddress(ErrorMessage = "Formato de correo electrónico no válido.")]
            [MaxLength(100, ErrorMessage = "El correo electrónico no puede tener más de 100 caracteres.")]
        public string Email { get; set; }
        }
    }
