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
        public AccessResult DeleteAccount()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SQL Method Implementation for Retreiving Account from DB
        /// </summary>
        /// <returns></returns>
        public Account GetAccount()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SQL Method Implementation for Changing Password
        /// </summary>
        /// <returns></returns>
        public AccessResult ChangePassword()
        {
            throw new NotImplementedException();
        }
    }
}
