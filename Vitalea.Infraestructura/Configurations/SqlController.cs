using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vitalea.Dominio.Models;

namespace Vitalea.Infraestructura.Configurations
{
    public class SqlController
    {
        private string[] comparators = new string[2] { "=", ">" };
        string statement = string.Empty;
        public string SelectToTable(QuerySqlViewModel info)
        {
            return BuildSelectToTable(info);
        }

        private string BuildSelectToTable(QuerySqlViewModel info)
        {
            statement = "SELECT ";

            for (int i = 0; i < info.valores.Length; i++)
            {
                if (i == (info.valores.Length - 1))
                {
                    statement += info.valores[i] + " ";
                }
                else
                {
                    statement += info.valores[i] + ", ";
                }
            }

            string complemento = info.condiciones != null ? " WHERE " : " ";
            statement += "FROM " + info.tabla + complemento;

            if (info.condiciones != null)
            {
                for (int i = 0; i < info.condiciones.Length; i++)
                {
                    if (i == (info.condiciones.Length - 1))
                    {
                        statement += info.condiciones[i] + "=@" + info.condiciones[i] + ";";
                    }
                    else
                    {
                        statement += info.condiciones[i] + "=@" + info.condiciones[i] + " AND ";
                    }

                }

            }
            return statement;
        }

        public string UpdateToTable(QuerySqlViewModel info)
        {
            return BuildUpdateToTable(info);
        }

        private string BuildUpdateToTable(QuerySqlViewModel info)
        {
            statement = "UPDATE " + info.tabla + " SET ";

            for (int i = 0; i < info.valores.Length; i++)
            {
                if (i == (info.valores.Length - 1))
                {
                    statement += info.valores[i] + "=@" + info.valores[i] + " ";
                }
                else
                {
                    statement += info.valores[i] + "=@" + info.valores[i] + ", ";
                }
            }

            statement += "WHERE ";

            for (int i = 0; i < info.condiciones.Length; i++)
            {
                if (i == (info.condiciones.Length - 1))
                {
                    statement += info.condiciones[i] + "=@" + info.condiciones[i] + ";";
                }
                else
                {
                    statement += info.condiciones[i] + "=@" + info.condiciones[i] + " AND ";
                }
            }
            return statement;
        }

        public string SelectToTableInnerJoin(QuerySqlViewModel info)
        {
            return BuildSelectToTableInnerJoin(info);
        }
        private string BuildSelectToTableInnerJoin(QuerySqlViewModel info)
        {
            statement = "SELECT ";

            for (int i = 0; i < info.valores.Length; i++)
            {
                if (i == (info.valores.Length - 1))
                {
                    statement += info.valores[i] + " ";
                }
                else
                {
                    statement += info.valores[i] + ", ";
                }
            }

            statement += "FROM ";


            for (int i = 0; i < info.tablas.Length; i++)
            {
                if (i == (info.tablas.Length - 1))
                {
                    if (info.igualador == 0)
                    {
                        var constante = info.eval == 0 ? info.join[i + 1] : info.join[i];
                        statement += info.tablas[i] + " " + info.idenTabla[i] + " ON " + " " + info.idenTabla[i] + "." + info.join[i] + "=" + info.idenTabla[i - 1] + "." + constante;
                    }
                    else
                    {
                        statement += info.tablas[i] + " " + info.idenTabla[i] + " ON " + " " + info.idenTabla[i] + "." + info.join[i] + "=" + info.idenTabla[i] + "." + info.join[i];
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        statement += info.tablas[i] + " " + info.idenTabla[i] + " INNER JOIN ";
                    }
                    else
                    {
                        statement += info.tablas[i] + " " + info.idenTabla[i] + " ON " + " " + info.idenTabla[i] + "." + info.join[i] + "=" + info.idenTabla[i - 1] + "." + info.join[i - 1] + " INNER JOIN ";
                    }
                }
            }


            if (info.condiciones != null)
            {
                statement += " WHERE ";
                for (int i = 0; i < info.condiciones.Length; i++)
                {
                    if (i == (info.condiciones.Length - 1))
                    {
                        if (info.typeCondition != null) { statement += info.idenTabla[info.eval] + "." + info.condiciones[i] + comparators[info.typeCondition[i]] + "@" + info.condiciones[i] + ";"; }

                        else { statement += info.idenTabla[0] + "." + info.condiciones[i] + "=@" + info.condiciones[i] + ";"; }
                    }
                    else
                    {
                        if (info.typeCondition != null) { statement += info.idenTabla[info.eval] + "." + info.condiciones[i] + comparators[info.typeCondition[i]] + "@" + info.condiciones[i] + " AND "; }

                        else { statement += info.idenTabla[0] + "." + info.condiciones[i] + "=@" + info.condiciones[i] + " AND "; }
                    }

                }
            }

            return statement;
        }


        public string SelectToUserData()
        {
            return BuildSelectToUserData();
        }

        private string BuildSelectToUserData()
        {
            statement = @"SELECT U.IdUsuario AS id, U.Activo,NombreUsuario AS usuario,P.Nombre AS Rol,U.IdPerfil 
                        FROM Usuarios U INNER JOIN Perfiles P ON U.IdPerfil=P.IdPerfil
						INNER JOIN Sedes_x_Usuario SU ON SU.IdUsuario=U.IdUsuario
                        WHERE NombreUsuario=@NombreUsuario AND Contrasena=@Contrasena AND SU.idsede=@idsede";

            return statement;
        }

        public string SelectToUsers()
        {
            return BuildSelectToUsers();
        }

        private string BuildSelectToUsers()
        {
            statement = @"SELECT
	                U.IdUsuario,
	                U.NombreCompleto,U.Documento,U.Correo,U.NombreUsuario,convert(nvarchar,U.FechaNacimiento,101)as FechaNacimiento,
	                CASE WHEN U.Activo!=1 THEN 2 ELSE U.Activo END AS Estado,
	                P.Nombre AS Perfil,U.IdPerfil,
	                CASE WHEN U.Activo=1 THEN 'true' ELSE 'false' END AS EstadoNom, U.UsuarioAthenea,Usuario_Encriptado,U.Firma,U.Idtipodocumento
	                FROM USUARIOS U INNER JOIN PERFILES P ON U.IdPerfil=P.IdPerfil
	                where u.CodigoProyecto=1 OR u.CodigoProyecto IS NULL";
            return statement;   
        }
    }
}
