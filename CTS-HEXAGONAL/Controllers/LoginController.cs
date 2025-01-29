using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Vitalea.Dominio.InterfaceService;
using Vitalea.Dominio.Models;
using Vitalea.Dominio.Service;
using Vitalea.Infraestructura.Security;

namespace CTS_HEXAGONAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginService _Ilogin;
        private readonly IUserService _IUserService;
        Reply _Reply= new Reply();

        public LoginController(ILoginService _login, IUserService _userService)
        {
            _Ilogin = _login;
            _IUserService= _userService;
        }

        #region Metodo para realizar la autenticación
        [HttpPost]
        [Route("Login")]
        public async Task<Reply> Authentication([FromBody] Users _login)
        {
            try
            {
                return await _Ilogin.Authentication(_login);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Metodo para realizar la autenticación
        [HttpPut]
        [Route("CambiarContrasena")]
        public async Task<Reply> ChangePassword([FromBody] Users _login)
        {
            try
            {
                return await _Ilogin.PasswordChange(_login);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Metodo para obtener refresh Token JWT
        [HttpPost]
        [Route("ObtenerRefreshToken")]
        public ReplyTokens ObtenerRefreshToken(ReplyTokens _tokenValidador)
        {
          return _Ilogin.GetRefreshToken(_tokenValidador);
        }
        #endregion

        #region Metodo para enviar correo de recuperacion
        [HttpPost]
        [Route("CorreoRecuperacion")]
        public async Task<Reply> EnvioRecuperarContrasena(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _Reply.Ok = false;
                _Reply.Message = "El campo de correo electrónico es obligatorio.";
                _Reply.Status = StatusCodes.Status400BadRequest;
                return _Reply;
            }

            // Llamar al método EnvioRecuperarContrasena
            var resultado = await _IUserService.EnvioRecuperarContrasena(email);

            if (resultado == "0")
            {
                _Reply.Ok = false;
                _Reply.Message = "No se encontró un usuario con ese correo.";
                _Reply.Status = StatusCodes.Status404NotFound;
                return _Reply;
            }
            return new Reply { Ok = true, Message = "Se ha enviado un enlace de recuperación al correo proporcionado.", Status = StatusCodes.Status200OK };   
        }
        #endregion
    }
}
