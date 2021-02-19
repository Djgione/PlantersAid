using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.DataAccessLayer.SQLTableColumns
{
    public static class UserTable
    {
        public const string USER_TABLE_NAME = "[PlantersAidAccountsUsers].[dbo].[users]";
        public const string USERNAME_COLUMN_NAME = "[Username]";
        public const string FIRSTNAME_COLUMN_NAME = "[FirstName]";
        public const string LASTNAME_COLUMN_NAME = "[LastName]";
        public const string DATEOFBIRTH_COLUMN_NAME = "[DateOfBirth]";
        public const string GENDER_COLUMN_NAME = "[Gender]";
        public const string ACCOUNTID_COLUMN_NAME = "[accountId]";
    }
}
