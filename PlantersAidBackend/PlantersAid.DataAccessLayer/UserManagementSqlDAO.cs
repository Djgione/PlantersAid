using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using PlantersAid.DataAccessLayer.Interfaces;
using PlantersAid.DataAccessLayer.SQLTableColumns;
using PlantersAid.Models;

namespace PlantersAid.DataAccessLayer
{
    public class UserManagementSqlDAO : IUserManagementDAO
    {
        private readonly string _connectionString;

        public UserManagementSqlDAO()
        {
            _connectionString = Environment.GetEnvironmentVariable("plantersAidAccountUsersConnectionString");
        }

        public Result UpdateProfile(Profile profile)
        {
            Result result;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction = connection.BeginTransaction();

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = @"UPDATE " + UserTable.USER_TABLE_NAME + @" SET " + UserTable.USERNAME_COLUMN_NAME + @" = @Username, " + UserTable.FIRSTNAME_COLUMN_NAME + @" = @FirstName, " 
                        + UserTable.LASTNAME_COLUMN_NAME + @" = @LastName, " + UserTable.GENDER_COLUMN_NAME + @" = @Gender, " + UserTable.DATEOFBIRTH_COLUMN_NAME + @" = @DateOfBirth WHERE " 
                            + AccountTable.ACCOUNT_ID_COLUMN_NAME + @" = @Id";
                    command.Parameters.AddWithValue("@Username", profile.Username);
                    command.Parameters.AddWithValue("@FirstName", profile.FirstName);
                    command.Parameters.AddWithValue("@LastName", profile.LastName);
                    command.Parameters.AddWithValue("@Gender", profile.Gender);
                    command.Parameters.AddWithValue("@Id", profile.Id);

                    command.ExecuteNonQuery();
                    transaction.Commit();

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

            result = new Result(true, "Profile successfully updated");
            return result;

        }

        //public Result UpdateProfilePicture(int id, )
    }
}
