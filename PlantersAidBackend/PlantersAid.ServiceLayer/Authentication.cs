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
    public class Authentication
    {
        
        public IAccountDAO DataAccess { get; }
        public Authentication (IAccountDAO dataAccess)
        {
            DataAccess = dataAccess;
        }

        public Result DeleteAccount(Account acc)
        {
            throw new NotImplementedException();
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
                return result;
            

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            acc.Password = Hasher(acc.Password, salt);

            return DataAccess.CreateAccount(acc, salt);
        }


        /// <summary>
        /// Attempts to Log in with Account credentials contained in Account acc
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public Result Login(Account acc)
        {
            byte [] salt = DataAccess.RetrieveSalt(acc.Email);
            if (salt is null)
                return new Result(false, "Account does not exist.");

            acc.Password = Hasher(acc.Password,salt);

            return DataAccess.Login(acc);
        }

        public Result ChangePassword(Account acc)
        {
            Result result = CheckPasswordRequirements(acc.Password);

            if(!result.Success)
                return result;

            return DataAccess.ChangePassword(acc);
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

    }
}
