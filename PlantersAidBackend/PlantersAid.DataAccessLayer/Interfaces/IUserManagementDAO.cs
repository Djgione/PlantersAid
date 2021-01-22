using PlantersAid.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.DataAccessLayer.Interfaces
{
    public interface IUserManagementDAO
    {
        public Result UpdateProfile(Profile profile);


    }
}
