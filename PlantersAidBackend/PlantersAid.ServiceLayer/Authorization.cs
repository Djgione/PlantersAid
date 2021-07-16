using Microsoft.IdentityModel.Tokens;
using PlantersAid.Models;
using PlantersAid.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlantersAid.ServiceLayer
{
    public class Authorization : IAuthorization
    {
        private readonly string IssuerAudience = Environment.GetEnvironmentVariable("issuerAndAudiencePlantersAid");

        public Authorization()
        { }

        public void Authorize(in AuthzRequest request)
        {

        }

        /// <summary>
        /// Retrieves principal from expired token and returns it
        /// Can throw SecurityTokenException
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private ClaimsPrincipal RetrievePrincipalFromExpiredToken(string token)
        {
            ///Creates a TokenValidationParameters that checks for valid audience, issuer, issuer signing key, but for an invalid lifetime
            ///Wants an expired token with all other valid parameters
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = IssuerAudience,
                ValidAudience = IssuerAudience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("loginSymmetricSecurityKey"))),
                ValidateLifetime = false
            };

            ///Creates a token handler that will validate the token for these parameters and output a security token with the same information
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid AccessToken");

            return principal;
        }




    }
}
