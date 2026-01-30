namespace RickAndMortyBackend.DTOs
{
    public class CharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public LocationDto Origin { get; set; } = new LocationDto();
        public LocationDto Location { get; set; } = new LocationDto();
    }

    public class CharacterDetailDto : CharacterDto
    {
        public List<EpisodeDto> Episodes { get; set; } = new List<EpisodeDto>();
        public DateTime Created { get; set; }
    }

    public class LocationDto
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class EpisodeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AirDate { get; set; } = string.Empty;
        public string Episode { get; set; } = string.Empty;
    }

    public class PaginatedResponse<T>
    {
        public InfoDto Info { get; set; } = new InfoDto();
        public List<T> Results { get; set; } = new List<T>();
    }

    public class InfoDto
    {
        public int Count { get; set; }
        public int Pages { get; set; }
        public int? Next { get; set; }
        public int? Prev { get; set; }
    }
}
