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

            var passwordhasher = new PasswordHasher<User>();

            User user = new User();

            var result = passwordhasher.VerifyHashedPassword(
                user,
                pass,
                match
                );


            if (result == PasswordVerificationResult.Success)
            { 
            return true;
            }    

            return false;

        }


    }
}
