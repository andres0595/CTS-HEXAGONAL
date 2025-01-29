using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vitalea.Dominio.Models;

namespace Vitalea.Dominio.InterfaceService
{
    public interface IUserService
    {
        Task<string> ProcessUserAsync(string nombreUsuario);
        Task<Reply> ProcessUserChangePasswordAsync(string nombreUsuario);
        Task<bool> ValidarFechaUrlAsync(string token);
        Task<string> EnvioRecuperarContrasena(string email);
    }
}
