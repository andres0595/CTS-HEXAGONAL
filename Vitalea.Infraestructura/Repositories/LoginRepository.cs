using Microsoft.Data.SqlClient;
using Vitalea.Dominio.Models;
using Vitalea.Infraestructura.Configurations;
using Dapper;
using Vitalea.Dominio.InterfacesInfrastructure;
using System.IdentityModel.Tokens.Jwt;
using Vitalea.Infraestructura.Security;

namespace Vitalea.Infraestructura.Repositories
{
    public class LoginRepository : ILoginInfrastructure
    {
        public ContextDB conn;
        QuerySqlViewModel _info = new QuerySqlViewModel();
        private SqlController _sqlController;
        public LoginRepository(SqlController sqlController)
        {
            conn = new ContextDB();
            _sqlController = sqlController;
        }

        public async Task<Users> login(Users _login)
        {
            Users loginComplete = new Users();
            using (SqlConnection connection = conn.ConnectBD())
            {
                try
                { 
                    string query = _sqlController.SelectToUserData();
                    var parameters = new
                    {
                        NombreUsuario = _login.usuario,
                        Contrasena = _login.contrasena,
                        idsede = _login.idsede,
                    };
                    loginComplete = connection.QuerySingleOrDefault<Users>(query, parameters);

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }

            }
            return loginComplete;
        }
      
        public async Task<Reply> PasswordChange(Users _user)
        {
            using (SqlConnection connection = conn.ConnectBD())
            {
                try
                {
                    _info.tabla = "Usuarios";
                    _info.valores = new string[2] { "Contrasena", "UltimaFechaIngreso" };
                    _info.condiciones = new string[1] { "Usuario_Encriptado" };
                    string query = _sqlController.UpdateToTable(_info);
                    var parameters = new
                    {
                        Contrasena = _user.contrasena,
                        UltimaFechaIngreso = DateTime.Now,
                        Usuario_Encriptado= _user.Usuario_Encriptado
                    };
                    await connection.ExecuteAsync(query, parameters);
                    return new Reply { Ok = true, Message = "Tú cambio de contraseña ha sido exitoso" };
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
