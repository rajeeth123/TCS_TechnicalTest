using System;
using System.ComponentModel.DataAnnotations;

namespace TCSTest.DTOs
{
    public record ScheduleDto(Guid Id,Guid ChannelId, Guid ContentId, DateTime AirTime, DateTime EndTime);
    public record CreateScheduleDto([Required] Guid ChannelId, [Required] Guid ContentId, [Required] DateTime AirTime, [Required] DateTime EndTime);
    public record UpdateScheduleDto(DateTime? AirTime, DateTime? EndTime);
}
