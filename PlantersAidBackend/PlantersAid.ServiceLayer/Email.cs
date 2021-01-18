using PlantersAid.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.ServiceLayer
{
    public class Email
    {
        /// <summary>
        /// Email Sending, Can be Used in Conjunction with MailMessageBuilder
        /// </summary>
        
        public static Result SendEmail(MailMessage email)
        {
            var creds = new NetworkCredential("plantersaid@gmail.com", Environment.GetEnvironmentVariable("plantersAidEmailPassword"));
            Result result;
            var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = creds
            };

            try
            {
                client.Send(email);
                result = new Result(true, "Message Sent!");
            }
            catch (Exception e)
            {
                result = new Result(false, e.ToString());
            }

            email.Dispose();
            return result;
        }

        
    }
}
