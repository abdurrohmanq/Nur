﻿using Nur.Domain.Enums;
using Nur.Application.UseCases.Users.DTOs;
using Nur.Application.UseCases.Addresses.DTOs;
using Nur.Application.UseCases.Suppliers.DTOs;
using Nur.Application.UseCases.Payments.DTOs;
using Nur.Application.UseCases.Orders.OrderItems.DTOs;

namespace Nur.Application.UseCases.Orders.DTOs;

public class OrderDTO
{
    public long Id { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public Status Status { get; set; }
    public decimal? DeliveryFee { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderType OrderType { get; set; }
    public string Description { get; set; }

    public UserDTO User { get; set; }
    public AddressDTO Address { get; set; }
    public SupplierDTO Supplier { get; set; }
    public PaymentDTO Payment { get; set; }

    public ICollection<OrderItemDTO> OrderItems { get; set; }
}
