namespace Nur.APIService.Models.Addresses;

public class AddressDTO
{
    public long Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string DoorCode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
