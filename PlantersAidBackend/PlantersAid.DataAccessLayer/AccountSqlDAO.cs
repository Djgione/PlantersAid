using System;
using System.Collections.Generic;
using System.Text;
using PlantersAid.DataAccessLayer.Interfaces;
using PlantersAid.Models;
using Microsoft.Data.SqlClient;
namespace PlantersAid.DataAccessLayer
{
    public class AccountSqlDAO : IAccountDAO
    {
        readonly string AccountsUsersConnectionString;
        readonly string RestrictedInfoConnectionString;
        readonly string AccountTable = "account";
        readonly string UserTable = "user";
        readonly string RestrictedTable = "passwordinfo";
        public AccountSqlDAO()
        {
            AccountsUsersConnectionString = Environment.GetEnvironmentVariable("plantersAidAccountUsersConnectionString");
            RestrictedInfoConnectionString = Environment.GetEnvironmentVariable("plantersAidRestrictedInfoConnectionString");
        }

        /// <summary>
        /// SQL Method Implementation for Deleting Account
        /// </summary>
        /// <returns></returns>
        public Result DeleteAccount(Account acc)
        {
            Result result;
            int accountId = RetrieveId(acc.Email);
            var build = new StringBuilder();

            if(accountId == -1)
            {
                result = new Result(false, "Account does not exist");
                return result;
            }


            using (var connection = new SqlConnection(RestrictedInfoConnectionString))
            {

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();

                command.Transaction = transaction;

                try
                {
                    //Deleting Account from the Restricted Database
                    command.CommandText = "DELETE FROM @tableName WHERE accountId = @accountId";
                    command.Parameters.AddWithValue("@tableName", RestrictedTable);
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

            using (var connection = new SqlConnection(AccountsUsersConnectionString))
            {
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();

                command.Transaction = transaction;

                try
                {
                    //Deleting Account from the Restricted Database
                    command.CommandText = "DELETE FROM @tableName WHERE accountId = @accountId";
                    command.Parameters.AddWithValue("@tableName", AccountTable);
                    command.Parameters.AddWithValue("@accountId", accountId);

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

        /// <summary>
        /// SQL Method Implementation for Retreiving Account from DB
        /// </summary>
        /// <returns></returns>
        public Account GetAccount(string identifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SQL Method Implementation for Changing Password
        /// </summary>
        /// <returns></returns>
        public Result ChangePassword(Account acc)
        {
            Result result;
            var id = RetrieveId(acc.Email);
            if(id == -1)
            {
                result = new Result(false, "Account does not exist");
                return result;
            }


            using (var connection = new SqlConnection(RestrictedInfoConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                transaction = connection.BeginTransaction();

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "Update password from @tableName where accoundId = @id";
                    command.Parameters.AddWithValue("@tableName", AccountTable);
                    command.Parameters.AddWithValue("@id", id);
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
                return result;
            }
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
            if(accountId == -1)
            {
                result = new Result(false, "Account does not exist");
                return result;
            }

            using (var connection = new SqlConnection(RestrictedInfoConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();

                try
                {

                    command.CommandText = "SELECT EXISTS (SELECT * FROM @tableName WHERE accountId = @accountId AND password = @password)";
                    command.Parameters.AddWithValue("@tableName", RestrictedTable);
                    command.Parameters.AddWithValue("@accountId", accountId);
                    command.Parameters.AddWithValue("@password", acc.Password);

                    var rowsValid = (int) command.ExecuteScalar();
                    if(rowsValid != 1)
                    {
                        result = new Result(false, "Account was not found");
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
            using (var connection = new SqlConnection(AccountsUsersConnectionString))
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

                SqlCommand commandCreateUserEntry = connection.CreateCommand();

                commandCreateUserEntry.Connection = connection;
                commandCreateUserEntry.Transaction = transaction;

                try
                {
                    //Inserts into Account Table
                    commandInitialCreate.CommandText = "Insert Into @tableName (email, salt) VALUES (@email, @salt)";
                    commandInitialCreate.Parameters.AddWithValue("@tableName", AccountTable);
                    commandInitialCreate.Parameters.AddWithValue("@email", acc.Email);
                    commandInitialCreate.Parameters.AddWithValue("@salt", Encoding.ASCII.GetString(salt));
                    commandInitialCreate.ExecuteNonQuery();

                    //Retrieves AccountId from AccountTable
                    accountId = RetrieveId(acc.Email);
                    if (accountId == -1)
                    {
                        result = new Result(false, "Failed to Retrieve Salt");
                        return result;
                    }

                    //Inserts into Users Table
                    commandCreateUserEntry.CommandText = "Insert into @tableName (accountId) VALUES (@accountId)";
                    commandCreateUserEntry.Parameters.AddWithValue("@tableName", UserTable);
                    commandCreateUserEntry.Parameters.AddWithValue("@accountId", accountId);
                    commandCreateUserEntry.ExecuteNonQuery();

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
            using (var connection = new SqlConnection(RestrictedInfoConnectionString))
            {
                connection.Open();

                SqlCommand commandCreateRestricted = connection.CreateCommand();
                SqlTransaction transactionRestricted = connection.BeginTransaction();

                try
                {
                    //Inserting new account info into the restricted access database
                    commandCreateRestricted.CommandText = "Insert into @tableName(accountId, password) VALUES (@accountId, @password)";
                    commandCreateRestricted.Parameters.AddWithValue("@tableName", RestrictedTable);
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
            using (var connection = new SqlConnection(AccountsUsersConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;

                try
                {
                    command.CommandText = "Select salt from @tableName where email = @email";
                    command.Parameters.AddWithValue("@tableName", AccountTable);
                    command.Parameters.AddWithValue("@email", email);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var result = reader.GetString(0);
                        return Encoding.ASCII.GetBytes(result);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to Retrieve Salt");
                    Console.WriteLine(ex);
                    
                    return null;
                }

            }
        }
    

        private int RetrieveId(string email)
        {
           
            using (var connection = new SqlConnection(AccountsUsersConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;


                try
                {
                    command.CommandText = "Select accountId from @tableName where email = @email";
                    command.Parameters.AddWithValue("@tableName", AccountTable);
                    command.Parameters.AddWithValue("@email", email);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.GetInt32(0);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Failed to Retrieve Id");
                    Console.WriteLine(ex.ToString());
                    return -1;
                }

            }
        }
    }
}
