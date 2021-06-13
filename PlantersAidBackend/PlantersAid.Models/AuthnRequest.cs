using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PlantersAid.Models
{
    /// <summary>
    /// Model for an Authentication Request
    /// </summary>
    public class AuthnRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
