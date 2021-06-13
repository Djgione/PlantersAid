using PlantersAid.Models.Interfaces;
using System.Text.Json.Serialization;
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
    public class Account : Maskable
    {
        public int Id { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public Account(string email, string password = "", int id = -1)
        {
            Id = id;
            Email = email;
            Password = password;
        }

        public override string ToString()
        {
            return "Email: " + Email + ", Password: " + Mask();
        }
    }
}
