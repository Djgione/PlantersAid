using System;
using System.Collections.Generic;
using System.Text;
using PlantersAid.Models;
namespace PlantersAid.DataAccessLayer.Interfaces
{
    public interface IAccountDAO
    {
        public Account GetAccount();

        public AccessResult DeleteAccount();

    }
}
