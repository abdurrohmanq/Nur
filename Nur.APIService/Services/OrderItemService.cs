using System.Net.Http.Json;
using Nur.APIService.Helpers;
using Nur.APIService.Interfaces;
using Microsoft.Extensions.Logging;
using Nur.APIService.Models.Response;
using Nur.APIService.Models.OrderItems;

namespace Nur.APIService.Services;

public class OrderItemService(HttpClient httpClient, ILogger<OrderItemService> logger) : IOrderItemService
{
    public async Task<OrderItemResultDTO> AddAsync(OrderItemCreationDTO dto, CancellationToken cancellationToken)
    {
        using var multipartFormContent = ConvertHelper.ConvertToMultipartFormContent(dto);
        using var response = await httpClient.PostAsync("create", multipartFormContent);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<OrderItemResultDTO>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<OrderItemResultDTO> UpdateAsync(OrderItemUpdateDTO dto, CancellationToken cancellationToken)
    {
        using var multipartFormContent = ConvertHelper.ConvertToMultipartFormContent(dto);
        using var response = await httpClient.PutAsync("update", multipartFormContent);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<OrderItemResultDTO>>(cancellationToken: cancellationToken);
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

    public async Task<OrderItemResultDTO> GetAsync(long id, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync($"get/{id}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<OrderItemResultDTO>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<IEnumerable<OrderItemResultDTO>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync("get-all", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<IEnumerable<OrderItemResultDTO>>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<IEnumerable<OrderItemResultDTO>> GetByOrderIdAsync(long orderId, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync($"get-by-order-id/{orderId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<IEnumerable<OrderItemResultDTO>>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<IEnumerable<OrderItemResultDTO>> GetByProductIdAsync(long supplierId, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync($"get-by-supplier-id/{supplierId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<IEnumerable<OrderItemResultDTO>>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }
}
