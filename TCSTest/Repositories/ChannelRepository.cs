using System;
using System.Linq;
using System.Threading.Tasks;
using TCSTest.Models;

namespace TCSTest.Repositories
{
    public class ChannelRepository : JsonRepository<Channel>
    {
        public ChannelRepository(string filePath, JsonFileLockProvider lockProvider) : base(filePath, lockProvider) { }

        public override async Task<Channel?> GetAsync(Guid id)
        {
            var all = await ReadAllAsync();
            return all.FirstOrDefault(x => x.ChannelId == id);
        }

        public override async Task<Channel> AddAsync(Channel item)
        {
            var all = await ReadAllAsync();
            all.Add(item);
            await WriteAllAsync(all);
            return item;
        }

        public override async Task<Channel?> UpdateAsync(Guid id, Channel item)
        {
            var all = await ReadAllAsync();
            var idx = all.FindIndex(x => x.ChannelId == id);
            if (idx == -1) return null;
            all[idx] = item;
            await WriteAllAsync(all);
            return item;
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var all = await ReadAllAsync();
            var removed = all.RemoveAll(x => x.ChannelId == id) > 0;
            if (removed) await WriteAllAsync(all);
            return removed;
        }
    }
}
