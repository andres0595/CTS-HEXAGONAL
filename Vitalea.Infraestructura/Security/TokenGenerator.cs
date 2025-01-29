﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Vitalea.Dominio.InterfacesInfrastructure;
using Vitalea.Dominio.Models;
using Vitalea.Infraestructura.Connection;

namespace Vitalea.Infraestructura.Security
{
    public class TokenGenerator : ITokenInfrastructure
    {
        public string GenerateTokenJwt(string username)
        {
            var interfaceConfig = new InterfaceConfig();
            interfaceConfig.InitializeConfig();
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(interfaceConfig.secretKeyJWT));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create a claimsIdentity
            //ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) });
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim("user", username) });

            // create token to the user
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: interfaceConfig.audienceJWT,
                issuer: interfaceConfig.issuerJWT,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(interfaceConfig.expirationJWT)),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }


        public string GenerateRefreshTokenJwt()
        {
            var byteArray = new byte[64];
            var refresToken = string.Empty;
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(byteArray);
                refresToken = Convert.ToBase64String(byteArray);
            }
            return refresToken;
        }

        public ReplyTokens GetRefreshToken(ReplyTokens _tokenValidador)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenExpirado = tokenhandler.ReadJwtToken(_tokenValidador.token);
            //string Usuario = tokenExpirado.Claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();
            string Usuario = tokenExpirado.Claims.Where(x => x.Type == "user").FirstOrDefault().ToString();
            ReplyTokens replyTokens = new ReplyTokens();
            Usuario = Usuario.Replace("user: ", "");
            replyTokens.token = GenerateTokenJwt(Usuario);
            replyTokens.Refreshtoken = GenerateRefreshTokenJwt();

            return replyTokens;
        }
    }
}
