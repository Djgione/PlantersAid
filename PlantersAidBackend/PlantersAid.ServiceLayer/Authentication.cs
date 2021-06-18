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

        public static readonly string ClaimsRole = "UserRole";
        private readonly string IssuerAudience = "plantersaid.com";
        

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
        private string BuildToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //Client Secret is the key
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("loginSymmetricSecurityKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                }),
                Issuer = IssuerAudience,
                Audience = IssuerAudience,
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

            Logger.Log("Account Built: " + account.ToString());

            //If User is not found / Unsuccessful Login
            var loginResult = AccountDataAccess.Login(account);

            if(!loginResult.Success)
            {
                Logger.Log("Unsuccessful Login");
                return null;
            }
            //If the Login Method from the AccountSqlDAO Works, use the same account that you used to start that method and combine it with a retrieved profile in order to create the AuthnResponse
            var profile = UserDataAccess.RetrieveProfile(account.Id);
            var token = BuildToken(account);

            Logger.Log("Profile Retrieved: " + profile.ToString());

            return new AuthnResponse(account, profile, token);
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
                throw new SecurityTokenException("Invalid Token");

            return principal;
        }

        private string BuildRefreshToken()
        {
            var randomNumber = new byte[32];

        }

        public int RetrieveId(string email)
        {
            return AccountDataAccess.RetrieveId(email);
        }
    }
}
