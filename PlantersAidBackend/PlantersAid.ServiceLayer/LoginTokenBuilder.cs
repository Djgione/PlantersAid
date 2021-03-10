using PlantersAid.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace PlantersAid.ServiceLayer
{
    public class LoginTokenBuilder : ITokenBuilder
    {
        public string BuildToken(string email)
        {
            
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("loginSymmetricSecurityKey")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512);
            DateTimeOffset issueTime = DateTimeOffset.UtcNow;
            DateTimeOffset expiryTime = issueTime.AddMinutes(30);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Iat, issueTime.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, expiryTime.ToUnixTimeSeconds().ToString()),
            };

            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


            return encodedJwt;
        }
    }
}
