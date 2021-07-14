using PlantersAid.DataAccessLayer.Interfaces;
using PlantersAid.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace PlantersAid.ServiceLayer
{
    public class AccountService
    {
        
        public IAccountDAO DataAccess { get; }
        public AccountService (IAccountDAO dataAccess)
        {
            DataAccess = dataAccess;
        }

        public Result DeleteAccount(String email)
        {
            try
            {
                int id = DataAccess.RetrieveId(email);

                var result = DataAccess.DeleteAccount(id);

                if (result.Success)
                {
                    Logger.Log("Account Deleted Successfully");
                }
                else
                {
                    Logger.Log("Account failed to Delete");
                }
                return result;
            }
            catch(Exception)
            {
                Logger.Log("Account failed to Delete");
                return new Result(false, "Account Failed to Delete");
            }

        }

        /// <summary>
        /// Attempt to Create Account with IAccountDAO
        /// Creates Salt and Hashes with HMACSHA512
        /// 
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public Result CreateAccount(Account acc)
        {
            Result result = CheckPasswordRequirements(acc.Password);
            //If Password does not Meet Requirements
            if(!result.Success)
            {
                Logger.Log("Password Did Not Meet Requirements");
                return result;
            }
                
            

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            acc.Password = Hasher(acc.Password, salt);

            Logger.Log("Password Succesfully Hashed, Attempting Store");

            var finalResult = DataAccess.CreateAccount(acc, salt);

            if(finalResult.Success)
            {
                Logger.Log("Account Successfully Created");
            }
            else
            {
                Logger.Log("Account Failed to Create");
            }

            return finalResult;
        }
 
        /// <summary>
        /// The Method used to change the password of an account 
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public Result ChangePassword(Account acc)
        {
            Result result = CheckPasswordRequirements(acc.Password);

            if (!result.Success)
            {
                Logger.Log(result.Message);
                return result;
            }
                

            byte[] salt = DataAccess.RetrieveSalt(acc.Email);
            if (salt is null)
            {
                var badResult = new Result(false, "Account does not exist.");
                Logger.Log(badResult.Message);
                return badResult;
            }
                

            acc.Password = Hasher(acc.Password, salt);

            var goodResult = DataAccess.ChangePassword(acc);
            Logger.Log(goodResult.Message);
            return goodResult;
        }

        private static string Hasher(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

        }


        /// <summary>
        /// Puts Password through set of Requirements to make sure it holds to standard
        /// </summary>
        /// <param name="password"></param>
        /// <returns>True if Password meets requirements, false if not with message on which failed</returns>
        public static Result CheckPasswordRequirements(string password)
        {
            Result result;

            //Password Length Check
            if (password.Length < 8)
            {
                result = new Result(false, "Password does not meet length requirements");
                return result;
            }

            Match match = Regex.Match(password, "[a-z]");
            //Password Lowercase Letter Check
            if (match.Success == false)
            {
                result = new Result(false, "Password does not contain a lowercase letter");
                return result;
            }

            match = Regex.Match(password, "[A-Z]");
            //Password Uppercase Letter Check
            if (match.Success == false)
            {
                result = new Result(false, "Password does not contain an uppercase letter");
                return result;
            }

            match = Regex.Match(password, "[0-9]");
            //Password Digit Check
            if(match.Success == false)
            {
                result = new Result(false, "Password does not contain a digit");
                return result;
            }

            result = new Result(true, "Password met all Requirements");
            return result;

        }

        public IEnumerable<Permission> RetrievePermissions(int accountId)
        {

        }




        public int RetrieveId(string email)
        {
            return DataAccess.RetrieveId(email);
        }
    }
}
