using System;
using System.Collections.Generic;
using System.Text;
using PlantersAid.DataAccessLayer.Interfaces;
using PlantersAid.Models;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using PlantersAid.DataAccessLayer.SQLTableColumns;
using System.Linq;



namespace PlantersAid.DataAccessLayer
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string _accountsUsersConnectionString;
        private readonly string _restrictedInfoConnectionString;
        private readonly string _restrictedTable = "[PlantersAidRestrictedInfo].[dbo].[passwordinfo]";
       // private readonly DatabaseDataContext accountDb;
        public AccountSqlDAO()
        {
            _accountsUsersConnectionString = Environment.GetEnvironmentVariable("plantersAidAccountUsersConnectionString");
            _restrictedInfoConnectionString = Environment.GetEnvironmentVariable("plantersAidRestrictedInfoConnectionString");

           //var accountDb = new DatabaseDataContext(_accountsUsersConnectionString);
        }

        /// <summary>
        /// SQL Method Implementation for Deleting Account
        /// </summary>
        /// <returns></returns>
        public Result DeleteAccount(int accountId)
        {
            Result result;
            var build = new StringBuilder();

            if(accountId < 1)
            {
                result = new Result(false, "Account does not exist");
                return result;
            }


            using (var connection = new SqlConnection(_restrictedInfoConnectionString))
            {

                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();

                command.Transaction = transaction;
                command.Connection = connection;
                try
                {
                    //Deleting Account from the Restricted Database
                    command.CommandText = @"DELETE FROM " + _restrictedTable + @" WHERE accountId = @accountId";
                    command.Parameters.AddWithValue("@accountId", accountId);

                    command.ExecuteNonQuery();
                    transaction.Commit();

                    build.Append("Account removed from Restricted Table Successfully. ");
                }
                catch(Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        result = new Result(false, ex.ToString());
                    }
                    catch(Exception e)
                    {
                        result = new Result(false, $"Outer Exception: {ex} | Inner Exception {e}");
                    }
                    return result;
                }
            }

            using (var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();

                command.Transaction = transaction;
                command.Connection = connection;

                try
                {
                    //Deleting Account from the Restricted Database
                    command.CommandText = @"DELETE FROM " + AccountTable.ACCOUNT_TABLE_NAME + @" WHERE accountId = @accountId";
                    command.Parameters.AddWithValue("@accountId", accountId);

                    command.ExecuteNonQuery();
                    transaction.Commit();

                    build.Append("Account removed from Account & User Table Successfully.");
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        result = new Result(false, ex.ToString());
                    }
                    catch (Exception e)
                    {
                        result = new Result(false, $"Outer Exception: {ex} | Inner Exception {e}");
                    }
                    return result;
                }
            }

            result = new Result(true, build.ToString());
            return result;

        }

     

        /// <summary>
        /// Deletes a collection of RefreshTokenId's, Cascade Delete Handled by SQL
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Result DeleteAccount(ICollection<int> ids)
        {
            Result result;
            StringBuilder build = new StringBuilder();

            using (var connection = new SqlConnection(_restrictedInfoConnectionString))
            {

                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();

                command.Transaction = transaction;
                command.Connection = connection;
                try
                {
                    //Deleting Account from the Restricted Database
                    command.CommandText = @"DELETE FROM " + _restrictedTable + @" WHERE accountId = @accountId";

                    foreach (int id in ids)
                    {
                        command.Parameters.AddWithValue("@accountId", id);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    build.Append("Accounts removed from Restricted Table Successfully. ");
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        result = new Result(false, ex.ToString());
                    }
                    catch (Exception e)
                    {
                        result = new Result(false, $"Outer Exception: {ex} | Inner Exception {e}");
                    }
                    return result;
                }
            }


            using (var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();

                command.Transaction = transaction;
                command.Connection = connection;

                try
                {
                    //Deleting Account from the Restricted Database
                    command.CommandText = @"DELETE FROM " + AccountTable.ACCOUNT_TABLE_NAME + @" WHERE accountId = @accountId";
                    foreach (int id in ids)
                    { 
                        command.Parameters.AddWithValue("@accountId", id);
                        command.ExecuteNonQuery();
                    }
                    transaction.Commit();

                    build.Append("Accounts removed from Account & User Table Successfully.");
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        result = new Result(false, ex.ToString());
                    }
                    catch (Exception e)
                    {
                        result = new Result(false, $"Outer Exception: {ex} | Inner Exception {e}");
                    }
                    return result;

                }
            }

            result = new Result(true, build.ToString());
            return result;
        }

        /// <summary>
        /// SQL Method Implementation for Retreiving Account from DB
        /// </summary>
        /// <returns></returns>
        //public Account GetAccount(int identifier)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// SQL Method Implementation for Changing Password
        /// </summary>
        /// <returns></returns>
        public Result ChangePassword(Account acc)
        {
            Result result;
            var id = RetrieveId(acc.Email);
            if(id < 1)
            {
                result = new Result(false, "Account does not exist");
                return result;
            }

            //Creates connection to restricted info, attempts to change password based on the accountId
            using (var connection = new SqlConnection(_restrictedInfoConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                transaction = connection.BeginTransaction();

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = @"Update " + _restrictedTable + @" Set [password] = @password where accountId = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@password", acc.Password);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    result = new Result(true, "Password Successfully Updated");
                    
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Failed to Update Password");
                    Console.WriteLine(ex.ToString());
                    result = new Result(false, ex.ToString());
                }
            }
            //Creates connection to the Account, attempts to delete all refresh tokens to force login 
            using (var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                transaction = connection.BeginTransaction();

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = @"Delete From " + RefreshTokenTable.REFRESH_TOKEN_TABLE_NAME + @" Where " + RefreshTokenTable.ACCOUNTID_COLUMN_NAME + @" = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    transaction.Commit();

                    result.Message += "All Refresh Tokens have been deleted";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    result.Success = false;
                    result.Message+="Refresh Tokens failed to Delete\n" + ex.ToString();
                }
            }

            return result;

        }
    

        /// <summary>
        /// SQL Method for Logging In, Checking Password Hash
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public Result Login(Account acc)
        {
            Result result;
            int accountId = RetrieveId(acc.Email);
            if(accountId < 1)
            {
                result = new Result(false, "Account does not exist");
                return result;
            }

            using (var connection = new SqlConnection(_restrictedInfoConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();

                try
                {

                    command.CommandText = @"SELECT accountId FROM " + _restrictedTable + @" WHERE EXISTS(SELECT * FROM " + _restrictedTable + @" WHERE accountId = @accountId AND password = @password)";
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.Parameters.AddWithValue("@password", acc.Password);

                    int response;
                    var sqlResult = command.ExecuteScalar();
                    if(sqlResult is null || sqlResult ==DBNull.Value)
                    {
                        response = -1;
                    }
                    else
                    {
                        response = (int)sqlResult;
                    }


                    if(response < 1 )
                    {
                        result = new Result(false, "Login Failed");
                    }
                    else
                    {
                        result = new Result(true, "Account Logged In Successfully");
                    }
                    
                }
                catch(Exception e)
                {
                    result = new Result(false, e.ToString());
                }

                return result;
            }
        }

        /// <summary>
        /// SQL Method for Creating Account
        /// </summary>
        /// <returns></returns>
        public Result CreateAccount(Account acc, byte[] salt)
        {
            Result result;
            int accountId;
            StringBuilder build = new StringBuilder();


            //Connection to main Database
            using (var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                connection.Open();

                /**
                 * This section creates the initial entry within the account 
                 */

                SqlCommand commandInitialCreate = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();

                

                commandInitialCreate.Connection = connection;
                commandInitialCreate.Transaction = transaction;


                /**
                 * This section Creates the entry within the User Table with the Same accountId 
                 */

                try
                {
                    //Inserts into Account Table
                    commandInitialCreate.CommandText =@"Insert Into " + AccountTable.ACCOUNT_TABLE_NAME + @" (email, salt) OUTPUT INSERTED.accountId VALUES (@email, @salt)";
                    commandInitialCreate.Parameters.AddWithValue("@email", acc.Email);
                    commandInitialCreate.Parameters.AddWithValue("@salt", salt);


                    accountId = (int) commandInitialCreate.ExecuteScalar();
                    if (accountId == -1)
                    {
                        result = new Result(false, "Failed to Output Salt");
                        return result;
                    }

                    //Inserts into Users Table
                    commandInitialCreate.CommandText = @"Insert into " + UserTable.USER_TABLE_NAME + @" (accountId) VALUES (@accountId)";
                    commandInitialCreate.Parameters.AddWithValue("@accountId", accountId);
                    commandInitialCreate.ExecuteScalar();

                    transaction.Commit();
                    build.Append("Inserts into Accounts and Users Table Successful. ");
                }
                catch (Exception ex)
                {
                    try
                    {
                        //Attempts to rollback entire transaction
                        transaction.Rollback();
                        result = new Result(false, ex.ToString());
                    }
                    catch (Exception e)
                    {
                        result = new Result(false, $"Outer Exception: {ex}, Inner Exception {e}");
                    }

                    return result;

                } 
            }

            //Connection to other database
            using (var connection = new SqlConnection(_restrictedInfoConnectionString))
            {
                connection.Open();

                SqlCommand commandCreateRestricted = connection.CreateCommand();
                SqlTransaction transactionRestricted = connection.BeginTransaction();

                commandCreateRestricted.Connection = connection;
                commandCreateRestricted.Transaction = transactionRestricted;
                
                try
                {
                    //Inserting new account info into the restricted access database
                    commandCreateRestricted.CommandText = @"Insert into " + _restrictedTable + @" ([accountId], [password]) VALUES (@accountId, @password)";
                    commandCreateRestricted.Parameters.AddWithValue("@accountId", accountId);
                    commandCreateRestricted.Parameters.AddWithValue("@password", acc.Password);
                    commandCreateRestricted.ExecuteNonQuery();

                    transactionRestricted.Commit();
                    build.Append("Insertion into restricted access was successful.");
                }
                catch(Exception ex)
                {
                    try
                    {
                        //Attempts to rollback entire transaction
                        transactionRestricted.Rollback();
                        result = new Result(false, ex.ToString());
                    }
                    catch(Exception e)
                    {
                        build.Append($"Outer Exception: {ex}, Inner Exception {e}");
                        result = new Result(false, build.ToString());
                    }
                    return result;
                }
            }


            result = new Result(true, build.ToString());
            return result;

        }

        /// <summary>
        /// SQL Method for Retrieving Salt
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public byte[] RetrieveSalt(string email)
        {
            using (var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    command.CommandText = @"Select salt from "+ AccountTable.ACCOUNT_TABLE_NAME+ @" where email = @email";
                    command.Parameters.AddWithValue("@email", email);

                    var result = (byte[])command.ExecuteScalar();
                    if (result is null)
                        return null;
                    return result;
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to Retrieve Salt");
                    Console.WriteLine(ex);
                    
                    return null;
                }

            }
        }
    

        public int RetrieveId(string email)
        {
           
            using (var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    command.CommandText = @"SELECT accountId FROM " + AccountTable.ACCOUNT_TABLE_NAME + @" WHERE email = @email";
                    command.Parameters.AddWithValue("@email", email);
                    return (int)command.ExecuteScalar();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Failed to Retrieve RefreshTokenId");
                    Console.WriteLine(ex.ToString());
                    return -1;
                }

            }
        }

        /// <summary>
        /// Retrieving refresh token from Table
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public RefreshToken RetrieveRefreshToken(int accountId, string deviceId)
        {
            using (var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    var refreshToken = new RefreshToken();

                    command.CommandText = @"SELECT " + RefreshTokenTable.RESFRESHTOKEN_COLUMN_NAME + @", " + RefreshTokenTable.EXPIRATIONDATE_COLUMN_NAME + 
                        @" FROM " + RefreshTokenTable.REFRESH_TOKEN_TABLE_NAME + @" WHERE accountId = @accountId 
                        AND deviceId = @deviceId";
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.Parameters.AddWithValue("@deviceId", deviceId);



                    using (var reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            refreshToken.AccountId = accountId;
                            refreshToken.DeviceId = deviceId;
                            refreshToken.RefreshTokenId = reader[RefreshTokenTable.RESFRESHTOKEN_COLUMN_NAME].ToString();
                            refreshToken.ExpirationDate = Convert.ToDateTime(reader[RefreshTokenTable.EXPIRATIONDATE_COLUMN_NAME].ToString());
                        }
                    }

                    return refreshToken;
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }

        /// <summary>
        /// Used to update a single refresh token located at specific account id / device id
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Result UpdateRefreshToken(RefreshToken token)
        {

            Result result;

            using (var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                try
                {
                    command.CommandText = @"UPDATE " + RefreshTokenTable.REFRESH_TOKEN_TABLE_NAME + @" SET " +
                        RefreshTokenTable.RESFRESHTOKEN_COLUMN_NAME + @" = @refreshTokenId, " + RefreshTokenTable.EXPIRATIONDATE_COLUMN_NAME +
                        @" = @expirationDate WHERE " + RefreshTokenTable.ACCOUNTID_COLUMN_NAME + @" = @accountId AND " + RefreshTokenTable.DEVICEID_COLUMN_NAME +
                        @" = @deviceId";

                    command.Parameters.AddWithValue("@refreshTokenId", token.RefreshTokenId);
                    command.Parameters.AddWithValue("@expirationDate", token.ExpirationDate);
                    command.Parameters.AddWithValue("@accountId", token.AccountId);
                    command.Parameters.AddWithValue("@deviceId", token.DeviceId);


                    command.ExecuteNonQuery();
                    transaction.Commit();
                    result = new Result(true, "Refresh Token Successfully Updated");
                }
                catch (Exception outer)
                {

                    try
                    {
                        transaction.Rollback();
                        result = new Result(false, outer.ToString());

                    }
                    catch (Exception inner)
                    {
                        result = new Result(false, outer.ToString() + " | " + inner.ToString());
                    }
                }

                return result;
            }
            
        }

        public Result AddRefreshToken(RefreshToken token)
        {
            Result result;

            using (var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                try
                {
                    command.CommandText = @"INSERT INTO " + RefreshTokenTable.REFRESH_TOKEN_TABLE_NAME + @" (" +
                        RefreshTokenTable.ACCOUNTID_COLUMN_NAME + @", " + RefreshTokenTable.DEVICEID_COLUMN_NAME + @", " +
                        RefreshTokenTable.RESFRESHTOKEN_COLUMN_NAME + @", " + RefreshTokenTable.EXPIRATIONDATE_COLUMN_NAME +
                        @") VALUES (@accountId, @deviceId, @refreshTokenId, @expirationDate)";

                    command.Parameters.AddWithValue("@refreshTokenId", token.RefreshTokenId);
                    command.Parameters.AddWithValue("@expirationDate", token.ExpirationDate);
                    command.Parameters.AddWithValue("@accountId", token.AccountId);
                    command.Parameters.AddWithValue("@deviceId", token.DeviceId);


                    command.ExecuteNonQuery();
                    transaction.Commit();
                    result = new Result(true, "Refresh Token Successfully Added");
                }
                catch (Exception outer)
                {

                    try
                    {
                        transaction.Rollback();
                        result = new Result(false, outer.ToString());

                    }
                    catch (Exception inner)
                    {
                        result = new Result(false, outer.ToString() + " | " + inner.ToString());
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Used to clear all refresh tokens related to specific account, or a specific one based on Id
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public Result ClearRefreshToken(int accountId, string deviceId = "")
        {
            Result result;

            using (var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                try
                {
                    command.CommandText = @"DELETE * FROM " + RefreshTokenTable.REFRESH_TOKEN_TABLE_NAME +
                        @" WHERE " + RefreshTokenTable.ACCOUNTID_COLUMN_NAME + @" = @accountId";
                    command.Parameters.AddWithValue("@accountId", accountId);

                    if(!String.IsNullOrEmpty(deviceId))
                    {
                        command.CommandText += @" AND " + RefreshTokenTable.DEVICEID_COLUMN_NAME + @" = @deviceId";
                        command.Parameters.AddWithValue("@deviceId", deviceId);
                    }

                    command.ExecuteNonQuery();
                    transaction.Commit();
                    result = new Result(true, "Refresh Token(s) Cleared");
                }
                catch (Exception outer)
                {
                    try
                    {
                        transaction.Rollback();
                        result = new Result(false, outer.ToString());
                        
                    }
                    catch (Exception inner)
                    {
                        result = new Result(false, outer.ToString() + " | " + inner.ToString());   
                    }
                }

                return result;
            }
        }

        private String WrapParameters(string str)
        {
            return "\'" + str + "\'";
        }

        public Result ClearDatabases()
        {
            Result result;
            var build = new StringBuilder();

            using (var connection = new SqlConnection(_restrictedInfoConnectionString))
            {

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();

                command.Transaction = transaction;

                try
                {
                    //Deleting Account from the Restricted Database
                    command.CommandText = @"DELETE FROM " + _restrictedTable;

                    command.ExecuteNonQuery();
                    transaction.Commit();

                    build.Append("Accounts removed from Restricted Table Successfully. ");
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        result = new Result(false, ex.ToString());
                    }
                    catch (Exception e)
                    {
                        result = new Result(false, $"Outer Exception: {ex} | Inner Exception {e}");
                    }
                    return result;
                }
            }

            using (var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();

                command.Transaction = transaction;

                try
                {
                    //Deleting Account from the Restricted Database
                    command.CommandText = "DELETE FROM " +  AccountTable.ACCOUNT_TABLE_NAME;

                    command.ExecuteNonQuery();
                    transaction.Commit();

                    build.Append("Account removed from Account & User Table Successfully. ");
                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                        result = new Result(false, ex.ToString());
                    }
                    catch (Exception e)
                    {
                        result = new Result(false, $"Outer Exception: {ex} | Inner Exception {e}");
                    }
                    return result;
                }
            }

            result = new Result(true, build.ToString());
            return result;
        }

        public IEnumerable<Permission> RetrievePermissions(int accountId)
        {
            List<Permission> permissions;
            using (var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    connection.Open();
                    command.CommandText = @"SELECT " + PermissionAccountMappingTable.PERMISSION_ID_NAME + @" FROM " + PermissionAccountMappingTable.PERMISSION_ACCOUNT_MAPPING_TABLE_NAME +
                        @" WHERE " + PermissionAccountMappingTable.ACCOUNT_ID_NAME + @" = @accountId";
                    command.Parameters.AddWithValue("@accountId", accountId);
                    permissions = new List<Permission>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                permissions.Add((Permission)reader.GetInt32(i));
                            }
                        }
                    }

                    return permissions;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public Result RemovePermissions(int accountId, ref IEnumerable<Permission> permissions)
        {
            Result result;
            using (var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                command.Connection = connection;

                var permissionCSV = PermissionToCSV(permissions);

                try
                {
                    command.CommandText = @"DELETE FROM " + PermissionAccountMappingTable.PERMISSION_ACCOUNT_MAPPING_TABLE_NAME + @" WHERE " + PermissionAccountMappingTable.ACCOUNT_ID_NAME + @" = @accountId AND " +
                        PermissionAccountMappingTable.PERMISSION_ID_NAME + @" IN (@permissions)";
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.Parameters.AddWithValue("@permissions", permissionCSV);
                    command.ExecuteNonQuery();

                    result = new Result(true, "Permissions Removed Successfully");
                    
                }
                catch (Exception ex)
                {
                    result = new Result(false, ex.ToString());
                    
                }
                return result;
            }
        }

        /// <summary>
        /// Turns List of Permissions into a CSV String
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static String PermissionToCSV(IEnumerable<Permission> list)
        {
            var permissionCSV = "";

            using (var permissionEnum = list.GetEnumerator())
            {
                while (permissionEnum.MoveNext())
                {
                    permissionCSV += (int)permissionEnum.Current + ", ";
                }
            }

            permissionCSV = permissionCSV.Substring(0, permissionCSV.Length - 2);

            return permissionCSV;
        }

        private static String PermissionAccountToCSV(int accountId, IEnumerable<Permission> list)
        {
            var result = "";
            using (var enumerator = list.GetEnumerator())
            {
                while(enumerator.MoveNext())
                {
                    result += "(" + accountId + ", " + (int)enumerator.Current + "), ";
                }
            }

            return result.Substring(0, result.Length - 2);
            
        }

        public Result AddPermissions(int accountId, ref IEnumerable<Permission> permissions)
        {
            Result result;
            using(var connection = new SqlConnection(_accountsUsersConnectionString))
            {
                var command = connection.CreateCommand();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                command.Connection = connection;
                var valuesString = PermissionAccountToCSV(accountId, permissions);

                try
                {
                    command.CommandText = @"INSERT INTO " + PermissionAccountMappingTable.PERMISSION_ACCOUNT_MAPPING_TABLE_NAME + @"( " + PermissionAccountMappingTable.ACCOUNT_ID_NAME + @", " + PermissionAccountMappingTable.PERMISSION_ID_NAME + @") VALUES " + valuesString;
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.ExecuteNonQuery();
                    result = new Result(true, "Permissions added Successfully");
                }
                catch (Exception ex)
                {
                    result = new Result(false, ex.ToString());
                }
                return result;
            }
        }
    }
}
