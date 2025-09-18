using System;
namespace TCSTest.DTOs
{
    public record ContentDto(Guid ContentId, string Title, string Type, string Genre, int DurationMinutes,string Rating, int Year, int? Season, int? Episode);

    public record CreateContentDto(string Title, string Type, string Genre, int DurationMinutes, string Rating, int Year, int? Season, int? Episode);

    public record UpdateContentDto(string Title, string Type, string Genre, int? DurationMinutes, string Rating, int? Year, int? Season, int? Episode);
}
