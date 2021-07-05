using System;
using System.Collections.Generic;
using System.Text;
using PlantersAid.DataAccessLayer.Interfaces;
using PlantersAid.Models;
using Microsoft.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using PlantersAid.DataAccessLayer.SQLTableColumns;

namespace PlantersAid.DataAccessLayer
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string _accountsUsersConnectionString;
        private readonly string _restrictedInfoConnectionString;
        private readonly string _restrictedTable = "[PlantersAidRestrictedInfo].[dbo].[passwordinfo]";
        public AccountSqlDAO()
        {
            _accountsUsersConnectionString = Environment.GetEnvironmentVariable("plantersAidAccountUsersConnectionString");
            _restrictedInfoConnectionString = Environment.GetEnvironmentVariable("plantersAidRestrictedInfoConnectionString");
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
        /// Deletes a collection of Id's, Cascade Delete Handled by SQL
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
                    Console.WriteLine("Failed to Retrieve Id");
                    Console.WriteLine(ex.ToString());
                    return -1;
                }

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

    }
}
