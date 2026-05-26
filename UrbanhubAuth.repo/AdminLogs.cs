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
    public class  AdminLogs(UrbanHubDbContext context , UserCard userCard)
    {
        public async Task<Result<List<Log>>> Get()
        {
            var result = new Result<List<Log>>();
            try
            {
                var getlogs = await context.Logs.Include(l => l.User).ToListAsync();

                if (getlogs == null)
                {
                    result.Error = false;
                    result.Message = "No Logs Found";
                    return result;
                }
                result.Data = getlogs;
                result.Error = true;
                result.Message = $"Total {getlogs.Count} Logs retrieved successfully.";

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
