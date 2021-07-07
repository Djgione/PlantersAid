using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.Models
{
    /// <summary>
    /// Request for Authorization in order to Use Specific Service
    /// Permission Request Coming from the Client
    /// </summary>
    public class AuthzRequest
    {
        public string AccessToken { get; set; }
        public string DeviceId { get; set; }

        public AuthzRequest(string accessToken, string deviceId)
        {
            AccessToken = accessToken;
            DeviceId = deviceId;
        }
    }
}
