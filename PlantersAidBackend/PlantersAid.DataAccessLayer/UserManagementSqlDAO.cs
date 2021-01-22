using System;
using System.Collections.Generic;
using System.Text;
using PlantersAid.DataAccessLayer.Interfaces;
using PlantersAid.Models;

namespace PlantersAid.DataAccessLayer
{
    public class UserManagementSqlDAO : IUserManagementDAO
    {
        private readonly string _connectionString;

        public UserManagementSqlDAO()
        {
            _connectionString = Environment.GetEnvironmentVariable("plantersAidAccountUsersConnectionString");
        }

        public Result UpdateProfile(Profile profile)
        {
            Result result;


            throw new NotImplementedException();

        }
    }
}
