using System;
using System.Collections.Generic;
using System.Text;

namespace PlantersAid.DataAccessLayer.SQLTableColumns
{
    public static class AccountTable
    {
        public const string ACCOUNT_TABLE_NAME = "[PlantersAidAccountsUsers].[dbo].[account]";
        public const string SALT_COLUMN_NAME = "[salt]";
        public const string EMAIL_COLUMN_NAME = "[email]";
        public const string ACCOUNT_ID_COLUMN_NAME = "[accountId]";
    }
}
