using System;
using System.Collections.Generic;
using System.Text;
using PlantersAid.Models;
namespace PlantersAid.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface for Accessing all Data Relating to Accounts
    /// </summary>
    public interface IAccountDAO
    {
        /// <summary>
        /// Method for Retrieving Account from DB
        /// </summary>
        /// <returns></returns>
        public Account GetAccount(string identifier);

        /// <summary>
        /// Method for Deleting Account for DB
        /// </summary>
        /// <returns>AccessResult with Boolean of Success</returns>
        public Result DeleteAccount(Account acc);


        /// <summary>
        /// Method for Changing Password
        /// </summary>
        /// <returns>AccessResult with Boolean of Success</returns>
        public Result ChangePassword(Account acc);

        /// <summary>
        /// Method for Creating Account
        /// </summary>
        /// <returns>AccessResult with Boolean of Success</returns>
        public Result CreateAccount(Account acc, byte[] salt);

        /// <summary>
        /// Retrieves the Salt Associated with the email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Salt if exists, null if none exists</returns>
        public byte[] RetrieveSalt(string email);

        /// <summary>
        /// Check if User Credentials are Valid
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public Result Login(Account acc);

        public Result ClearDatabases();

        public int RetrieveId(string email);
    }
}
