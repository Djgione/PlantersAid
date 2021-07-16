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
using System.Security.Cryptography;

namespace PlantersAid.ServiceLayer
{
    public class Authentication : IAuthentication
    {
        // Byte sizes for refresh token and device id
        private const int REFRESH_TOKEN_SIZE = 32;
        private const int DEVICE_ID_SIZE = 20;
        private const int REFRESH_TOKEN_DURATION = 90;
        private const int ACCESS_TOKEN_DURATION = 2;
        private readonly string IssuerAudience = Environment.GetEnvironmentVariable("issuerAndAudiencePlantersAid");
        public IAccountDAO AccountDataAccess { get; }
        public IUserManagementDAO UserDataAccess { get; }

        public static readonly string ClaimsRole = "UserRole";
        
        

        public Authentication(IAccountDAO dataAccessAcc, IUserManagementDAO dataAccessUser)
        {
            AccountDataAccess = dataAccessAcc;
            UserDataAccess = dataAccessUser;
        }

        /// <summary>
        /// Method that creates the JwtSecurityToken utilizing the ID from the database
        /// Better than using other information from database as the ID will never change, so users can be logged in on multiple items while changing account info
        /// 
        /// Sid = ID from DB
        /// Gender = Gender from profile
        /// Name = Full Name from Profile
        /// Email = Email from Account
        /// DateOfBirth = DateOfBirth from Profile
        /// NameIdentifier = Username from Profile
        /// DeviceId = DeviceId 
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private string BuildAccessToken(Account account, string deviceId, IEnumerable<int> permissionIds)
        {
            var permissionClaims = new List<Claim>();
            foreach(int id in permissionIds)
            {
                permissionClaims.Add(new Claim("Permission", id.ToString()));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            //Client Secret is the key
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("loginSymmetricSecurityKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Sid, account.Id.ToString()),
                    new Claim("DeviceId", deviceId)
                }),
                Issuer = IssuerAudience,
                Audience = IssuerAudience,
                Expires = DateTime.UtcNow.AddDays(ACCESS_TOKEN_DURATION),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            //Adding Permissions to the Subject Claims
            tokenDescriptor.Subject.AddClaims(permissionClaims);

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Authenticates a Login Request coming in the form of AuthnRequest, and creates a AuthnResponse with the correct information
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AuthnResponse Authenticate(in AuthnRequest request)
        {
            //Builds an account from the incoming request
            var account = new Account(request.Email, request.Password, AccountDataAccess.RetrieveId(request.Email));

            Logger.Log("Account Built: " + account.ToString());

            //If User is not found / Unsuccessful Login
            var loginResult = AccountDataAccess.Login(account);

            if(!loginResult.Success)
            {
                Logger.Log(loginResult.Message);
                return null;
            }
            //If the Login Method from the AccountSqlDAO Works, use the same account that you used to start that method and combine it with a retrieved profile in order to create the AuthnResponse
            //Creates the profile, token, device id, and refresh token
            try
            {
                var profile = UserDataAccess.RetrieveProfile(account.Id);
                Logger.Log("Profile Retrieved: " + profile.ToString());

                var deviceId = BuildRandomString(DEVICE_ID_SIZE);
                Logger.Log("DeviceId Generated");

                var permissions = AccountDataAccess.RetrievePermissions(account.Id);
                var permissionIds = Roles.GetIds(permissions);

                var token = BuildAccessToken(account, deviceId, permissionIds);
                var refreshToken = BuildRefreshToken(account.Id, deviceId);

                //Stores the Refresh Token in the Database
                var refreshStoreResult = AccountDataAccess.AddRefreshToken(refreshToken);
                if (refreshStoreResult.Success == false)
                {
                    throw new Exception(refreshStoreResult.Message);
                }
                Logger.Log("Refresh Token successfully stored");

                var response = new AuthnResponse(account, profile, token);

                return response;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// Builds a refresh token with accountId and deviceId
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        private RefreshToken BuildRefreshToken(int accountId, string deviceId)
        {
            var tokenId = BuildRandomString(REFRESH_TOKEN_SIZE);
            var expirationTime = DateTime.UtcNow.AddDays(REFRESH_TOKEN_DURATION);
            var token = new RefreshToken()
            {
                AccountId = accountId,
                RefreshTokenId = tokenId,
                ExpirationDate = expirationTime,
                DeviceId = deviceId
            };

            return token;
        }


        /// <summary>
        /// Builds a random string [bytes] length 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string BuildRandomString(int bytes)
        {
            var randomNumber = new byte[bytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }
}
