using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TCSTest.DTOs;
using TCSTest.Models;
using TCSTest.Repositories;
using TCSTest.Exceptions;
using TCSTest.Interface;

namespace TCSTest.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IRepository<ScheduleEntry> _schedulerepository;
        private readonly IRepository<Channel> _channelRepository;
        private readonly IRepository<ContentItem> _contentRepository;

        public ScheduleService(IRepository<ScheduleEntry> schedulerepo, IRepository<Channel> channelRepo, IRepository<ContentItem> contentRepo)
        {
            _schedulerepository = schedulerepo; _channelRepository = channelRepo; _contentRepository = contentRepo;
        }

        public async Task<IReadOnlyList<ScheduleDto>> GetAllAsync()
        {
            var all = await _schedulerepository.GetAllAsync();
            return all.Select(ToDto).ToList();
        }

        public async Task<IReadOnlyList<ScheduleDto>> GetByChannelAsync(Guid channelId)
        {
            var all = await _schedulerepository.GetAllAsync();
            return all.Where(s => s.ChannelId == channelId).Select(ToDto).ToList();
        }

        public async Task<IReadOnlyList<ScheduleDto>> GetNowPlayingAsync()
        {
            var now = DateTime.UtcNow;
            var all = await _schedulerepository.GetAllAsync();
            return all.Where(s => s.AirTime <= now && s.EndTime > now).Select(ToDto).ToList();
        }

        public async Task<ScheduleDto> CreateAsync(CreateScheduleDto dto)
        {
            if (dto.EndTime <= dto.AirTime) throw new ValidationException("EndUtc must be after StartUtc");

            var channel = await _channelRepository.GetAsync(dto.ChannelId);
            if (channel == null) throw new ValidationException($"Channel {dto.ChannelId} not found");
            var content = await _contentRepository.GetAsync(dto.ContentId);
            if (content == null) throw new ValidationException($"Content {dto.ContentId} not found");

            var all = await _schedulerepository.GetAllAsync();
            var overlap = all.Any(s => s.ChannelId == dto.ChannelId && s.AirTime < dto.EndTime && s.EndTime > dto.AirTime);
            if (overlap) throw new ValidationException("Schedule overlaps with existing entry on the same channel");

            var entry = new ScheduleEntry { Id = Guid.NewGuid(), ChannelId = dto.ChannelId, ContentId = dto.ContentId, AirTime = dto.AirTime, EndTime = dto.EndTime };
            var created = await _schedulerepository.AddAsync(entry);
            return ToDto(created);
        }

        public async Task<ScheduleDto?> UpdateAsync(Guid channelId, Guid contentId, UpdateScheduleDto dto)
        {
            var all = await _schedulerepository.GetAllAsync();
            var existing = all.FirstOrDefault(s => s.ChannelId == channelId && s.ContentId == contentId);
            if (existing == null) return null;

            var airtime = dto.AirTime ?? existing.AirTime;
            var end = dto.EndTime ?? existing.EndTime;
            if (end <= airtime) throw new ValidationException("EndTime must be after AirTime");

            var overlaps = all.Any(s => s.ChannelId == channelId && s.Id != existing.Id && s.AirTime < end && s.EndTime > airtime);
            if (overlaps) throw new ValidationException("Updated schedule overlaps with existing entry on the same channel");

            // update and persist
           
            var updatedEntry = new ScheduleEntry
            {
                Id = existing.Id,
                ChannelId = existing.ChannelId,
                ContentId = existing.ContentId,
                AirTime = airtime,
                EndTime = end
            };
            var updated = await _schedulerepository.UpdateAsync(existing.Id, updatedEntry);
            return updated == null ? null : ToDto(updated);            
        }

        public async Task<bool> DeleteAsync(Guid channelId, Guid contentId)
        {
            var all = await _schedulerepository.GetAllAsync();
            var existing = all.FirstOrDefault(s => s.ChannelId == channelId && s.ContentId == contentId);
            if (existing == null) return false;
            return await _schedulerepository.DeleteAsync(existing.Id);
        }

        private static ScheduleDto ToDto(ScheduleEntry s) => new(s.Id, s.ChannelId, s.ContentId, s.AirTime, s.EndTime);
    }
}
