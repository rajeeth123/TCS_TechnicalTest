using TCSTest.DTOs;

namespace TCSTest.Interface
{
    public interface IChannelService
    {
        Task<IReadOnlyList<ChannelDto>> GetAllAsync();        
        Task<ChannelDto> CreateAsync(CreateChannelDto dto);
        Task<ChannelDto?> UpdateAsync(Guid id, UpdateChannelDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
