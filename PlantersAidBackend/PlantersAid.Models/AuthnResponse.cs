using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.Models
{
    /// <summary>
    /// Model that is returned upon a Successfulo Authentication Request
    /// </summary>
    public class AuthnResponse
    {
        public int Id {get;set;}
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }


        public AuthnResponse(Account account, Profile profile, string token, string deviceId)
        {
            Id = account.Id;
            Username = profile.Username;
            FirstName = profile.FirstName;
            LastName = profile.LastName;
            Gender = profile.Gender;
            Email = account.Email;
            DateOfBirth = profile.DateOfBirth;
            Token = token;
            DeviceId = deviceId;
        }
    }
}
