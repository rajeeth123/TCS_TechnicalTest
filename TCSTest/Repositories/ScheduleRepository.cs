using System;
using System.Linq;
using System.Threading.Tasks;
using TCSTest.Models;
using TCSTest.Repositories;

namespace TCSTest.Repositories
{
    public class ScheduleRepository : JsonRepository<ScheduleEntry>
    {
        public ScheduleRepository(string filePath, JsonFileLockProvider lockProvider) : base(filePath, lockProvider) { }

        public override async Task<ScheduleEntry?> GetAsync(Guid id)
        {
            var all = await ReadAllAsync();
            return all.FirstOrDefault(x => x.Id == id);
        }

        public override async Task<ScheduleEntry> AddAsync(ScheduleEntry item)
        {
            var all = await ReadAllAsync();
            all.Add(item);
            await WriteAllAsync(all);
            return item;
        }

        public override async Task<ScheduleEntry?> UpdateAsync(Guid id, ScheduleEntry item)
        {
            var all = await ReadAllAsync();
            var idx = all.FindIndex(x => x.Id == id);
            if (idx == -1) return null;
            all[idx] = item;
            await WriteAllAsync(all);
            return item;
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var all = await ReadAllAsync();
            var removed = all.RemoveAll(x => x.Id == id) > 0;
            if (removed) await WriteAllAsync(all);
            return removed;
        }
    }
}
