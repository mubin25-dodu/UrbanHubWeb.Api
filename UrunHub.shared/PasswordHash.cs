using AutoMapper;
using UrbanHub.DTO;
using UrbanHub.Entities;
using Microsoft.AspNetCore.Identity;

namespace UrbanHub.shared
{
    public class PasswordHash 
    {
        public string Hash(string pass) {

            var passwordhasher = new PasswordHasher<User>();
            
            User user = new User();

            string HashedPassword = passwordhasher.HashPassword(user, pass);

            return HashedPassword;
        
        }

        public bool MatchHash(string pass , string match)
        {
            if (string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(match))
            {
                return false;
            }

            var passwordhasher = new PasswordHasher<User>();

            User user = new User();

            try
            {
                var result = passwordhasher.VerifyHashedPassword(
                    user,
                    pass,
                    match
                    );

                return result == PasswordVerificationResult.Success;
            }
            catch (FormatException)
            {
                return false;
            }


        }


    }
}
