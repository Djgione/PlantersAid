using System;
using System.Collections.Generic;
using System.Data;
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

        /// <summary>
        /// SQL Implementation for updating profile according to ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        public Result UpdateProfile(int id, Profile profile)
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
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                    transaction.Commit();

                }
                catch(Exception e)
                {
                    try
                    {
                        transaction.Rollback();
                        throw;
                    }
                    catch(Exception ex)
                    {
                        List<Exception> exceptions = new List<Exception>();
                        exceptions.Add(e);
                        exceptions.Add(ex);
                        throw new AggregateException(exceptions);
                    }
                }

            }

            result = new Result(true, "Profile successfully updated");
            return result;

        }

        /// <summary>
        /// SQL Implementation for updating profile picture according to ID, not idempotent, only overwrite
        /// To eliminate the Picture, upload a null byte array
        /// </summary>
        /// <param name="id"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public Result UpdateProfilePicture(int id, byte[] image)
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
                    command.CommandText = @"UPDATE " + ProfilePicTable.PROFILE_PIC_TABLE_NAME + @" SET " +
                        ProfilePicTable.PROFILE_PIC_COLUMN_NAME + @" = @byte WHERE " + ProfilePicTable.ACCOUNT_ID_COLUMN_NAME + @" = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.Add("@byte", SqlDbType.VarBinary).Value = image;

                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch(Exception e)
                {
                    try
                    {
                        transaction.Rollback();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        List<Exception> exceptions = new List<Exception>();
                        exceptions.Add(e);
                        exceptions.Add(ex);
                        throw new AggregateException(exceptions);
                    }
                }
            }


            result = new Result(true, "Profile picture successfully updated");
            return result;
        }


        /// <summary>
        /// SQL Implementation for retrieving information and compiling into the profile object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Profile RetrieveProfile(int id)
        {
            Profile profile;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();

                command.Connection = connection;

                try
                {
                    command.CommandText = @"SELECT " + UserTable.USERNAME_COLUMN_NAME + @", " + UserTable.FIRSTNAME_COLUMN_NAME + @", " + UserTable.LASTNAME_COLUMN_NAME + @", " + UserTable.GENDER_COLUMN_NAME +
                        @", " + UserTable.DATEOFBIRTH_COLUMN_NAME + @" FROM " + UserTable.USER_TABLE_NAME + @" WHERE " + UserTable.ACCOUNTID_COLUMN_NAME + @" = @id";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                //Order: Username, Firstname, Lastname, Gender, DateofBirth
                                profile = new Profile(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4));
                                return profile;
                            }

                        }
                        else
                        {
                            throw new Exception("No Profile Found");
                        }
                    }
                    throw new Exception("Reader not Created Properly");
                }
                catch (Exception)
                {
                    throw;
                }

            }

        }

        /// <summary>
        /// SQL Implementation for Retrieving Profile Picture
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public byte[] RetrieveProfilePicture(int id)
        {
            byte[] bytes;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();

                command.Connection = connection;

                try
                {
                    command.CommandText = @"SELECT " + ProfilePicTable.PROFILE_PIC_COLUMN_NAME + @" FROM " + ProfilePicTable.PROFILE_PIC_TABLE_NAME + 
                        @" WHERE " + ProfilePicTable.ACCOUNT_ID_COLUMN_NAME + @" = @id";
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                bytes = reader[ProfilePicTable.PROFILE_PIC_COLUMN_NAME] as byte[];
                                return bytes;
                            }
                        }
                        else
                        {
                            throw new Exception("No Profile Picture Found");
                        }
                    }
                    throw new Exception("Reader not Created Properly");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        
    }
}
