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
    public class Payment(UrbanHubDbContext context , UserCard userCard , IMapper mapper)
    {
        public Result<ParkingDetailsModel> Getbooking(int id)
        {
            var result = new Result<ParkingDetailsModel>();
            try
            {
                var bookings = context.ParkingBookings.Include(p => p.Parking).
                        FirstOrDefault(p => p.ID == id)
                    ;
                if (bookings == null)
                {
                    result.Error = false;
                    result.Message = "No Bookings Found";
                    return result;
                }

                var fee = context.PlatformWallets.Select(w => w.PlatformFee).FirstOrDefault()
                          * bookings.PaymentAmount;
                var newdata = new ParkingDetailsModel()
                {
                    ParkingSpaces = mapper.Map<ParkingSpaceDTO>(bookings.Parking),
                    ParkingBooking = bookings,
                    Platformfee = fee,
                    TotalBill = (fee + bookings.PaymentAmount)
                };

                result.Data = newdata;
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
        public Result<ParkingDetailsModel> ProcessPayment(int id)
        {
            var result = new Result<ParkingDetailsModel>();
            try
            {
                var bookings = context.ParkingBookings.Include(
                    p => p.Parking).FirstOrDefault(p=>p.ID ==id);
                if (bookings == null)
                {
                    result.Error = false;
                    result.Message = "No Bookings Found";
                    return result;
                }
                var fee = context.PlatformWallets.Select(w => w.PlatformFee).FirstOrDefault()
                          * bookings.PaymentAmount;
                var OTP =  new Random().Next(100000, 1000000);

                bookings.OTP = OTP;
                bookings.PaymentStatus = "Paid";
                bookings.TotalBill = (fee + bookings.PaymentAmount);
                var addmoney = new PlatformWallet()
                {
                    UID = userCard.UserId,
                    AddMoney = (bookings.PaymentAmount+fee),
                };

                var notif = new Notification()
                {
                    To = bookings.RenterID,
                    Message = $"Your payment for parking booking {bookings.Parking.Address} has been processed. " +
                              $"Your OTP is {OTP}.Please provide this OTP to the Owner to complete the transaction.",
                    From = "UrbanHub",
                    Date = DateTime.Now
                };
                context.Notifications.Add(notif);
                context.PlatformWallets.Add(addmoney);
                context.ParkingBookings.Update(bookings);
                context.SaveChanges();
                result.Message = $"Your payment for parking booking {bookings.Parking.Address} has been processed. " +
                                 $"Your OTP is {OTP}.Please provide this OTP to the Owner to complete the transaction.";
                result.Data = null;
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
