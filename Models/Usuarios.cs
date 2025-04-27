using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectommstoreConsumido.Models
{
    public class Usuarios
    {
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string password { get; set; }
        public string Email { get; set; }

        // Propiedades adicionales para la respuesta de la API de login
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }        


        public Usuarios()
        {
            UsuarioID = 0;
            NombreUsuario = "";
            this.password = "";
            Email = "";
            IsAuthenticated = true;
            Token = "";
            ErrorMessage = "";    
        }

        public Usuarios(int usuarioID, string nombreUsuario, string password, string email, bool isAuthenticated, string token, string errorMessage)
        {
            UsuarioID = usuarioID;
            NombreUsuario = nombreUsuario;
            this.password = password;
            Email = email;
            IsAuthenticated = isAuthenticated;
            Token = token;
            ErrorMessage = errorMessage;
        }
    }
}