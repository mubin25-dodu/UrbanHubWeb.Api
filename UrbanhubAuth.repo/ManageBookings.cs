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
    public class ManageBookings(UrbanHubDbContext context , UserCard userCard)
    {
        public Result<List<ParkingBooking>> GetAll()
        {
            var result = new Result<List<ParkingBooking>>();
            try
            {
                var bookings = context.ParkingBookings.Where(p => p.OwnerID == userCard.UserId).
                    Include(p => p.Renter).Include(p => p.Parking)
                    .OrderByDescending(p => p.Date)
                    .ToList();
                if(bookings == null || bookings.Count == 0)
                {
                    result.Data = null;
                    result.Message = "No Parking Bookings found.";
                    result.Error = false;
                    return result;
                }
                

                result.Data = bookings ?? new List<ParkingBooking>() ;
                result.Message = "Parking Bookings retrieved successfully.";
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
        public Result<ParkingBooking> Accept( int id)
        {
            var result = new Result<ParkingBooking>();
            try
            {
                var spaces = context.ParkingBookings.Include(p=>p.Parking).FirstOrDefault(a=>a.ID == id);
                if (spaces == null)
                {
                    result.Data = null;
                    result.Message = "No Parking Space found.";
                    result.Error = false;
                    return result;
                }

                spaces.Status = "Accepted";

                
                var notification = new Notification()
                {
                    From = userCard.UserId.ToString(),
                    To = spaces.RenterID ,
                    Title = "Parking Booking Accepted",
                    Message = $"Your parking booking has been accepted! Pay the rent to confirm your parking space."  +
                              $"Address: {spaces.Parking.Address}. " +
                              $"Rent: {spaces.Parking.RentPerHour} BDT/hour.",
                    Date = DateTime.Now
                };

                context.Notifications.Add(notification);
                context.ParkingBookings.Update(spaces);
                context.SaveChanges();

                result.Data = null;
                result.Message = "Parking request accepted.";
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

        public Result<ParkingBooking> Cancel( int id)
        {
            var result = new Result<ParkingBooking>();
            try
            {
                var spaces = context.ParkingBookings.Include(p => p.Parking).FirstOrDefault(a => a.ID == id);
                if (spaces == null)
                {
                    result.Data = null;
                    result.Message = "No Parking Space found.";
                    result.Error = false;
                    return result;
                }
                spaces.Status = "Canceled";

                var notification = new Notification()
                {
                    From = userCard.UserId.ToString(),
                    To = spaces.RenterID ,
                    Message = $"Your parking booking request for {spaces.Parking.Address} has been declined." +
                              $" Please try booking another parking space. For assistance, contact support.",
                    Date = DateTime.Now
                };

                context.Notifications.Add(notification);
                context.ParkingBookings.Update(spaces);
                context.SaveChanges();

                result.Data = null;
                result.Message = "Parking request Canceled.";
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
        public Result<ParkingBooking> RequestPayment( ParkingBooking data)
        {
            var result = new Result<ParkingBooking>();
            try
            {
                var booking = context.ParkingBookings.Find(data.ID);
                if (booking == null)
                {
                    result.Data = null;
                    result.Message = "No Parking Space found.";
                    result.Error = false;
                    return result;
                }
                else if (booking.OTP != data.OTP)
                {
                    result.Data = null;
                    result.Message = "Invalid OTP.";
                    result.Error = false;
                    return result;
                }

                var wallet = new Wallet()
                {
                    UserID = userCard.UserId,
                    Amount = booking.PaymentAmount,
                    Date = DateTime.Now,
                    Status = true
                };

                var bookings= context.ParkingBookings.Find(data.ID);
                if(bookings != null)
                {
                    bookings.OTP = null;
                    bookings.Withdrawn= true;
                }

                var notification = new Notification()
                {
                    From = "UrbanHub",
                    To = userCard.UserId,
                    Message = $"Payment Received – Your payment has been successfully credited to your wallet. " +
                              $"You may withdraw your funds at any time.",
                    Date = DateTime.Now
                };

                context.Notifications.Add(notification);
                context.ParkingBookings.Update(bookings);
                context.Wallets.Add(wallet);
                context.SaveChanges();

                result.Data = null;
                result.Message = "🔔 Payment Received – Your payment has been successfully credited to your wallet.";
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
