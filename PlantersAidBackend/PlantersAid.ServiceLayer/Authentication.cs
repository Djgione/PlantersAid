using Microsoft.IdentityModel.Tokens;
using PlantersAid.DataAccessLayer.Interfaces;
using PlantersAid.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using PlantersAid.Models;

namespace PlantersAid.ServiceLayer
{
    public class Authentication : IAuthService
    {
        public IAccountDAO AccountDataAccess { get; }
        public IUserManagementDAO UserDataAccess { get; }

        public Authentication(IAccountDAO dataAccessAcc, IUserManagementDAO dataAccessUser)
        {
            AccountDataAccess = dataAccessAcc;
            UserDataAccess = dataAccessUser;
        }

        /// <summary>
        /// Method that creates the JwtSecurityToken utilizing the ID from the database
        /// Better than using other information from database as the ID will never change, so users can be logged in on multiple items while changing account info
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private string buildToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("loginSymmetricSecurityKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Authenticates a Login Request coming in the form of AuthnRequest, and creates a AuthnResponse with the correct information
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AuthnResponse Authenticate(AuthnRequest request)
        {
            //Builds an account from the incoming request
            var account = new Account(request.Email, request.Password, RetrieveId(request.Email));




            // T E M P O R A R Y
            Profile profile;
            throw new NotImplementedException();



            var token = buildToken(account);

            return new AuthnResponse(account, profile, token);
        }

        public int RetrieveId(string email)
        {
            return AccountDataAccess.RetrieveId(email);
        }
    }
}
