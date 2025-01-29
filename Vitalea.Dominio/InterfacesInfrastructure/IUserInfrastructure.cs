using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vitalea.Dominio.Models;

namespace Vitalea.Dominio.InterfacesInfrastructure
{
    public interface IUserInfrastructure
    {
        Task<int?> GetUserIdByUserNameUserNameAsync(string nombreUsuario);
        Task<DateTime?> GetLastLoginDateByUserNameAsync(string nombreUsuario);
        Task<string> GetUserEncryptedByUserNameAsync(string nombreUsuario);
        Task<DateTime?> ObtenerFechaVencimientoUrlAsync(string usuarioEncriptado);
        Task <List<Users>> GetUsers();
        void UpdateEncrypt(string encryptedUser);
    }
}
