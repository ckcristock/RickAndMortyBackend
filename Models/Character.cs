namespace RickAndMortyBackend.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime Created { get; set; }

        // Origin
        public string OriginName { get; set; } = string.Empty;
        public string OriginUrl { get; set; } = string.Empty;

        // Location
        public string LocationName { get; set; } = string.Empty;
        public string LocationUrl { get; set; } = string.Empty;

        // Episodes (stored as JSON string)
        public string EpisodesJson { get; set; } = string.Empty;
    }
}
