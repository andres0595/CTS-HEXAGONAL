using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vitalea.Dominio.Models
{
    public class Login
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int Activo { get; set; }    
    }

    public class ReplyTokens
    {
        public string? token { get; set; }
        public string? Refreshtoken { get; set; }
    }
}
