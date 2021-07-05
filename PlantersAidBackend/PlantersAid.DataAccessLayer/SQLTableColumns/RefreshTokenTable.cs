using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantersAid.DataAccessLayer.SQLTableColumns
{
    public static class RefreshTokenTable
    {
        public const string REFRESH_TOKEN_TABLE_NAME = "[PlantersAidAccountsUsers].[dbo].[refreshTokens]";
        public const string ACCOUNTID_COLUMN_NAME = "[accountId]";
        public const string RESFRESHTOKEN_COLUMN_NAME = "[refreshTokenId]";
        public const string DEVICEID_COLUMN_NAME = "[deviceId]";
        public const string EXPIRATIONDATE_COLUMN_NAME = "[expirationDate]";
    }
}
