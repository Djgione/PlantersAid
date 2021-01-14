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
        public Account GetAccount();

        /// <summary>
        /// Method for Deleting Account for DB
        /// </summary>
        /// <returns></returns>
        public AccessResult DeleteAccount();


        /// <summary>
        /// Method for Changing Password
        /// </summary>
        /// <returns></returns>
        public AccessResult ChangePassword();

    }
}
