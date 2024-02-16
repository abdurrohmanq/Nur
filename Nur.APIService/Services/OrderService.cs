using System.Net.Http.Json;
using Nur.APIService.Helpers;
using Nur.APIService.Interfaces;
using Microsoft.Extensions.Logging;
using Nur.APIService.Models.Orders;
using Nur.APIService.Models.Response;

namespace Nur.APIService.Services;

public class OrderService(HttpClient httpClient, ILogger<OrderService> logger) : IOrderService
{
    public async Task<OrderResultDTO> AddAsync(OrderCreationDTO dto, CancellationToken cancellationToken)
    {
        using var multipartFormContent = ConvertHelper.ConvertToMultipartFormContent(dto);
        using var response = await httpClient.PostAsync("create", multipartFormContent);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<OrderResultDTO>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<OrderResultDTO> UpdateAsync(OrderUpdateDTO dto, CancellationToken cancellationToken)
    {
        using var multipartFormContent = ConvertHelper.ConvertToMultipartFormContent(dto);
        using var response = await httpClient.PutAsync("update", multipartFormContent);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<OrderResultDTO>>(cancellationToken: cancellationToken);
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

    public async Task<OrderResultDTO> GetAsync(long id, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync($"get/{id}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<OrderResultDTO>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<IEnumerable<OrderResultDTO>> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync($"get-by-user-id/{userId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<IEnumerable<OrderResultDTO>>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<IEnumerable<OrderResultDTO>> GetBySupplierIdAsync(long supplierId, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync($"get-by-supplier-id/{supplierId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<IEnumerable<OrderResultDTO>>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<IEnumerable<OrderResultDTO>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync("get-all", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<IEnumerable<OrderResultDTO>>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }
}
