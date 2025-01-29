using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vitalea.Dominio.Models;

namespace Vitalea.Dominio.InterfacesInfrastructure
{
    public interface ILoginInfrastructure
    {
        public Task<Users> login(Users _Login);
        public Task<Reply> PasswordChange(Users _user);      
    }
}
