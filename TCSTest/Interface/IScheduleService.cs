using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCSTest.DTOs;

namespace TCSTest.Interface
{
    public interface IScheduleService
    {
        Task<IReadOnlyList<ScheduleDto>> GetAllAsync();
        Task<IReadOnlyList<ScheduleDto>> GetByChannelAsync(Guid channelId);
        Task<IReadOnlyList<ScheduleDto>> GetNowPlayingAsync();
        Task<ScheduleDto> CreateAsync(CreateScheduleDto dto);
        Task<ScheduleDto?> UpdateAsync(Guid channelId, Guid contentId, UpdateScheduleDto dto);
        Task<bool> DeleteAsync(Guid channelId, Guid contentId);
    }
}
