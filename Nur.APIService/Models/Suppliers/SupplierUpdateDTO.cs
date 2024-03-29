﻿using Microsoft.AspNetCore.Http;

namespace Nur.APIService.Models.Suppliers;

public class SupplierUpdateDTO
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public long VehicleId { get; set; }
    public IFormFile Attachment { get; set; }
}
