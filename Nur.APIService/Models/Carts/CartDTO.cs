using Nur.APIService.Models.CartItems;
using Nur.APIService.Models.Users;

namespace Nur.APIService.Models.Carts;

public class CartDTO
{
    public long Id { get; set; }
    public decimal TotalPrice { get; set; }
    public UserDTO User { get; set; }

    public ICollection<CartItemResultDTO> CartItems { get; set; }
}
