using RickAndMortyBackend.Models.ApiModels;
using RickAndMortyBackend.Services.Interfaces;
using System.Text.Json;

namespace RickAndMortyBackend.Services
{
    public class RickAndMortyApiService : IRickAndMortyApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RickAndMortyApiService> _logger;
        private const string BaseUrl = "https://rickandmortyapi.com/api";

        public RickAndMortyApiService(HttpClient httpClient, ILogger<RickAndMortyApiService> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _logger = logger;
        }

        public async Task<RickAndMortyApiResponse<CharacterApi>> GetCharactersAsync(
            int page = 1,
            string? name = null,
            string? status = null,
            string? species = null)
        {
            try
            {
                var queryParams = new List<string> { $"page={page}" };
                if (!string.IsNullOrEmpty(name)) queryParams.Add($"name={Uri.EscapeDataString(name)}");
                if (!string.IsNullOrEmpty(status)) queryParams.Add($"status={Uri.EscapeDataString(status)}");
                if (!string.IsNullOrEmpty(species)) queryParams.Add($"species={Uri.EscapeDataString(species)}");

                var queryString = string.Join("&", queryParams);
                var response = await _httpClient.GetAsync($"/character?{queryString}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"API request failed with status code: {response.StatusCode}");
                    return new RickAndMortyApiResponse<CharacterApi>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<RickAndMortyApiResponse<CharacterApi>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result ?? new RickAndMortyApiResponse<CharacterApi>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching characters from Rick and Morty API");
                throw;
            }
        }

        public async Task<CharacterApi?> GetCharacterByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/character/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Character {id} not found in API");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<CharacterApi>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching character {id} from Rick and Morty API");
                throw;
            }
        }

        public async Task<List<EpisodeApi>> GetEpisodesByIdsAsync(List<int> episodeIds)
        {
            try
            {
                if (!episodeIds.Any())
                    return new List<EpisodeApi>();

                var ids = string.Join(",", episodeIds);
                var response = await _httpClient.GetAsync($"/episode/{ids}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Episodes not found in API");
                    return new List<EpisodeApi>();
                }

                var content = await response.Content.ReadAsStringAsync();

                // The API returns a single object if one ID, or an array if multiple
                if (episodeIds.Count == 1)
                {
                    var episode = JsonSerializer.Deserialize<EpisodeApi>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return episode != null ? new List<EpisodeApi> { episode } : new List<EpisodeApi>();
                }
                else
                {
                    var episodes = JsonSerializer.Deserialize<List<EpisodeApi>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return episodes ?? new List<EpisodeApi>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching episodes from Rick and Morty API");
                throw;
            }
        }
    }
}
