using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vitalea.Dominio.Models;

namespace Vitalea.Dominio.InterfaceService
{
    public interface ILoginService
    {
        public Task<Reply> Authentication(Users _login);
        public Task<Reply> PasswordChange(Users _user);
        public ReplyTokens GetRefreshToken(ReplyTokens _replyTokens);
    }
}
