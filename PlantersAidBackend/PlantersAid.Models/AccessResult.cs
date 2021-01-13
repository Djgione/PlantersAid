using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.Models
{
    public class AccessResult
    {
        bool Success { get; set; }
        string Message { get; set; }

        public AccessResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
