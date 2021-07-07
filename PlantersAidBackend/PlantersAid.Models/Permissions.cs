using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.Models
{
    public enum Permissions
    {
        [Display(GroupName = "Account", Name = "DeleteAccount", Description = "Can Delete Their Own Account")]
        DeleteAccount = 0x10,
        [Display(GroupName = "Account", Name = "DisableAccount", Description = "Deactivate Account but Save Data")]
        DisableAccount = 0x11,
        [Display(GroupName = "Account", Name = "EditProfile", Description = "Editing the User's Own Profile")]
        EditProfile = 0x12,
        None,

       [Display(GroupName = "Social", Name = "AddFriend", Description = "Adds a Friend to your Friend List")]
       AddFriend = 0x30,
       [Display(GroupName = "Social", Name = "RemoveFriend", Description = "Removes a Friend from your Friend List")]
       RemoveFriend = 0x31,
       [Display(GroupName = "Social", Name = "BlockPerson", Description = "Blocks a Person from Your Friend List and Contacting")]
       BlockPerson = 0x32,
       [Display(GroupName = "Social", Name = "UnblockPerson", Description = "Unblocks a Person from Your Friend List and Contacting")]
       UnblockPerson = 0x33,


        EnableAccount,
        
        ViewProfile,
       
        EditProfilePicture,
        UploadPlantPicture,
        DeletePlantPicture,
        AddPlant,
        DeletePlant,
        
        SearchPlant,
       
    }
}
