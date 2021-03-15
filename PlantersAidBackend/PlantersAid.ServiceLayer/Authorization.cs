using PlantersAid.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.ServiceLayer
{
    public class Authorization
    {
        private const string defaultEmail = "";
        private readonly AuthorizationClaims _authClaims;

        public Authorization(AuthorizationClaims authClaims)
        {
            _authClaims = authClaims;
        }

        public bool Authorize()

    }
}
