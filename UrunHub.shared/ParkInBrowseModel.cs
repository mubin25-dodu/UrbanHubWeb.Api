using UrbanHub.DTO;

namespace UrbanHub.shared;

public class ParkInBrowseModel
{
    public SearchParkingSpace? SearchSpaces { get; set; }
    public List<ParkingSpaceDTO>? ParkingSpaces { get; set; } = new();
    public int CurrentPage { get; set; } = 0;
    public int TotalResults { get; set; } = 0;
}