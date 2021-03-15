using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.Models
{
    public enum AuthEnum
    {
        None,
        DeleteAccount,
        DisableAccount,
        EnableAccount,
        EditProfile,
        ViewProfile,
        AddFriend,
        EditProfilePicture,
        UploadPlantPicture,
        DeletePlantPicture,
        AddPlant,
        DeletePlant,
        RemoveFriend,
        SearchPlant,
        BlockPerson,
        UnblockPerson,
    }
}
