using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Vitalea.Dominio.InterfaceService;
using Vitalea.Dominio.InterfacesInfrastructure;
using Vitalea.Dominio.Models;

namespace Vitalea.Dominio.Service
{
    public class ServiceUser : IUserService
    {
        Reply _Reply = new Reply();
        private readonly IUserInfrastructure _IUserInfrastructure;
        private readonly  IEmailService _IEmailService;
        public ServiceUser(IUserInfrastructure IUserInfrastructure, IEmailService emailService)
        {
            _IUserInfrastructure = IUserInfrastructure;
            _IEmailService = emailService;
        }


        public async Task<string> ProcessUserAsync(string nombreUsuario)
        {
            var idUsuario = await _IUserInfrastructure.GetUserIdByUserNameUserNameAsync(nombreUsuario);
            var usuarioEncriptado = await _IUserInfrastructure.GetUserEncryptedByUserNameAsync(nombreUsuario);

            if (idUsuario == null)
            {
                return $"3,{usuarioEncriptado ?? "Sin usuario"}";
            }
            var ultimaFechaIngreso = await _IUserInfrastructure.GetLastLoginDateByUserNameAsync(nombreUsuario);

            if (ultimaFechaIngreso != null)
            {
                return $"1,{usuarioEncriptado}";
            }

            return $"0,{usuarioEncriptado}";
        }

        public async Task<Reply> ProcessUserChangePasswordAsync(string nombreUsuario)
        {
            var usuarioEncriptado = await _IUserInfrastructure.GetUserEncryptedByUserNameAsync(nombreUsuario);
            if (usuarioEncriptado == null)
            {
                _Reply.Ok = false;
                _Reply.Message = "Tú usuario no registra en el sistema. Por favor verifica los datos";
                _Reply.Data = null;
                _Reply.Status = StatusCodes.Status400BadRequest;
            }
            else
            {
                _Reply.Ok = true;
                _Reply.Message = "Usuario encontrado";
                _Reply.Data = usuarioEncriptado;
                _Reply.Status = StatusCodes.Status200OK;
            }
            return _Reply;
        }

        public async Task<bool> ValidarFechaUrlAsync(string token)
        {
            try
            {
                var fechaVencimiento = await _IUserInfrastructure.ObtenerFechaVencimientoUrlAsync(token);

                if (fechaVencimiento.HasValue)
                {
                    var minutes = (DateTime.Now - fechaVencimiento.Value).TotalMinutes;
                    return minutes <= 5;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error validando la fecha de la URL", ex);
            }
        }

        public async Task<string> EnvioRecuperarContrasena(string email)
        {
            // Consultar el usuario por correo
            List<Users> user = new List<Users>();
            user =await _IUserInfrastructure.GetUsers();
            // Buscar el usuario cuyo correo coincida con el email proporcionado

            if (user == null)
            {
                return "0"; // Usuario no encontrado
            }

            var usuarioEncriptado = user.FirstOrDefault(u => u.correo == email)?.Usuario_Encriptado;
            // Generar URL de recuperación
            string UrlRecuperacion = $"https://cts.vitalea.com.co/cambiar-contrasena/{usuarioEncriptado}";

            // Cuerpo del correo
            string body = GenerarCuerpoCorreo(UrlRecuperacion);

            // Actualizar encriptado en la base de datos
            _IUserInfrastructure.UpdateEncrypt(usuarioEncriptado);

            // Enviar el correo
            _IEmailService.Send(email, "Recuperar Contraseña", body);

            return $"Hemos enviado un link a tu correo, {usuarioEncriptado}";
        }

        private string GenerarCuerpoCorreo(string urlRecuperacion)
        {
            return
                "<body>" +
                "<div style='border: 1px solid #103B60; border-radius: 5px; max-width:100%; padding:5px;'>" +
                "<div style='background-color:#E52180;height: 4rem;'>" +
                "<img  src ='cid:imagen' style='width: 6rem; margin-left: 25rem;padding-top: 1rem'/>" +
                "</div>" +
                "<div style='text-align:center;'>" +
                "<h3 style = 'color:#2E4789;'>¿Has olvidado o quieres cambiar tú clave?</h3>" +
                "<p>Con el siguiente enlace ingresaras al sistema y podras restaurar tú contraseña</p>" +
                $"<a href=\"{urlRecuperacion}\">Restaurar Contraseña</a>" +
                "<p>Cordialmente,</p>" +
                "<strong style = 'color:#2E4789'>IT Health</strong>" +
                "</div>" +
                "<div style='background-color:#E52180;height: 1rem;'></div>" +
                "</div>" +
                "</body>";
        }
    }
}
