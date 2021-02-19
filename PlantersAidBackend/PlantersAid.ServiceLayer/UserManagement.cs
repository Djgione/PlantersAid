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
            try
            {
                return DataAccess.UpdateProfile(id, profile);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public Result UpdateProfilePicture(int id, byte[] image)
        {
            try
            {
                return DataAccess.UpdateProfilePicture(id, image);
            }
            catch (Exception)
            { 
                throw;
            }
          
        }

        public Profile RetrieveProfile(int id)
        {
            try
            {
                return DataAccess.RetrieveProfile(id);
            }
            catch(Exception)
            {
                throw;
            }
        }


        public byte[] RetrieveProfilePicture(int id)
        {
            try
            {
                return DataAccess.RetrieveProfilePicture(id);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
