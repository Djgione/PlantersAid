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
        public string AccessToken { get; set; }


        public AuthnResponse(Account account, Profile profile, string token)
        {
            Id = account.Id;
            Username = profile.Username;
            FirstName = profile.FirstName;
            LastName = profile.LastName;
            Gender = profile.Gender;
            Email = account.Email;
            DateOfBirth = profile.DateOfBirth;
            AccessToken = token;
        }

        public AuthnResponse(int id, string username, string firstName, string lastName, string email, string gender, 
            DateTime dateOfBirth, string token)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Gender = gender;
            DateOfBirth = dateOfBirth;
        }
    }
}
