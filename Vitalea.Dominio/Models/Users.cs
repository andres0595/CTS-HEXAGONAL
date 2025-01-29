using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vitalea.Dominio.Models
{
    public class Users
    {
       
        public string? usuario { get; set; }       
        public string? contrasena { get; set; }       
        public int idsede { get; set; }
        public int id { get; set; }
        public int perfil { get; set; }
        public string? nombreCOmpleto { get; set; }
        public string? documento { get; set; }
        public string? correo { get; set; }
        public string? fechaNacimiento { get; set; }
        public int activo { get; set; }
        public string? NombrePerfil { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime UltimaFechaingreso { get; set; }
        public int[]? SedesArray { get; set; }
        /*Parametros para el cambio de contrasena*/
        public string? contrasenaAn { get; set; }
        public Boolean EstadoBool { get; set; }
        /*parametro para asignarle modulos*/
        public int[]? ModuloArray { get; set; }
        public int[]? Permisos { get; set; }
        public string? UsuarioAthenea { get; set; }
        public int idUsuario_creacion { get; set; }
        public string? Usuario_Encriptado { get; set; }
        public string? Firma { get; set; }
        public int Idtipodocumento { get; set; }
        public string? fechaFirma { get; set; }
        public string? Rol { get; set; }
        public int IdPerfil { get; set; }
    }
}
