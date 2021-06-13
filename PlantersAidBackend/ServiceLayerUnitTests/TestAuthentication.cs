using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlantersAid.ServiceLayer;
using PlantersAid.DataAccessLayer;
using PlantersAid.DataAccessLayer.Interfaces;
using PlantersAid.Models;

namespace ServiceLayerUnitTests
{
    [TestClass]
    public class TestAuthentication

    {
        private AccountService Construct()
        {
            var dao = new AccountSqlDAO();
            return new AccountService(dao);
        }

        [TestMethod]
        public void TestCheckPasswordRequirements_UnderLengthRequirements()
        {

            var mockPassword = "abcd";

            var result = AccountService.CheckPasswordRequirements(mockPassword);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Password does not meet length requirements", result.Message);
        }


        [TestMethod]
        public void TestCheckPasswordRequirements_NoLowercaseLetter()
        {
            var mockPassword = "ABCDEFGH8*";

            var result = AccountService.CheckPasswordRequirements(mockPassword);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Password does not contain a lowercase letter", result.Message);
        }

        [TestMethod]
        public void TestCheckPasswordRequirements_NoUppercaseLetter()
        {
            var mockPassword = "abcdefgh123*";

            var result = AccountService.CheckPasswordRequirements(mockPassword);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Password does not contain an uppercase letter", result.Message);
        }

        [TestMethod]
        public void TestCheckPasswordRequirements_NoDigit()
        {
            var mockPassword = "ABDCefgh****";

            var result = AccountService.CheckPasswordRequirements(mockPassword);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Password does not contain a digit", result.Message);
        }


        [TestMethod]
        public void TestCheckPasswordRequirements_Pass()
        {
            var mockPassword = "ABDCefgh1234****";

            var result = AccountService.CheckPasswordRequirements(mockPassword);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Password met all Requirements", result.Message);
        }

        //[TestMethod]
        //public void TestCreateAccount_Success()
        //{
        //    Result result;

        //    var auth = Construct();
        //    var account = new Account("daniel.gione@gmail.com", "DollaDollaBill*13");

        //    result = auth.CreateAccount(account);

        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual("Inserts into Accounts and Users Table Successful. Insertion into restricted access was successful.", result.Message);

        //}

        //[TestMethod]
        //public void TestLogin_Success()
        //{
        //    Result result;
        //    var auth = Construct();
        //    var account = new Account("daniel.gione@gmail.com", "DollaDollaBill*13");

        //    result = auth.Login(account);

        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual("Account Logged In Successfully", result.Message);

        //}

        //[TestMethod]
        //public void TestChangePassword_Success()
        //{
        //    Result result;
        //    var auth = Construct();
        //    var account = new Account("daniel.gione@gmail.com", "DollaDollaBiller*13");

        //    result = auth.ChangePassword(account);

        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual("Password Successfully Updated", result.Message);
        //}

        //[TestMethod]
        //public void TestLogin_Failure()
        //{
        //    Result result;
        //    var auth = Construct();
        //    var account = new Account("daniel.gione@gmail.com", "DollaDoll13");

        //    result = auth.Login(account);

        //    Assert.IsFalse(result.Success);
        //    Assert.AreEqual("Login Failed", result.Message);
        //}

        //[TestMethod]
        //public void DeleteAccount_Success()
        //{
        //    Result result;
        //    var auth = Construct();
        //    var account = new Account("daniel.gione@gmail.com", "DollaDollaBill*13");

        //    result = auth.DeleteAccount(account);

        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual("Account removed from Restricted Table Successfully. Account removed from Account & User Table Successfully.", result.Message);
        //}


        //[TestMethod]
        //public void DeleteAccount_Failure()
        //{
        //    Result result;
        //    var auth = Construct();
        //    var account = new Account("daniel.g@gmail.com", "DollaDollaBiller*13");

        //    result = auth.DeleteAccount(account);

        //    Assert.IsFalse(result.Success);
        //    Assert.AreEqual("Account does not exist", result.Message);
        //}

        

   
    }
}
