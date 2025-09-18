using System;

namespace TCSTest.Models
{
    public record Channel
    {
        public Guid ChannelId { get; init; } = Guid.NewGuid();
        public string Name { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string Language { get; init; } = string.Empty;
        public string Region { get; init; } = string.Empty;
    }
}
