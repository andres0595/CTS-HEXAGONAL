using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vitalea.Infraestructura.Connection;

namespace Vitalea.Infraestructura.Configurations
{
    public class ContextDB
    {
        private static SqlConnection objConexion;
        private static string error;

        public SqlConnection ConnectBD()
        {
            return new SqlConnection(InterfaceConfig.vitaleaConexionDB);
        }

        public SqlConnection ConnectBDVioleta()
        {
            return new SqlConnection(InterfaceConfig.violetaConexionDB);
        }
        //  public IDbConnection Connection() => new SqlConnection(InterfaceConfig.vitaleaConexionDB);
    }
}
