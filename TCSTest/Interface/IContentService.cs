using TCSTest.DTOs;

namespace TCSTest.Interface
{
    public interface IContentService
    {
        Task<IReadOnlyList<ContentDto>> GetAllAsync();
        Task<ContentDto?> GetAsync(Guid id);
        Task<ContentDto> CreateContentAsync(CreateContentDto dto);       
        Task<ContentDto?> UpdateContentAsync(Guid id, UpdateContentDto dto);        
        Task<bool> DeleteAsync(Guid id);
    }
}
