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
    public class PlatformServices(UrbanHubDbContext context , UserCard userCard)
    {
        public async Task<Result<List<Services>>> Get()
        {
            var result = new Result<List<Services>>();
            try
            {
                var Get = await context.Services.ToListAsync();

                if (Get == null)
                {
                    result.Error = true;
                    result.Message = "No services Found";
                    return result;
                }
                result.Data = Get;
                result.Error = false;
                result.Message = $"Total {Get.Count} Services retrieved successfully.";

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
