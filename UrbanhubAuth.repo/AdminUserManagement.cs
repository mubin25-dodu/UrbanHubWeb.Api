using System.Drawing;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using UrbanHub.Data;
using UrbanHub.DTO;
using UrbanHub.Entities;
using UrbanHub.shared;
using System.Text.Json;
using Microsoft.Data.SqlClient.DataClassification;

namespace UrbanHubManagement.repo
{
    public class  AdminUserManagement(UrbanHubDbContext context , UserCard userCard)
    {
        public async Task<Result<List<User>>> Get(string searchTerm)
        {
            var result = new Result<List<User>>();
            try
            {
                var getusers = new List<User>();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    getusers = await context.Users.Where(u => u.Uid != userCard.UserId && (u.Name.Contains(searchTerm) || u.Email.Contains(searchTerm))).ToListAsync();
                    //getusers = await context.Users.Where(u => u.Name.Contains(searchTerm) || u.Email.Contains(searchTerm)).ToListAsync();
                }
                else
                {
                    getusers = await context.Users.Where(u => u.Uid != userCard.UserId ).ToListAsync();
                }
                if (getusers == null)
                {
                    result.Error = false;
                    result.Message = "No Users Found";
                    return result;
                }
                result.Data = getusers;
                result.Error = true;
                result.Message = $"Total {getusers.Count} Users retrieved successfully.";

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Data = null;
                result.Message = "An error occurred while retrieving parking spaces.";
                result.Error = false;
                throw;
            }
            return result;
        }
        public async Task<Result<User>> BanUnbanUser(int id)
        {
            var result = new Result<User>();
            try
            {
                var getusers = await context.Users.FindAsync(id);
                if (getusers == null)
                {
                    result.Error = false;
                    result.Message = "No Users Found";
                    return result;
                }

                var logupdate = new Log();
                if (getusers.Status.ToLower() == "banned")
                {
                    getusers.Status = "Active";
                    logupdate.Message = $"User Unbanned successfully.By {userCard.UserId}";
                    result.Message = $"User Unbanned successfully.";

                }
                else
                {
                    getusers.Status = "Banned";
                    logupdate.Message = $"User Banned successfully.By {userCard.UserId}";
                    result.Message = $"User banned successfully.";
                }


                logupdate.UpdatedAt = DateTime.Now;
                logupdate.UpdatedBy = userCard.UserId;

                context.Logs.Add(logupdate);
                context.Users.Update(getusers);
                await context.SaveChangesAsync();

                result.Data = getusers;
                result.Error = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Data = null;
                result.Message = "An error occurred while retrieving parking spaces.";
                result.Error = false;
                throw;
            }
            return result;
        }
       
       
    }
}
