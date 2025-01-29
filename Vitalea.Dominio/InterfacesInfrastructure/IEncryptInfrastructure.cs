using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vitalea.Dominio.InterfacesInfrastructure
{
    public interface IEncryptInfrastructure
    {
        string Encryption(string str);
        string Decryption(string str, string stkey);
    }
}
