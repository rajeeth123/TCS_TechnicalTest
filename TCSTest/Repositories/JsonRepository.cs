using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TCSTest.Repositories
{
    public class JsonFileLockProvider
    {
        private readonly System.Collections.Generic.Dictionary<string, SemaphoreSlim> _locks = new();
        private readonly object _sync = new();
        public SemaphoreSlim GetLock(string filePath)
        {
            lock (_sync)
            {
                if (!_locks.ContainsKey(filePath)) _locks[filePath] = new SemaphoreSlim(1, 1);
                return _locks[filePath];
            }
        }
    }

    public abstract class JsonRepository<T> : IRepository<T> where T : class
    {
        private readonly string _filePath;
        private readonly JsonFileLockProvider _lockProvider;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true, WriteIndented = true };

        protected JsonRepository(string filePath, JsonFileLockProvider lockProvider)
        {
            _filePath = filePath;
            _lockProvider = lockProvider;
        }

        protected virtual async Task<System.Collections.Generic.List<T>> ReadAllAsync()
        {
            var sem = _lockProvider.GetLock(_filePath);
            await sem.WaitAsync();
            try
            {
                if (!File.Exists(_filePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? "./");
                    await using var fs = File.Create(_filePath);
                    await JsonSerializer.SerializeAsync(fs, new System.Collections.Generic.List<T>(), _jsonOptions);
                    await fs.FlushAsync();
                }

                await using var stream = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var list = await JsonSerializer.DeserializeAsync<System.Collections.Generic.List<T>>(stream, _jsonOptions) ?? new System.Collections.Generic.List<T>();
                return list;
            }
            finally
            {
                sem.Release();
            }
        }

        protected virtual async Task WriteAllAsync(System.Collections.Generic.List<T> list)
        {
            var sem = _lockProvider.GetLock(_filePath);
            await sem.WaitAsync();
            try
            {
                await using var stream = File.Open(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                await JsonSerializer.SerializeAsync(stream, list, _jsonOptions);
                await stream.FlushAsync();
            }
            finally
            {
                sem.Release();
            }
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync() => await ReadAllAsync();

        public abstract Task<T?> GetAsync(System.Guid id);
        public abstract Task<T> AddAsync(T item);
        public abstract Task<T?> UpdateAsync(System.Guid id, T item);
        public abstract Task<bool> DeleteAsync(System.Guid id);
    }
}
