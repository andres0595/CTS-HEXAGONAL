using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vitalea.Infraestructura.Connection
{
    public class InterfaceConfig
    {
        static internal string vitaleaConexionDB;
        static internal string violetaConexionDB;
        public string secretKeyJWT;
        public string expirationJWT;
        public string issuerJWT;
        public string audienceJWT;
        static internal string keySecurityEncryptionPassUsers;

        public void InitializeConfig()
        {
            var constructor = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuracion = constructor.Build();
            secretKeyJWT = configuracion.GetSection("Configuracion").GetSection("webAPI").GetSection("secretKeyJWT").Value;
            expirationJWT = configuracion.GetSection("Configuracion").GetSection("webAPI").GetSection("expirationJWT").Value;
            issuerJWT = configuracion.GetSection("Configuracion").GetSection("webAPI").GetSection("issuerJWT").Value;
            audienceJWT = configuracion.GetSection("Configuracion").GetSection("webAPI").GetSection("audienceJWT").Value;
            keySecurityEncryptionPassUsers = configuracion.GetSection("Configuracion").GetSection("webAPI").GetSection("keySecurityEncryptionPassUsers").Value;
            string catalog = string.Empty;
            int validadorCatalog = 2;   // 2 DESARROLLO, 3 TEST, 4 DEMO SIEMPRE CAMBIAR SEGUN AMBIENTE A PUBLICAR
            catalog = validadorCatalog == 2 ? "cts_soyvioleta_dev" :
                validadorCatalog == 3 ? "cts_soyvioleta_test" :
                validadorCatalog == 4 ? "cts_soyvioleta_demo" : "";
            string violetaConexionDBPruebas = $"Server=tcp:soyvioleta.database.windows.net,1433;Initial Catalog={catalog};Persist Security Info=False;User ID=soyvioleta;Password=eBnr3*qG7toT;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            bool activarAmbientePruebas = true; //SIEMPRE CAMBIAR A FALSE PARA CONECTAR AMBIENTE PRODUCCION             
            if (activarAmbientePruebas)
            {
                vitaleaConexionDB = configuracion.GetSection("ConexionesDB").GetSection("vitaleaConexionTestDB").Value;
                violetaConexionDB = violetaConexionDBPruebas;
            }
            else
            {
                vitaleaConexionDB = configuracion.GetSection("ConexionesDB").GetSection("vitaleaConexionDB").Value;

            }
        }
    }
}
