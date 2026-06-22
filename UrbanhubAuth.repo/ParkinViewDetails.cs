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
    public class ParkinViewDetails(UrbanHubDbContext context , IMapper mapper , UserCard card)
    {
        public async Task<Result<ParkingDetailsModel>> GetParkingSpace(int id)
        {
            var result = new Result<ParkingDetailsModel>();
            try
            {
                //getting parking infos
                var parkingSpace =  context.ParkingSpaces.Find(id);
                if (parkingSpace == null)
                {
                    result.Data = null;
                    result.Message = "Parking space not found.";
                    result.Error = true;
                    return result;
                }

                //getting booking infos

                var bookings = await context.ParkingBookings.Where(e => e.ParkingID == id 
                                                                        && (e.Status == "Booked" )).ToListAsync();
                if (bookings == null || bookings.Count == 0)
                {
                    result.Message = "No Bookings Found";
                }

                //the bug i always face ("list of" bookings and parking space)

                result.Data = new ParkingDetailsModel()
                {
                    ParkingSpaces = mapper.Map<ParkingSpaceDTO>(parkingSpace),
                    ParkingBookings = bookings ?? new List<ParkingBooking>()
                };
                result.Error = false;
                result.Message = "Details and Booking retrieved ";


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Data = null;
                result.Message = "An error occurred while retrieving parking spaces.";
                result.Error = true;
                throw;
            }
            return result;
        }

        public async Task<Result<ParkingBookingDTO>> RequestBooking(ParkingBookingDTO data )
        {
            var result = new Result<ParkingBookingDTO>();

            try
            {
                var owner = context.ParkingSpaces.Where(e =>e.ID ==data.ParkingID && e.OwnerId == card.UserId);
                if (owner.Any())
                {
                    result.Data = null;
                    result.Message = "You cannot request a booking for your own parking space.";
                    result.Error = true;
                    return result;
                }
                //if already send a request
                var check = context.ParkingBookings.Any(e => e.ParkingID == data.ParkingID && e.RenterID == card.UserId
                && e.Status.ToLower()=="pending");
                if (check)
                {
                    result.Data = null;
                    result.Message = "You have already requested a booking for this parking space. " +
                                     "Cancel it first or wait for owner's response.";
                    result.Error = true;
                    return result;
                }

                //if the time slot is already booked in backend check... 
                //infrontend checking is not enough maybe another user is looking for this same spot same time 
                // faster internet owala winns  
                var existingBookings =await context.ParkingBookings
                    .Where(b => b.ParkingID == data.ParkingID
                                && b.Status == "Booked"
                                && b.StartingTime < data.EndingTime
                                && b.EndingTime > data.StartingTime)
                    .ToListAsync();
                if (existingBookings.Count > 0)
                {
                    result.Data = data;
                    result.Error = true;
                    result.Message = "This time slot conflicts with an existing booking.";
                    return result;
                }

                var startday = data.StartingTime.DayOfWeek.ToString();
                var endday = data.EndingTime.DayOfWeek.ToString();
                var starttime = TimeOnly.FromDateTime(data.StartingTime);
                var endtime = TimeOnly.FromDateTime(data.EndingTime);

                //not checking end date its on the user if he wants to 
                //share his parking space for 1 day or 1 month or 1 year (and yes i'm lazy)

                var schedule = context.ParkingSpaces.Where(e=> e.IsAvailable== true && e.ID == data.ParkingID).ToListAsync();

                var checkavailable =  JsonSerializer.Deserialize<List<AvailabeSchadule>>((await schedule)[0].Available);
                if (!checkavailable.Any())
                {
                    result.Data = null;
                    result.Message = "Slot Not available";
                    result.Error = true;
                    return result;
                }


                TimeSpan amount = data.EndingTime-data.StartingTime;
                double hours = amount.TotalHours;

                var save = new ParkingBooking();
                save.ParkingID = data.ParkingID;
                save.OwnerID = data.OwnerID;
                save.StartingTime = data.StartingTime;
                save.EndingTime = data.EndingTime;
                save.Status = "Pending";
                save.RenterID = card.UserId;
                save.PaymentAmount= decimal.Parse((hours * (double)data.RentPerHour).ToString());
                save.PaymentStatus = "Pending";
                save.Date = DateTime.Now;
                context.ParkingBookings.Add(save);
                context.SaveChanges();
                
                result.Data = null;
                result.Error = false;
                result.Message = "Booking Request Send Successfully";

            }
            catch (Exception e)
            {
                result.Data = data;
                result.Message = "something went wrong";
                result.Error = true;
                throw;
            }
            return result;
        }
    }
   
}
