using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.Models
{
    /// <summary>
    /// Minimum Information to Interact on the Application
    /// Used mainly for identification and authentication
    /// </summary>
    public class Account
    {
        public string Email { get; }
        public string Password { get; set; }

        public Account(string email, string password = "")
        {
            Email = email;
            Password = password;
        }

    }
}
