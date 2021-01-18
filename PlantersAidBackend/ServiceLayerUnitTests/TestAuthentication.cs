using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlantersAid.ServiceLayer;
using PlantersAid.DataAccessLayer;
using PlantersAid.DataAccessLayer.Interfaces;

namespace ServiceLayerUnitTests
{
    [TestClass]
    public class TestAuthentication

    {
        private Authentication Construct()
        {
            var fakeDAO = new AccountSqlDAO();
            return new Authentication(fakeDAO);
        }

        [TestMethod]
        public void TestCheckPasswordRequirements_UnderLengthRequirements()
        {
            
            var mockPassword = "abcd";

            var result = Authentication.CheckPasswordRequirements(mockPassword);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Password does not meet length requirements", result.Message);
        }


        [TestMethod]
        public void TestCheckPasswordRequirements_NoLowercaseLetter()
        {
            var mockPassword = "ABCDEFGH8*";

            var result = Authentication.CheckPasswordRequirements(mockPassword);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Password does not contain a lowercase letter", result.Message);
        }

        [TestMethod]
        public void TestCheckPasswordRequirements_NoUppercaseLetter()
        {
            var mockPassword = "abcdefgh123*";

            var result = Authentication.CheckPasswordRequirements(mockPassword);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Password does not contain an uppercase letter", result.Message);
        }

        [TestMethod]
        public void TestCheckPasswordRequirements_NoDigit()
        {
            var mockPassword = "ABDCefgh****";

            var result = Authentication.CheckPasswordRequirements(mockPassword);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Password does not contain a digit", result.Message);
        }


        [TestMethod]
        public void TestCheckPasswordRequirements_Pass()
        {
            var mockPassword = "ABDCefgh1234****";

            var result = Authentication.CheckPasswordRequirements(mockPassword);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Password met all Requirements", result.Message);
        }

    }
}
