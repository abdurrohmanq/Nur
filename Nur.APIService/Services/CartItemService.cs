using System.Net.Http.Json;
using Nur.APIService.Helpers;
using Nur.APIService.Interfaces;
using Microsoft.Extensions.Logging;
using Nur.APIService.Models.CartItems;
using Nur.APIService.Models.Response;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Nur.APIService.Services;

public class CartItemService(HttpClient httpClient, ILogger<CartItemService> logger) : ICartItemService
{
    public async Task<CartItemResultDTO> AddAsync(CartItemCreationDTO dto, CancellationToken cancellationToken)
    {
        using var multipartFormContent = ConvertHelper.ConvertToMultipartFormContent(dto);
        using var response = await httpClient.PostAsync("create", multipartFormContent);
        if (!response.IsSuccessStatusCode)
            return default!;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var result = await response.Content.ReadFromJsonAsync<Response<CartItemResultDTO>>(options, cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<CartItemResultDTO> UpdateAsync(CartItemUpdateDTO dto, CancellationToken cancellationToken)
    {
        using var multipartFormContent = ConvertHelper.ConvertToMultipartFormContent(dto);
        using var response = await httpClient.PutAsync("update", multipartFormContent);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<CartItemResultDTO>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        using var response = await httpClient.DeleteAsync($"delete/{id}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<bool>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<bool> DeleteByProductNameAsync(string productName, CancellationToken cancellationToken)
    {
        using var response = await httpClient.DeleteAsync($"delete-by-product-name?productName={productName}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<bool>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<CartItemResultDTO> GetAsync(long id, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync($"get/{id}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<CartItemResultDTO>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<IEnumerable<CartItemResultDTO>> GetByCartIdAsync(long id, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync($"get-by-cart-id/{id}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var result = await response.Content.ReadFromJsonAsync<Response<IEnumerable<CartItemResultDTO>>>(options, cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<IEnumerable<CartItemResultDTO>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync("get-all", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var result = await response.Content.ReadFromJsonAsync<Response<IEnumerable<CartItemResultDTO>>>(options, cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }
}
