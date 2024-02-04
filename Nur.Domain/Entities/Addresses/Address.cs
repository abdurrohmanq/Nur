using Nur.Domain.Commons;

namespace Nur.Domain.Entities.Addresses;

public class Address: Auditable
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; } // Viloyat nomi
    public string Country { get; set; }
    public string ZipCode { get; set; } // Pochta indeksi
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
