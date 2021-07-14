using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.Models
{
    public enum Permission
    {
        [Display(GroupName = "Account", Name = "DeleteAccount", Description = "Can Delete Their Own Account")]
        DeleteAccount = 10,
        [Display(GroupName = "Account", Name = "DisableAccount", Description = "Deactivate Account but Save Data")]
        DisableAccount = 11,
        [Display(GroupName = "Account", Name = "EditProfile", Description = "Editing the User's Own Profile")]
        EditProfile = 12,
        [Display(GroupName = "Account", Name = "EditProfilePicture", Description = "Editing the User's Own Profile Picture")]
        EditProfilePicture = 13,
        [Display(GroupName = "Account", Name = "EnableAccount", Description = "Enabling a Disabled Account to Reactivate it")]
        EnableAccount = 14,
        
        [Display(GroupName = "Search", Name = "SearchPlant", Description = "Searches for a Plant in the Database")]
        SearchPlant = 1,
        [Display(GroupName = "Search", Name = "DisplayResult", Description = "Displays the results of a search")]
        DisplayResult = 2,
        

        None = 0,

        [Display(GroupName = "Social", Name = "AddFriend", Description = "Adds a Friend to your Friend List")]
        AddFriend = 30,
        [Display(GroupName = "Social", Name = "RemoveFriend", Description = "Removes a Friend from your Friend List")]
        RemoveFriend = 31,
        [Display(GroupName = "Social", Name = "BlockPerson", Description = "Blocks a Person from Your Friend List and Contacting")]
        BlockPerson = 32,
        [Display(GroupName = "Social", Name = "UnblockPerson", Description = "Unblocks a Person from Your Friend List and Contacting")]
        UnblockPerson = 33,
        [Display(GroupName = "Social", Name = "ViewProfile", Description = "Views a Person's Profile")]
        ViewProfile = 34,
    
        [Display(GroupName = "Garden", Name = "AddCustomPlant", Description = "Adds a Plant (that does not exist in the database currently) to your Personal Garden")]
        AddCustomPlant = 20,
        [Display(GroupName = "Garden", Name = "RemovePlant", Description = "Removes a Plant from your Personal Garden")]
        RemovePlant = 21,
        [Display(GroupName = "Garden", Name = "AddPlantPicture", Description = "Adds a picture of your plant")]
        AddPlantPicture = 22,
        [Display(GroupName = "Garden", Name = "RemovePlantPicture", Description = "Removes a picture of your plant")]
        RemovePlantPicture = 23,
        [Display(GroupName = "Garden", Name = "AddPlant", Description = "Adds a plant (that exists in the database) to your Personal Garden")]
        AddPlant = 24,

       
    }
    


}
