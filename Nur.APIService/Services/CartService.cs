using System.Net.Http.Json;
using Nur.APIService.Interfaces;
using Nur.APIService.Models.Carts;
using Microsoft.Extensions.Logging;
using Nur.APIService.Models.Response;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Nur.APIService.Services;

public class CartService(HttpClient client, ILogger<CartService> logger) : ICartService
{
    public async Task<CartDTO> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
    {
        using var response = await client.GetAsync($"get-by-user-id/{userId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var result = await response.Content.ReadFromJsonAsync<Response<CartDTO>>(options, cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }
}