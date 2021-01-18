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
            throw new NotImplementedException();
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
            //SELECT EXISTS (SELECT* FROM login_details WHERE username = ? AND password = ?)
            throw new NotImplementedException();
        }

        /// <summary>
        /// SQL Method for Creating Account
        /// </summary>
        /// <returns></returns>
        public Result CreateAccount(Account acc, byte[] salt)
        {
            throw new NotImplementedException();
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
                SqlTransaction transaction;

                transaction = connection.BeginTransaction();

                command.Connection = connection;
                command.Transaction = transaction;

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
                    transaction.Rollback();
                    Console.WriteLine("Failed to Retrieve Salt");
                    Console.WriteLine(ex.ToString());
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
                SqlTransaction transaction;

                transaction = connection.BeginTransaction();

                command.Connection = connection;
                command.Transaction = transaction;

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
                    transaction.Rollback();
                    Console.WriteLine("Failed to Retrieve Id");
                    Console.WriteLine(ex.ToString());
                    return -1;
                }

            }
        }
    }
}
