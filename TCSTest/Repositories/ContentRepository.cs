using TCSTest.Models;
using TCSTest.Repositories;

namespace TCSTest.Repositories
{
    public class ContentRepository : JsonRepository<ContentItem>
    {
        public ContentRepository(string filePath, JsonFileLockProvider lockProvider): base(filePath, lockProvider) { }


        /// <summary>
        /// Method to add content
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override async Task<ContentItem> AddAsync(ContentItem item)
        {
            var all = await ReadAllAsync();
            all.Add(item);
            await WriteAllAsync(all);
            return item;
        }

        /// <summary>
        /// Method to delete content
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<bool> DeleteAsync(Guid id)
        {
            var all = await ReadAllAsync();
            var removed = all.RemoveAll(x => x.ContentId == id) > 0;
            if (removed) await WriteAllAsync(all);
            return removed;
        }

        /// <summary>
        /// To get all content details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<ContentItem?> GetAsync(Guid id)
        {
            var all = await ReadAllAsync();
            return all.FirstOrDefault(x => x.ContentId == id);
        }

        /// <summary>
        /// To update content details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public override async Task<ContentItem?> UpdateAsync(Guid id, ContentItem item)
        {
            var all = await ReadAllAsync();
            var idx = all.FindIndex(x => x.ContentId == id);
            if (idx == -1) return null;
            all[idx] = item;
            await WriteAllAsync(all);
            return item;
        }
    }
}
