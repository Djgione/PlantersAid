using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlantersAid.Models;
using PlantersAid.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantersAid.ControllerLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration _config;
        private IAuthentication _authenticationService;

        public AuthenticationController(IConfiguration config, IAuthentication authentication)
        {
            _config = config;
            _authenticationService = authentication;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] AuthnRequest request)
        {
            IActionResult response = Unauthorized();
            var authnResponse = _authenticationService.Authenticate(request);

            if(authnResponse != null)
            {
                response = Ok(authnResponse);
            }

            return response;
        }
       
    }
}
