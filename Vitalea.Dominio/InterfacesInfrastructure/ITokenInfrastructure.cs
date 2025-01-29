using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vitalea.Dominio.Models;

namespace Vitalea.Dominio.InterfacesInfrastructure
{
    public interface ITokenInfrastructure
    {
        public string GenerateTokenJwt(string usuario);
        public string GenerateRefreshTokenJwt();
        public ReplyTokens GetRefreshToken(ReplyTokens _replyTokens);
    }
}
