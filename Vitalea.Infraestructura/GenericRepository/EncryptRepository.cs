using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Vitalea.Dominio.InterfacesInfrastructure;
using Vitalea.Infraestructura.Connection;

namespace Vitalea.Infraestructura.GenericRepository
{
    public class EncryptRepository: IEncryptInfrastructure
    {
        public string Encryption(string str)
        {

            try
            {
                TripleDESCryptoServiceProvider des;
                MD5CryptoServiceProvider hashmd5;
                byte[] keyhash, buff;
                string stEncripted;
                hashmd5 = new MD5CryptoServiceProvider();
                keyhash = hashmd5.ComputeHash(Encoding.ASCII.GetBytes(InterfaceConfig.keySecurityEncryptionPassUsers));
                hashmd5 = null;
                des = new TripleDESCryptoServiceProvider();

                des.Key = keyhash;
                des.Mode = CipherMode.ECB;

                buff = Encoding.ASCII.GetBytes(str);
                stEncripted = Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buff, 0, buff.Length));

                return stEncripted;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Decryption(string str, string stkey)
        {
            try
            {
                TripleDESCryptoServiceProvider des;
                MD5CryptoServiceProvider hashmd5;
                byte[] keyhash, buff;
                string stDecripted;
                hashmd5 = new MD5CryptoServiceProvider();
                keyhash = hashmd5.ComputeHash(Encoding.ASCII.GetBytes(stkey));
                hashmd5 = null;
                des = new TripleDESCryptoServiceProvider();

                des.Key = keyhash;
                des.Mode = CipherMode.ECB;

                buff = Convert.FromBase64String(str);
                stDecripted = Encoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buff, 0, buff.Length));

                return stDecripted;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
