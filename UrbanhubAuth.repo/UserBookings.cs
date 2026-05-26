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
    public class UserBookings(UrbanHubDbContext context , UserCard userCard)
    {
        public Result<List<ParkingBooking>> GetAll()
        {
            var result = new Result<List<ParkingBooking>>();
            try
            {
                var Bookings = context.ParkingBookings.Where(p => p.RenterID == userCard.UserId )
                    .Include(p => p.Parking).OrderByDescending(p => p.Date).ToList();
                if(Bookings == null || Bookings.Count == 0)
                {
                    result.Data = null;
                    result.Message = "No bookings found.";
                    result.Error = false;
                    return result;
                }
                

                result.Data = Bookings;
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
        public Result<ParkingBooking> CancelBooking( int id)
        {
            var result = new Result<ParkingBooking>();
            try
            {
                var cancel = context.ParkingBookings.Find(id);
                if(cancel == null)
                {
                    result.Data = null;
                    result.Message = "No bookings found.";
                    result.Error = false;
                    return result;
                }
                cancel.Status ="Cancelled";
                context.SaveChanges();

                result.Data = null;
                result.Message = "Booking cancelled successfully.";
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
