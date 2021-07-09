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
                var result = DataAccess.UpdateProfile(id, profile);
                Logger.Log(result.ToString());
                return result;
            }
            catch(Exception ex)
            {
                Logger.Log(ex.ToString());
                return new Result(false, "Profile failed to update | " + ex.ToString());
            }
        }

        public Result UpdateProfilePicture(int id, byte[] image)
        {
            try
            {
                var result =  DataAccess.UpdateProfilePicture(id, image);
                Logger.Log(result.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                return new Result(false, "Profile picture failed to update | " + ex.ToString());
            }
          
        }

        public Profile RetrieveProfile(int id)
        {
            try
            {
                var result = DataAccess.RetrieveProfile(id);
                Logger.Log("Profile Retrieved : " + result.ToString());
                return result;
            }
            catch(Exception ex)
            {
                Logger.Log(ex.ToString());
                return null;
            }
        }


        public byte[] RetrieveProfilePicture(int id)
        {
            try
            {
                var result = DataAccess.RetrieveProfilePicture(id);
                Logger.Log("Profile Picture retrieved fromm Database for User: " + id);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString());
                return null;
            }
        }


    }
}
