using PlantersAid.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.ServiceLayer
{
    public class Authentication
    {
        public IAccountDAO DataAccess { get; }
        public Authentication (IAccountDAO dataAccess)
        {
            DataAccess = dataAccess;
        }




    }
}
