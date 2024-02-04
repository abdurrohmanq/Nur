using Nur.Domain.Commons;
using Nur.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nur.Domain.Entities.Carts;

public class Cart : Auditable
{
    public decimal TotalPrice { get; set; }
    public long? UserId { get; set; }
    public User User { get; set; }
}
