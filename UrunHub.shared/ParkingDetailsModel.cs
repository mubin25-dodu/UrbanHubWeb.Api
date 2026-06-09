using UrbanHub.DTO;
using UrbanHub.Entities;

namespace UrbanHub.shared;

public class ParkingDetailsModel
{
    public ParkingSpaceDTO? ParkingSpaces { get; set; } 
    public List<ParkingBooking>? ParkingBookings { get; set; }
    public ParkingBooking? ParkingBooking { get; set; }
    public ParkingBookingDTO ? ParkingBookingDTO { get; set; }
    public decimal? Platformfee { get; set; }
    public decimal? TotalBill { get; set; }
}