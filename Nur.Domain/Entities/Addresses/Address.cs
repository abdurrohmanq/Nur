using Nur.Domain.Commons;

namespace Nur.Domain.Entities.Addresses;

public class Address: Auditable
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; } = "Uzbekistan";
    public string DoorCode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
