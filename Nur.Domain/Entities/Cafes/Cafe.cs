using Nur.Domain.Commons;

namespace Nur.Domain.Entities.Cafes;

public class Cafe : Auditable
{
    public string InstagramLink { get; set; }
    public string FacebookLink { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
}
