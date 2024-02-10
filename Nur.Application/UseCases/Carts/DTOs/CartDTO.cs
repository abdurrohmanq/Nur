using Nur.Application.UseCases.Users.DTOs;

namespace Nur.Application.UseCases.Carts.DTOs;

public class CartDTO
{
    public long Id { get; set; }
    public decimal TotalPrice { get; set; }
    public UserDTO User { get; set; }
}
