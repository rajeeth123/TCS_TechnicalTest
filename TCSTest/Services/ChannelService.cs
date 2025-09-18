
using Microsoft.AspNetCore.Http.HttpResults;
using TCSTest.DTOs;
using TCSTest.Interface;
using TCSTest.Models;
using TCSTest.Repositories;

namespace TCSTest.Services
{
    public class ChannelService : IChannelService
    {
        private readonly IRepository<Channel> _channelRepository;
        public ChannelService(IRepository<Channel> channelRepository) => _channelRepository = channelRepository;

        public async Task<IReadOnlyList<ChannelDto>> GetAllAsync()
        {
            var all = await _channelRepository.GetAllAsync();
            return all.Select(x => new ChannelDto(x.ChannelId,x.Name,x.Category,x.Language,x.Region)).ToList();
        }        

        public async Task<ChannelDto> CreateAsync(CreateChannelDto dto)
        {
            var channel = new Channel { ChannelId = Guid.NewGuid(), Name = dto.Name, Category = dto.Category, Language = dto.Language,Region=dto.Region };
            var created = await _channelRepository.AddAsync(channel);
            return new ChannelDto(created.ChannelId, created.Name, created.Category, created.Language,created.Region);
        }

        public async Task<ChannelDto?> UpdateAsync(Guid id, UpdateChannelDto dto)
        {
            var existing = await _channelRepository.GetAsync(id);
            if (existing == null) return null;
            var updated = new Channel
            {
                ChannelId = existing.ChannelId,
                Name = dto.Name ?? existing.Name,
                Category = dto.Category ?? existing.Category,
                Language = dto.Language ?? existing.Language,
                Region = dto.Region ?? existing.Region
            };
            var res = await _channelRepository.UpdateAsync(id, updated);
            return res == null ? null : new ChannelDto(res.ChannelId, res.Name, res.Category, res.Language, res.Region);
        }

        public async Task<bool> DeleteAsync(Guid id) => await _channelRepository.DeleteAsync(id);
    }
}
