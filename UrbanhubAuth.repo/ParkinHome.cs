using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Drawing;
using System.Text.Json;
using UrbanHub.Data;
using UrbanHub.DTO;
using UrbanHub.Entities;
using UrbanHub.shared;

namespace UrbanHubManagement.repo
{
    public class ParkinHome(UrbanHubDbContext context, IMapper mapper)
    {
        public async Task<Result<ParkInBrowseModel>> GetAllParkingSpaces( int page , int offset)
        {
            var result = new Result<ParkInBrowseModel>();
            try
            {
                page = page <= 0 ? 1 : page;
                offset = offset <= 0 ? 21 : offset;
                var Available = await context.ParkingSpaces.Where(e => e.IsAvailable == true).CountAsync();
                var parkingSpaces = await context.ParkingSpaces
                    .Where(p => p.IsAvailable == true)
                    .Skip((page - 1)* offset).Take(offset).ToListAsync();

                var mappedSpaces = mapper.Map<List<ParkingSpaceDTO>>(parkingSpaces);


                result.Data = new ParkInBrowseModel
                {
                    ParkingSpaces = mappedSpaces,
                    TotalResults = Available,
                    CurrentPage = page
                };
                result.Message = "Parking spaces retrieved successfully.";
                result.Error = false;
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
        public async Task<Result<ParkInBrowseModel>> NearBy(double distance, double lat, double lon , int page , int offset)
        {
            var result = new Result<ParkInBrowseModel>();
            var currentLocation = new NetTopologySuite.Geometries.Point(lon, lat) { SRID = 4326 };
            try
            {
                page = page <= 0 ? 1:page;
                offset = offset <= 0 ? 21 : offset;

                var nearby = await context.ParkingSpaces
                    .Where(c => c.Location.Distance(currentLocation) <= distance && c.IsAvailable == true)
                    .ToListAsync();

                var filtered = nearby
                    .Where(c => c.Location != null && new HeversineFormula().GetDistanceKm(lat, lon, c.Location.Y, c.Location.X) <= distance)
                    .OrderBy(c => new HeversineFormula().GetDistanceKm(lat, lon, c.Location!.Y, c.Location!.X)).Skip((page - 1)*offset).Take(offset)
                    .ToList();


                if (filtered == null)
                {
                    result.Message = "No parking space found";
                    result.Data = null;
                    result.Error = false;
                    return result;
                }

                var newdata = mapper.Map<List<ParkingSpaceDTO>>(filtered);

                //passing distance to DTO
                foreach (var i in newdata)
                {
                    var space = filtered.FirstOrDefault(p => p.ID == i.ID);
                    i.Distance = Math.Round(new HeversineFormula().GetDistanceKm(lat, lon, space!.Location!.Y, space.Location!.X), 1);
                }

                result.Data = new ParkInBrowseModel
                {
                    ParkingSpaces = newdata,
                    TotalResults = newdata.Count
                };
                result.Message = "Parking spaces retrieved successfully.";
                result.Error = false;
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

        public async Task<Result<ParkInBrowseModel>> Search(SearchParkingSpace data , int page , int offset)
        {
            var result = new Result<ParkInBrowseModel>();
            try
            {
                page = page <= 0 ? 1 : page;
                offset = offset <= 0 ? 21 : offset;
                var Available = await context.ParkingSpaces.Where(e => e.IsAvailable == true && e.VehicleType == data.Type && e.IsAvailable == true &&
                                e.Address.Contains(data.SearchText)).CountAsync();
                var nearby = await context.ParkingSpaces
                    .Where(c => c.VehicleType == data.Type && c.IsAvailable == true &&
                                c.Address.Contains(data.SearchText))
                    .ToListAsync();


                //day 4 of trying

                if (data.DateAndTime != default || data.DateAndTime != null)
                {
                    //okay so lets catch the time and then lets try to filter out the extended data by matching the times and the day
                    var targetDay = data.DateAndTime?.DayOfWeek;
                    //found the bug heheheeey..... it was comparing the time with
                    //the date and time so i just need to extract the time
                    //from the date and time and then compare it with the start and end time in the schedule
                    var targetTime = TimeOnly.FromDateTime(data.DateAndTime.Value);
                    var filteredSpaces = nearby
                        .Where(p =>
                        {

                            var schedules = JsonSerializer.Deserialize<List<AvailabeSchadule>>(p.Available);

                            // validating day and time 
                            return schedules.Any(e =>
                                e.Day == targetDay.ToString() &&
                                e.StartTime <= targetTime &&
                                e.EndTime >= targetTime);
                        }).Skip((page-1)*offset).Take(offset)
                        .ToList();

                    //lets check if the time slot is available in that time 
                    //aaaaahhh logic is hard but ill be there soon i guess

                    //var Booking = await context.ParkingBookings
                    //    .Where(c => filteredSpaces.Any(f => f.ID == c.ID
                    //    &&
                    //    c.Status == "Booked") && (
                    //         JsonSerializer.Deserialize<List<AvailabeSchadule>>(p.Available)

                    //        ))
                    //    .ToListAsync();

                    var mappedSpaces = mapper.Map<List<ParkingSpaceDTO>>(filteredSpaces);
                    result.Data = new ParkInBrowseModel
                    {
                        ParkingSpaces = mappedSpaces,
                        TotalResults = Available,
                        SearchSpaces = data

                    };
                    result.Message = "Parking spaces retrieved successfully.";
                    result.Error = true;
                }
                else
                {
                    var mappedSpaces = mapper.Map<List<ParkingSpaceDTO>>(nearby);
                    result.Data = new ParkInBrowseModel
                    {
                        ParkingSpaces = mappedSpaces,
                        TotalResults = Available,
                        SearchSpaces = data
                    };
                    result.Message = "Parking spaces retrieved successfully.";
                    result.Error = true;

                }
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

        // Haversine formula. km distance

    }

}
