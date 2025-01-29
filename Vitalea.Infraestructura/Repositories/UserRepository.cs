using Azure.Core;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Net;
using Vitalea.Dominio.InterfacesInfrastructure;
using Vitalea.Dominio.Models;
using Vitalea.Infraestructura.Configurations;

namespace Vitalea.Infraestructura.Repositories
{
    public class UserRepository : IUserInfrastructure
    {
        public ContextDB conn;
        QuerySqlViewModel _info = new QuerySqlViewModel();
        private SqlController _sqlController;
        public UserRepository(SqlController sqlController)
        {
            conn = new ContextDB();
            _sqlController = sqlController;
        }


        public async Task<int?> GetUserIdByUserNameUserNameAsync(string nombreUsuario)
        {
            using (SqlConnection connection = conn.ConnectBD())
            {
                try
                {
                    _info.tabla = "Usuarios";
                    _info.valores = new string[1] { "IdUsuario" };
                    _info.condiciones = new string[1] { "NombreUsuario" };
                    _info.eval = 0;
                    _info.igualador = 1;
                    string query = _sqlController.SelectToTable(_info);                 
                    return await connection.QueryFirstOrDefaultAsync<int>(query, new { NombreUsuario = nombreUsuario });

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<DateTime?> GetLastLoginDateByUserNameAsync(string nombreUsuario)
        {

            using (SqlConnection connection = conn.ConnectBD())
            {
                try
                {
                    //_login.Password = Encrypt.Encryption(_login.Password);
                    _info.tabla = "Usuarios";
                    _info.valores = new string[1] { "UltimaFechaIngreso" };
                    _info.condiciones = new string[1] { "NombreUsuario" };
                    _info.eval = 0;
                    _info.igualador = 1;
                    string query = _sqlController.SelectToTable(_info);             
                    return await connection.QueryFirstOrDefaultAsync<DateTime?>(query, new { NombreUsuario = nombreUsuario });

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<string> GetUserEncryptedByUserNameAsync(string nombreUsuario)
        {

            using (SqlConnection connection = conn.ConnectBD())
            {
                try
                {
                    //_login.Password = Encrypt.Encryption(_login.Password);
                    _info.tabla = "Usuarios";
                    _info.valores = new string[1] { "Usuario_Encriptado" };
                    _info.condiciones = new string[1] { "NombreUsuario" };
                    _info.eval = 0;
                    _info.igualador = 1;
                    string query = _sqlController.SelectToTable(_info);
                    return await connection.QueryFirstOrDefaultAsync<string>(query, new { NombreUsuario = nombreUsuario });

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<DateTime?> ObtenerFechaVencimientoUrlAsync(string usuarioEncriptado)
        {
            using (SqlConnection connection = conn.ConnectBD())
            {
                try
                {
                    _info.tabla = "Usuarios";
                    _info.valores = new string[1] { "Fecha_Vencimiento_Url" };
                    _info.condiciones = new string[1] { "Usuario_Encriptado" };
                    _info.eval = 0;
                    _info.igualador = 1;
                    string query = _sqlController.SelectToTable(_info);
                    return await connection.QueryFirstOrDefaultAsync<DateTime?>(query, new { UsuarioEncriptado = usuarioEncriptado });

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            
            }
        }
        
        public async Task<List<Users>> GetUsers()
        {
            using (SqlConnection connection = conn.ConnectBD())
            {
                try
                {
                    string query = _sqlController.SelectToUsers();
                    return await connection.QueryFirstOrDefaultAsync<List<Users>?>(query);

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
        }      

        public void UpdateEncrypt(string encryptedUser)
        {
            throw new NotImplementedException();
        }

    }
}
