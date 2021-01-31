using PlantersAid.DataAccessLayer.Interfaces;
using PlantersAid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.ServiceLayer
{
    public class UserManagement 
    {

        IUserManagementDAO DataAccess { get; }

        public UserManagement(IUserManagementDAO dao)
        {
            DataAccess = dao;
        }

        public Result UpdateProfile(int id, Profile profile)
        {
            return DataAccess.UpdateProfile(id, profile);
        }




    }
}
