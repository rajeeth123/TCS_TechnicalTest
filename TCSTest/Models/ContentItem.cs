namespace TCSTest.Models
{
    public record ContentItem
    {
        public Guid ContentId { get; init; } = Guid.NewGuid();
        public string Title { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Genre { get; init; } = string.Empty;
        public int DurationMinutes { get; init; }
        public string Rating { get; init; } = string.Empty;
        public int Year { get; init; }
        public int? Season { get; init; }
        public int? Episode { get; init; }
        
    }
}
