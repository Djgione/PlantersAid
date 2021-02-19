using PlantersAid.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.DataAccessLayer.Interfaces
{
    public interface IUserManagementDAO
    {
        public Result UpdateProfile(int id, Profile profile);

        public Result UpdateProfilePicture(int id, byte [] image);

        public Profile RetrieveProfile(int id);

        public byte[] RetrieveProfilePicture(int id);

    }
}
