using RickAndMortyBackend.DTOs;

namespace RickAndMortyBackend.Services.Interfaces
{
    public interface ICharacterService
    {
        Task<PaginatedResponse<CharacterDto>> GetCharactersAsync(int page = 1, string? name = null, string? status = null, string? species = null);
        Task<CharacterDetailDto?> GetCharacterByIdAsync(int id);
    }
}
