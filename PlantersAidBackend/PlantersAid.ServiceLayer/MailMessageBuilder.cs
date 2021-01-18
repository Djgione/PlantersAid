using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.ServiceLayer
{
    public class MailMessageBuilder
    {
        MailAddress ToAddress;
        string Subject;
        string Body;
        readonly MailAddress FromAddress;


        public MailMessageBuilder()
        {
            FromAddress = new MailAddress("plantersaid@gmail.com");
        }

        public MailMessageBuilder SetToAddress(String address)
        {
            ToAddress = new MailAddress(address);
            return this;
        }

        public MailMessageBuilder SetSubject(String subject)
        {
            Subject = subject;
            return this;
        }

        public MailMessageBuilder SetBody(String body)
        {
            Body = body;
            return this;
        }

        public MailMessage Build()
        {
            var message = new MailMessage(FromAddress, ToAddress);
            message.Subject = Subject;
            message.Body = Body;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8; 
            return message;
        }
        
    }
}
