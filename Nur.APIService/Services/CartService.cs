using System.Net.Http.Json;
using Nur.APIService.Interfaces;
using Nur.APIService.Models.Carts;
using Microsoft.Extensions.Logging;
using Nur.APIService.Models.Response;

namespace Nur.APIService.Services;

public class CartService(HttpClient client, ILogger<CartService> logger) : ICartService
{
    public async Task<CartDTO> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
    {
        using var response = await client.GetAsync($"get/{userId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<CartDTO>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }
}