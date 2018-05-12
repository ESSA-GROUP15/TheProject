using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoStore.WebClient.CustomAuth
{
    public class CustomHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return Common.Cryptography.sha512encrypt(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var genHash = HashPassword(providedPassword);
            if(hashedPassword == genHash)
            {
                return PasswordVerificationResult.Success;
            }
            else{
                return PasswordVerificationResult.Failed;
            }
        }
    }
}