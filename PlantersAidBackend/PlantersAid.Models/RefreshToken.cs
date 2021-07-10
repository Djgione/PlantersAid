using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.Models
{
    public class RefreshToken
    {
        public int AccountId { get; set; }
        public string RefreshTokenId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string DeviceId { get; set; }

    }
}
