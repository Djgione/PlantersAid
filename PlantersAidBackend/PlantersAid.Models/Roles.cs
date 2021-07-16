using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.Models
{
    /// <summary>
    /// Contains Pre-Made Lists of Permissions based on roles
    /// </summary>
    public static class Roles
    {
        /// <summary>
        /// Role used to create a new user
        /// </summary>
        public static readonly IEnumerable<Permission> NewUser = new List<Permission>()
        {
        Permission.AddCustomPlant,
        Permission.AddFriend,
        Permission.AddPlant,
        Permission.AddPlantPicture,
        Permission.BlockPerson,
        Permission.DeleteAccount,
        Permission.DisableAccount,
        Permission.DisplayResult,
        Permission.EditProfile,
        Permission.EditProfilePicture,
        Permission.RemoveFriend,
        Permission.RemovePlant,
        Permission.RemovePlantPicture,
        Permission.SearchPlant,
        Permission.UnblockPerson,
        Permission.ViewProfile,
        };

        /// <summary>
        /// Role used to assign admin privileges
        /// </summary>
        public static readonly IEnumerable<Permission> Admin = new List<Permission>()
        {
            Permission.EnableAccount,
            Permission.Admin
        };

        public static IEnumerable<int> GetIds(IEnumerable<Permission> permissions)
        {
            using (var enumerator = permissions.GetEnumerator())
            {
                while(enumerator.MoveNext())
                {
                    yield return (int)enumerator.Current;
                }
            }
        }
    }
}
