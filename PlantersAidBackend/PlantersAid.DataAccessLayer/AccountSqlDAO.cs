using System;
using System.Collections.Generic;
using System.Text;
using PlantersAid.DataAccessLayer.Interfaces;
using PlantersAid.Models;

namespace PlantersAid.DataAccessLayer
{
    public class AccountSqlDAO : IAccountDAO
    {
        /// <summary>
        /// SQL Method Implementation for Deleting Account
        /// </summary>
        /// <returns></returns>
        public Result DeleteAccount(Account acc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SQL Method Implementation for Retreiving Account from DB
        /// </summary>
        /// <returns></returns>
        public Account GetAccount(string identifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SQL Method Implementation for Changing Password
        /// </summary>
        /// <returns></returns>
        public Result ChangePassword(Account acc)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SQL Method for Logging In, Checking Password Hash
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public Result Login(Account acc)
        {
            //SELECT EXISTS (SELECT* FROM login_details WHERE username = ? AND password = ?)
            throw new NotImplementedException();
        }

        /// <summary>
        /// SQL Method for Creating Account
        /// </summary>
        /// <returns></returns>
        public Result CreateAccount(Account acc, byte[] salt)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SQL Method for Retrieving Salt
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public byte[] RetrieveSalt(string email)
        {
            throw new NotImplementedException();
        }
    }
}
