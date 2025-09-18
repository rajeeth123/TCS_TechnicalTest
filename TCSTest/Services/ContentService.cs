using TCSTest.DTOs;
using TCSTest.Interface;
using TCSTest.Models;
using TCSTest.Repositories;

namespace TCSTest.Services
{
    public class ContentService : IContentService
    {
        private readonly IRepository<ContentItem> _contentRepository;

        public ContentService(IRepository<ContentItem> contentRepository)
        {
            _contentRepository = contentRepository;
        }

        /// <summary>
        /// Method for creating movie
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ContentDto> CreateContentAsync(CreateContentDto dto)
        {
            var movie = new ContentItem
            {
                ContentId = Guid.NewGuid(),
                Title = dto.Title,
                Type = dto.Type,
                Genre = dto.Genre,
                DurationMinutes = dto.DurationMinutes,
                Rating = dto.Rating,               
                Year = dto.Year,
                Season = dto.Season ?? 0,
                Episode = dto.Episode ?? 0
            };
            var created = await _contentRepository.AddAsync(movie);
            return ToDto(created);
        }

        
        /// <summary>
        /// Method for Delete Movie or show
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Guid id) => await _contentRepository.DeleteAsync(id);
        

        /// <summary>
        /// Get all content details
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyList<ContentDto>> GetAllAsync()
        {
            var all = await _contentRepository.GetAllAsync();
            return all.Select(ToDto).ToList();
        }

        /// <summary>
        /// Get the content by Guid id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ContentDto?> GetAsync(Guid id)
        {
            var item = await _contentRepository.GetAsync(id);
            return item is null ? null : ToDto(item);
        }

        /// <summary>
        /// Method to Update Movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ContentDto?> UpdateContentAsync(Guid id, UpdateContentDto dto)
        {
            var existing = await _contentRepository.GetAsync(id) as ContentItem;
            if (existing == null) return null;

            var final = new ContentItem
            {
                ContentId = existing.ContentId,
                Title = dto.Title ?? existing.Title,
                Type = dto.Type ?? existing.Type,
                Genre = dto.Genre ?? existing.Genre,
                DurationMinutes = dto.DurationMinutes ?? existing.DurationMinutes,
                Rating = dto.Rating ?? existing.Rating,
                Year = dto.Year ?? existing.Year,
                Season = dto.Season ?? existing.Season,               
                Episode = dto.Episode ?? existing.Episode
            };

            var res = await _contentRepository.UpdateAsync(id, final);
            return res == null ? null : ToDto(res);
        }

        
        private static ContentDto ToDto(ContentItem item)
        {
            var type = item switch
            {
                ContentItem => "Content",                
                _ => "Unknown"
            };

            return new ContentDto(item.ContentId, item.Title, item.Type, item.Genre, item.DurationMinutes,item.Rating, item.Year, item.Season,item.Episode);
        }
    }
}
