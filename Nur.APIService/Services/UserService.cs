using System.Net.Http.Json;
using Nur.APIService.Helpers;
using Nur.APIService.Interfaces;
using Nur.APIService.Models.Users;
using Microsoft.Extensions.Logging;
using Nur.APIService.Models.Response;

namespace Nur.APIService.Services;

public class UserService(HttpClient httpClient, ILogger<UserService> logger) : IUserService
{
    public async Task<UserDTO> AddAsync(UserCreationDTO dto, CancellationToken cancellationToken)
    {
        using var multipartFormContent = ConvertHelper.ConvertToMultipartFormContent(dto);
        using var response = await httpClient.PostAsync("create", multipartFormContent);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<UserDTO>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<UserDTO> UpdateAsync(UserDTO dto, CancellationToken cancellationToken)
    {
        using var multipartFormContent = ConvertHelper.ConvertToMultipartFormContent(dto);
        using var response = await httpClient.PutAsync("update", multipartFormContent);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<UserDTO>>(cancellationToken: cancellationToken);
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
    public async Task<UserDTO> GetAsync(long telegramId, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync($"get-by-telegram-id/{telegramId}", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<UserDTO>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }

    public async Task<IEnumerable<UserDTO>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync("get-all", cancellationToken);
        if (!response.IsSuccessStatusCode)
            return default!;

        var result = await response.Content.ReadFromJsonAsync<Response<IEnumerable<UserDTO>>>(cancellationToken: cancellationToken);
        if (result!.Status == 200)
            return result.Data;

        logger.LogInformation(message: result.Message);
        return default!;
    }
}
