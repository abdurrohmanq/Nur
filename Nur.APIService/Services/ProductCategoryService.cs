using System.Net.Http.Json;
using Nur.APIService.Helpers;
using Nur.APIService.Interfaces;
using Microsoft.Extensions.Logging;
using Nur.APIService.Models.Response;
using Nur.APIService.Models.ProductCategories;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Nur.APIService.Services;

public class ProductCategoryService(HttpClient httpClient, ILogger<ProductCategoryService> logger) : IProductCategoryService
{
    public async Task<ProductCategoryDTO> AddAsync(ProductCategoryCreationDTO dto, CancellationToken cancellationToken)
    {
        using var multipartFormContent = ConvertHelper.ConvertToMultipartFormContent(dto);
        using var response = await httpClient.PostAsync("create", multipartFormContent);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<ProductCategoryDTO>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<ProductCategoryDTO> UpdateAsync(ProductCategoryDTO dto, CancellationToken cancellationToken)
    {
        using var multipartFormContent = ConvertHelper.ConvertToMultipartFormContent(dto);
        using var response = await httpClient.PutAsync("update", multipartFormContent);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<ProductCategoryDTO>>(cancellationToken: cancellationToken);
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
    public async Task<ProductCategoryDTO> GetAsync(long id, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync($"get/{id}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<ProductCategoryDTO>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<IEnumerable<ProductCategoryDTO>> GetAllAsync(CancellationToken cancellationToken)
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

        var result = await response.Content.ReadFromJsonAsync<Response<IEnumerable<ProductCategoryDTO>>>(options, cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }
}
