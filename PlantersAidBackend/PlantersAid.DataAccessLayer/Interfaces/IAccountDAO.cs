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
        //public Account GetAccount(int identifier);

        /// <summary>
        /// Method for Deleting Account for DB
        /// </summary>
        /// <returns>AccessResult with Boolean of Success</returns>
        public Result DeleteAccount(int id);


        /// <summary>
        /// Method for Deleting Many Accounts at Once
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Result DeleteAccount(ICollection<int> ids);
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

        /// <summary>
        /// Retrieves the refresh token based on account and device id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public RefreshToken RetrieveRefreshToken(int accountId, string deviceId);

        /// <summary>
        /// Deletes an existing refresh token
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public Result ClearRefreshToken(int accountId, string deviceId = null);
        /// <summary>
        /// Updates an already Existing Refresh Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>

        public Result UpdateRefreshToken(RefreshToken token);

        /// <summary>
        /// Creates a brand new refresh token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>

        public Result AddRefreshToken(RefreshToken token);

        /// <summary>
        /// Retrieves all Permissions for the accountId
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public IEnumerable<Permission> RetrievePermissions(int accountId);

        /// <summary>
        /// Removes all permissions from the account based on id
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public Result RemovePermissions(int accountId, ref IEnumerable<Permission> permissions);

        /// <summary>
        /// Adds permissions to account based on ID
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public Result AddPermissions(int accountId, ref IEnumerable<Permission> permissions);
    }
}
