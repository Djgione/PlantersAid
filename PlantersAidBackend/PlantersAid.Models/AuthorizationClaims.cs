using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.Models
{
    public class AuthorizationClaims
    {
        public string Email { get; }
        public AuthEnum[] Claims {get;}

        public AuthorizationClaims(string email, AuthEnum[] claims)
        {
            Email = email;
            Claims = claims;
        }

    }
}
