using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Vitalea.Dominio.InterfaceService;

namespace Vitalea.Dominio.Service
{
    public class ServiceEmail: IEmailService
    {
        public void Send(string toEmail, string subject, string body)
        {
            // Configuración de SMTP
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("notificaciones@vitalea.com.co", "Recuperar Contraseña");
            mail.To.Add(new MailAddress(toEmail));
            mail.Subject = subject;
            mail.BodyEncoding = Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            mail.IsBodyHtml = true;
            mail.Body = body;

            // Configuración de imagen (logo)
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
            string imagePathF = AppDomain.CurrentDomain.BaseDirectory + @"Content/Logo/vitalea.png";
            LinkedResource logo = new LinkedResource(imagePathF);
            logo.ContentId = "imagen";
            htmlView.LinkedResources.Add(logo);
            mail.AlternateViews.Add(htmlView);

            // Envío del correo
            SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("notificaciones@vitalea.com.co", "Kof26557");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(mail);
        }
    }
}
