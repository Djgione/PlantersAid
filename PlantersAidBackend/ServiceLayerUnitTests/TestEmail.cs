using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlantersAid.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayerUnitTests
{
    [TestClass]
    public class TestEmail
    {

        [TestMethod]
        public void MailMessageBuilder_SuccessfulMailMessage()
        {
            var builder = new MailMessageBuilder();
            builder.SetBody("Body");
            builder.SetSubject("Subject");
            builder.SetToAddress("rebeccadiaz580@gmail.com");
            var message = builder.Build();

            Assert.IsInstanceOfType(message, typeof(MailMessage));
            Assert.AreEqual(message.Body, "Body");
            Assert.AreEqual(message.Subject, "Subject");
            Assert.AreEqual(message.To.ToString(), "rebeccadiaz580@gmail.com");
            Assert.AreEqual(message.From.ToString(), "plantersaid@gmail.com");
        }


        //[TestMethod]
        //public void SendMessage_Success()
        //{
        //    var builder = new MailMessageBuilder();
        //    builder.SetBody("Body");
        //    builder.SetSubject("Subject");
        //    builder.SetToAddress("daniel.gione@gmail.com");
        //    var message = builder.Build();

        //    var result = Email.SendEmail(message);
        //    Assert.IsTrue(result.Success);
        //}
    }
}
