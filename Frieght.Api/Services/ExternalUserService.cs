using System.Net.Http;
using System.Text.Json;
using Frieght.Api.Dtos;

namespace Frieght.Api.Services;

public interface IExternalUserService
{
    Task<UserDto?> GetUserAsync(string userId);
}

public class ExternalUserService : IExternalUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExternalUserService> _logger;
    private const string BaseUrl = "https://api.auth.afroinnovate.com/user/";

    public ExternalUserService(HttpClient httpClient, ILogger<ExternalUserService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<UserDto?> GetUserAsync(string userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDto>(content);
            }
            _logger.LogWarning("Failed to fetch user with ID {UserId}. Status code: {StatusCode}", userId, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user with ID {UserId} from external API", userId);
            return null;
        }
    }
} 