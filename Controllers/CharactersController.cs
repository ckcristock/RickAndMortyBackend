using Microsoft.AspNetCore.Mvc;
using RickAndMortyBackend.DTOs;
using RickAndMortyBackend.Services.Interfaces;

namespace RickAndMortyBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        private readonly ILogger<CharactersController> _logger;

        public CharactersController(ICharacterService characterService, ILogger<CharactersController> logger)
        {
            _characterService = characterService;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated list of characters with optional filters
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="name">Filter by name</param>
        /// <param name="status">Filter by status (alive, dead, unknown)</param>
        /// <param name="species">Filter by species</param>
        /// <returns>Paginated character list</returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<CharacterDto>>> GetCharacters(
            [FromQuery] int page = 1,
            [FromQuery] string? name = null,
            [FromQuery] string? status = null,
            [FromQuery] string? species = null)
        {
            try
            {
                if (page < 1)
                {
                    return BadRequest(new { error = "Page number must be greater than 0" });
                }

                var result = await _characterService.GetCharactersAsync(page, name, status, species);

                if (!result.Results.Any())
                {
                    return NotFound(new { error = "No characters found" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting characters");
                return StatusCode(500, new { error = "An error occurred while processing your request" });
            }
        }

        /// <summary>
        /// Get character details by ID
        /// </summary>
        /// <param name="id">Character ID</param>
        /// <returns>Character details with episodes</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterDetailDto>> GetCharacterById(int id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest(new { error = "Invalid character ID" });
                }

                var character = await _characterService.GetCharacterByIdAsync(id);

                if (character == null)
                {
                    return NotFound(new { error = $"Character with ID {id} not found" });
                }

                return Ok(character);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting character {id}");
                return StatusCode(500, new { error = "An error occurred while processing your request" });
            }
        }
    }
}
