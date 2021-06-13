using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PlantersAid.Models;

namespace PlantersAid.ServiceLayer.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates the AuthnRequest incoming from controller
        /// Returns an AuthnResponse with relevant information
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        AuthnResponse Authenticate(AuthnRequest request);
    }
}
