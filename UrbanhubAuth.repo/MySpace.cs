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

namespace UrbanHubManagement.repo
{
    public class MySpace(UrbanHubDbContext context , UserCard userCard)
    {
        public Result<List<ParkingSpace>> GetAll()
        {
            var result = new Result<List<ParkingSpace>>();
            try
            {
                //some lazy work ill update it letter exam ache
                var spaces = context.ParkingSpaces.Where(p => p.OwnerId == userCard.UserId
                && p.Available.ToLower() != "Removed by Owner")
                    .ToList();
                if(spaces == null || spaces.Count == 0)
                {
                    result.Data = null;
                    result.Message = "No Parking Space found.";
                    result.Error = false;
                    return result;
                }
                

                result.Data = spaces;
                result.Message = "";
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
        public Result<List<ParkingSpace>> Delete( int id)
        {
            var result = new Result<List<ParkingSpace>>();
            try
            {
                var spaces = context.ParkingSpaces.Find(id);
                if (spaces == null)
                {
                    result.Data = null;
                    result.Message = "No Parking Space found.";
                    result.Error = false;
                    return result;
                }

                spaces.IsAvailable = false;
                spaces.Available = "Removed by Owner";
                
                context.ParkingSpaces.Update(spaces);
                context.SaveChanges();

                result.Data = null;
                result.Message = "Parking space marked as removed.";
                result.Error = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Data = null;
                result.Message = "An error occurred while removing parking space.";
                result.Error = false;
                throw;
            }

            return result;
        }

    }
}
