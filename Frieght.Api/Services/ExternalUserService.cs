using System.Net.Http;
using System.Text.Json;
using Frieght.Api.Dtos;
using Microsoft.Extensions.Configuration;

namespace Frieght.Api.Services;

public interface IExternalUserService
{
    Task<UserDto?> GetUserAsync(string userId);
}

public class ExternalUserService : IExternalUserService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ExternalUserService> _logger;

    public ExternalUserService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ExternalUserService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<UserDto?> GetUserAsync(string userId)
    {
        try
        {
            var baseUrl = _configuration["ExternalServices:UserApi:BaseUrl"];
            var response = await _httpClient.GetAsync($"{baseUrl}/api/users/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            _logger.LogWarning("Failed to get user with ID: {UserId}. Status code: {StatusCode}",
                userId, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching user with ID: {UserId}", userId);
            return null;
        }
    }
} 