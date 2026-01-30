using RickAndMortyBackend.DTOs;
using RickAndMortyBackend.Repositories.Interfaces;
using RickAndMortyBackend.Services.Interfaces;
using RickAndMortyBackend.Models;
using RickAndMortyBackend.Models.ApiModels;
using System.Text.Json;

namespace RickAndMortyBackend.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly IRickAndMortyApiService _apiService;
        private readonly ICharacterRepository _characterRepository;
        private readonly ILogger<CharacterService> _logger;

        public CharacterService(
            IRickAndMortyApiService apiService,
            ICharacterRepository characterRepository,
            ILogger<CharacterService> logger)
        {
            _apiService = apiService;
            _characterRepository = characterRepository;
            _logger = logger;
        }

        public async Task<PaginatedResponse<CharacterDto>> GetCharactersAsync(
            int page = 1,
            string? name = null,
            string? status = null,
            string? species = null)
        {
            try
            {
                var apiResponse = await _apiService.GetCharactersAsync(page, name, status, species);

                // Save characters to database
                await SaveCharactersToDatabase(apiResponse.Results);

                var characterDtos = apiResponse.Results.Select(MapToDto).ToList();

                return new PaginatedResponse<CharacterDto>
                {
                    Info = new InfoDto
                    {
                        Count = apiResponse.Info.Count,
                        Pages = apiResponse.Info.Pages,
                        Next = ExtractPageNumber(apiResponse.Info.Next),
                        Prev = ExtractPageNumber(apiResponse.Info.Prev)
                    },
                    Results = characterDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting characters");
                throw;
            }
        }

        public async Task<CharacterDetailDto?> GetCharacterByIdAsync(int id)
        {
            try
            {
                var apiCharacter = await _apiService.GetCharacterByIdAsync(id);

                if (apiCharacter == null)
                    return null;

                // Save character to database
                await SaveCharacterToDatabase(apiCharacter);

                // Get episodes
                var episodeIds = apiCharacter.Episode
                    .Select(url => int.Parse(url.Split('/').Last()))
                    .ToList();

                var episodes = await _apiService.GetEpisodesByIdsAsync(episodeIds);

                var detailDto = new CharacterDetailDto
                {
                    Id = apiCharacter.Id,
                    Name = apiCharacter.Name,
                    Status = apiCharacter.Status,
                    Species = apiCharacter.Species,
                    Type = apiCharacter.Type,
                    Gender = apiCharacter.Gender,
                    Image = apiCharacter.Image,
                    Origin = new LocationDto
                    {
                        Name = apiCharacter.Origin.Name,
                        Url = apiCharacter.Origin.Url
                    },
                    Location = new LocationDto
                    {
                        Name = apiCharacter.Location.Name,
                        Url = apiCharacter.Location.Url
                    },
                    Created = apiCharacter.Created,
                    Episodes = episodes.Select(e => new EpisodeDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        AirDate = e.Air_date,
                        Episode = e.Episode
                    }).ToList()
                };

                return detailDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting character {id}");
                throw;
            }
        }

        private CharacterDto MapToDto(CharacterApi apiCharacter)
        {
            return new CharacterDto
            {
                Id = apiCharacter.Id,
                Name = apiCharacter.Name,
                Status = apiCharacter.Status,
                Species = apiCharacter.Species,
                Type = apiCharacter.Type,
                Gender = apiCharacter.Gender,
                Image = apiCharacter.Image,
                Origin = new LocationDto
                {
                    Name = apiCharacter.Origin.Name,
                    Url = apiCharacter.Origin.Url
                },
                Location = new LocationDto
                {
                    Name = apiCharacter.Location.Name,
                    Url = apiCharacter.Location.Url
                }
            };
        }

        private async Task SaveCharactersToDatabase(List<CharacterApi> apiCharacters)
        {
            var characters = apiCharacters.Select(ac => new Character
            {
                Id = ac.Id,
                Name = ac.Name,
                Status = ac.Status,
                Species = ac.Species,
                Type = ac.Type,
                Gender = ac.Gender,
                Image = ac.Image,
                Url = ac.Url,
                Created = ac.Created,
                OriginName = ac.Origin.Name,
                OriginUrl = ac.Origin.Url,
                LocationName = ac.Location.Name,
                LocationUrl = ac.Location.Url,
                EpisodesJson = JsonSerializer.Serialize(ac.Episode)
            }).ToList();

            await _characterRepository.UpsertCharactersAsync(characters);
        }

        private async Task SaveCharacterToDatabase(CharacterApi apiCharacter)
        {
            var character = new Character
            {
                Id = apiCharacter.Id,
                Name = apiCharacter.Name,
                Status = apiCharacter.Status,
                Species = apiCharacter.Species,
                Type = apiCharacter.Type,
                Gender = apiCharacter.Gender,
                Image = apiCharacter.Image,
                Url = apiCharacter.Url,
                Created = apiCharacter.Created,
                OriginName = apiCharacter.Origin.Name,
                OriginUrl = apiCharacter.Origin.Url,
                LocationName = apiCharacter.Location.Name,
                LocationUrl = apiCharacter.Location.Url,
                EpisodesJson = JsonSerializer.Serialize(apiCharacter.Episode)
            };

            await _characterRepository.UpsertCharacterAsync(character);
        }

        private int? ExtractPageNumber(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            var uri = new Uri(url);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var pageStr = query["page"];

            return int.TryParse(pageStr, out var page) ? page : null;
        }
    }
}
