using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.Models
{
    public class Account
    {
        string Email { get; }
        string Password { get; }

        public Account(string email, string password)
        {
            Email = email;
            Password = password;
        }


    }
}
