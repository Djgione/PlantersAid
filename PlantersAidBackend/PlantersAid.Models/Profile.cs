using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.Models
{
    public class Profile
    {
        public  string Username { get; set; }
        public  string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }

        public Profile(string username, string firstName, string lastName, string gender, DateTime dateOfBirth)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }
    }
}
