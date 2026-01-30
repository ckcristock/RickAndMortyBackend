using RickAndMortyBackend.Models;

namespace RickAndMortyBackend.Repositories.Interfaces
{
    public interface ICharacterRepository
    {
        Task<Character?> GetByIdAsync(int id);
        Task<List<Character>> GetAllAsync(int page, int pageSize, string? name, string? status, string? species);
        Task<int> GetTotalCountAsync(string? name, string? status, string? species);
        Task UpsertCharacterAsync(Character character);
        Task UpsertCharactersAsync(List<Character> characters);
    }
}
