using Microsoft.AspNetCore.Http;
using Vitalea.Dominio.InterfaceService;
using Vitalea.Dominio.InterfacesInfrastructure;
using Vitalea.Dominio.Models;

namespace Vitalea.Dominio.Service
{
    public class ServiceLogin : ILoginService
    {
        private readonly ILoginInfrastructure _ILoginInfrastructure;
        private readonly IUserService _IUserService;
        private readonly ITokenInfrastructure _IToken;
        private readonly IEncryptInfrastructure _IEncrypt;
        Reply _Reply = new Reply();
        public ServiceLogin(ILoginInfrastructure ILoginInfrastructure, IUserService userService, ITokenInfrastructure ITokenInfrastructure, IEncryptInfrastructure iEncrypt)
        {
            _ILoginInfrastructure = ILoginInfrastructure;
            _IUserService = userService;
            _IToken = ITokenInfrastructure;
            _IEncrypt = iEncrypt;

        }

        public async Task<Reply> Authentication(Users _login)
        {
            string validador = await _IUserService.ProcessUserAsync(_login.usuario);
            int codigoRespuesta = Convert.ToInt32(validador.Split(',')[0]);

            _login.contrasena = codigoRespuesta != 1 ? _IEncrypt.Encryption(_login.documento) : _IEncrypt.Encryption(_login.contrasena);
            Users loginComplete = new Users();
            loginComplete = await _ILoginInfrastructure.login(_login);
            // Validación: Si el usuario no existe o la contraseña es incorrecta
            if (loginComplete == null)
            {
                _Reply.Ok = false;
                _Reply.Message = "Invalid username or password";
                _Reply.Data = null;
                return _Reply;
            }
            if (loginComplete.activo != 1)
            {
                _Reply.Ok = false;
                _Reply.Message = "The user is inactive. Please contact your system administrator.";
                _Reply.Data = null;
                return _Reply;
            }
            else
            {
                Dictionary<string, string> DataUser = new Dictionary<string, string>
            {
                {"token", _IToken.GenerateTokenJwt(loginComplete.usuario) },
                    {"Rol",loginComplete.Rol},
                    {"IdUsuario",Convert.ToString(loginComplete.id)},
                    {"IdPerfil",Convert.ToString(loginComplete.IdPerfil)},
                    {"Usuario",loginComplete.usuario}
            };

                _Reply.Ok = true;
                _Reply.Data = DataUser;
                _Reply.Message = "Token generate";
                _Reply.Status = 200;
            }
            return _Reply;
        }

        public async Task<Reply> PasswordChange(Users _login)
        {

            _Reply = await _IUserService.ProcessUserChangePasswordAsync(_login.usuario);
            if (_Reply.Ok == false)
            {
                return _Reply;
            }
            else
            {
                _login.Usuario_Encriptado = _Reply.Data.ToString();
                var fechaVencimiento = await _IUserService.ValidarFechaUrlAsync(_login.Usuario_Encriptado);
                if (fechaVencimiento)
                {
                    _login.contrasena = _IEncrypt.Encryption(_login.contrasena);
                    _Reply = await _ILoginInfrastructure.PasswordChange(_login);
                }
                else
                {
                    _Reply.Ok = false;
                    _Reply.Status = StatusCodes.Status400BadRequest;
                    _Reply.Message = "Lo sentimos la url que intentas usar a caducado";
                }
            }
            return _Reply;
        }

        public ReplyTokens GetRefreshToken(ReplyTokens replyTokens)
        {
            return _IToken.GetRefreshToken(replyTokens);
        }

    }

}
