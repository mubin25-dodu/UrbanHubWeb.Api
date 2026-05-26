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
    public class Notifications(UrbanHubDbContext context , UserCard userCard)
    {
        public Result<List<Notification>> GetAll()
        {
            var result = new Result<List<Notification>>();
            try
            {
                var notifications = context.Notifications.Where(p => p.To == userCard.UserId).
                    OrderByDescending(p => p.Date)
                    .ToList();
                if(notifications == null || notifications.Count == 0)
                {
                    result.Data = null;
                    result.Message = "No Notifications found.";
                    result.Error = false;
                    return result;
                }
                
                result.Data = notifications;
                result.Message = "Notifications retrieved successfully.";
                result.Error = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Data = null;
                result.Message = "An error occurred while retrieving Bookings.";
                result.Error = false;
                throw;
            }

            return result;
        }
        
        public Result<List<Notification>> MarkAsSeenResult(int id)
        {
            var result = new Result<List<Notification>>();
            try
            {
                var notifications = context.Notifications.Where(p => p.To == userCard.UserId).
                    OrderByDescending(p => p.Date)
                    .ToList();
                if(notifications == null || notifications.Count == 0)
                {
                    result.Data = null;
                    result.Message = "No Notifications found.";
                    result.Error = false;
                    return result;
                }
                var mark = notifications.Find(n => n.ID == id);

                if (mark == null)
                {
                    result.Data = null;
                    result.Message = "Notification not found.";
                    result.Error = false;
                    return result;
                }

                mark.Seen= true;
                context.Notifications.Update(mark);
                context.SaveChanges();

                result.Data = notifications;
                result.Message = "Notifications retrieved successfully.";
                result.Error = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Data = null;
                result.Message = "An error occurred while retrieving Bookings.";
                result.Error = false;
                throw;
            }

            return result;
        }
        
    }
}
