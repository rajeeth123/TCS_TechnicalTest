using System;

namespace TCSTest.Models
{
    public record ScheduleEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ChannelId { get; init; }
        public Guid ContentId { get; init; }
        public DateTime AirTime { get; init; }
        public DateTime EndTime { get; init; }
    }
}
