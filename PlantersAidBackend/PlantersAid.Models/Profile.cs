using PlantersAid.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.Models
{
    public class Profile : Maskable
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

        public override string ToString()
        {
            return "Username: " + Mask() + " , First Name: " + Mask() + ", Last Name: " + Mask() + "Gender: " + Gender + ", Date of Birth: " + DateOfBirth;
        }
    }
}
