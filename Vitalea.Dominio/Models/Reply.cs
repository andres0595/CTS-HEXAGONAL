using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vitalea.Dominio.Models
{
    public class Reply
    {
        public bool Ok { get; set; }
        public string? Message { get; set; }
        public int Status { get; set; }
        public object? Data { get; set; }
    }
}
