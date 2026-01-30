using RickAndMortyBackend.Models.ApiModels;

namespace RickAndMortyBackend.Services.Interfaces
{
    public interface IRickAndMortyApiService
    {
        Task<RickAndMortyApiResponse<CharacterApi>> GetCharactersAsync(int page = 1, string? name = null, string? status = null, string? species = null);
        Task<CharacterApi?> GetCharacterByIdAsync(int id);
        Task<List<EpisodeApi>> GetEpisodesByIdsAsync(List<int> episodeIds);
    }
}
