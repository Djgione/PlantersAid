using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PlantersAid.ServiceLayer.Interfaces
{
    public interface ITokenBuilder
    {
        /// <summary>
        /// Build token for JWT to send to FrontEnd
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string BuildToken(string email);
    }
}
