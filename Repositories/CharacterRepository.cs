using Microsoft.EntityFrameworkCore;
using RickAndMortyBackend.Data;
using RickAndMortyBackend.Models;
using RickAndMortyBackend.Repositories.Interfaces;

namespace RickAndMortyBackend.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CharacterRepository> _logger;

        public CharacterRepository(ApplicationDbContext context, ILogger<CharacterRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Character?> GetByIdAsync(int id)
        {
            return await _context.Characters.FindAsync(id);
        }

        public async Task<List<Character>> GetAllAsync(
            int page,
            int pageSize,
            string? name,
            string? status,
            string? species)
        {
            var query = _context.Characters.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.Name.Contains(name));

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);

            if (!string.IsNullOrEmpty(species))
                query = query.Where(c => c.Species == species);

            return await query
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(string? name, string? status, string? species)
        {
            var query = _context.Characters.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.Name.Contains(name));

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);

            if (!string.IsNullOrEmpty(species))
                query = query.Where(c => c.Species == species);

            return await query.CountAsync();
        }

        public async Task UpsertCharacterAsync(Character character)
        {
            var existing = await _context.Characters.FindAsync(character.Id);

            if (existing == null)
            {
                await _context.Characters.AddAsync(character);
            }
            else
            {
                _context.Entry(existing).CurrentValues.SetValues(character);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpsertCharactersAsync(List<Character> characters)
        {
            foreach (var character in characters)
            {
                var existing = await _context.Characters.FindAsync(character.Id);

                if (existing == null)
                {
                    await _context.Characters.AddAsync(character);
                }
                else
                {
                    _context.Entry(existing).CurrentValues.SetValues(character);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
