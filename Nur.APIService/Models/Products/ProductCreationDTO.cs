using Microsoft.AspNetCore.Http;
using Nur.APIService.Models.Enums;

namespace Nur.APIService.Models.Products;

public class ProductCreationDTO
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public double? Quantity { get; set; }
    public string Description { get; set; }
    public Unit Unit { get; set; }
    public long CategoryId { get; set; }
    public IFormFile Attachment { get; set; }
}