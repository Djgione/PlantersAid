using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.Models
{
    public class Profile
    {
        public  string Username { get; }
        public  string FirstName { get; }
        public string LastName { get; }
        public DateTime DateOfBirth { get; }
        public string Gender { get; }

        public Profile(string username, string firstName, string lastName, DateTime dateOfBirth, string gender)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }
    }
}
