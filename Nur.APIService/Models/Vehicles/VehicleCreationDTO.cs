using Microsoft.AspNetCore.Http;

namespace Nur.APIService.Models.Vehicles;

public class VehicleCreationDTO
{
    public string Model { get; set; }
    public string Brand { get; set; }
    public string CarNumber { get; set; }
    public string Color { get; set; }
    public IFormFile Attachment { get; set; }
}
