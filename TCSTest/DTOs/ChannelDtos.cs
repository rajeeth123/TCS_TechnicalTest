using System;
using System.ComponentModel.DataAnnotations;

namespace TCSTest.DTOs
{
    public record ChannelDto(Guid ChannelId, string Name, string Category, string Language, string Region);
    public record CreateChannelDto([Required][StringLength(200)] string Name, [Required][StringLength(200)] string Category, [Required][StringLength(200)] string Language, [Required][StringLength(200)] string Region);
    public record UpdateChannelDto(string? Name, string? Category, string? Language,string Region);
}
