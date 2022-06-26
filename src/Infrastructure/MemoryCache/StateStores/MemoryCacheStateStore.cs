﻿using Microsoft.Extensions.Caching.Memory;
using NetCoreCleanArchitecture.Application.Common.StateStores;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreCleanArchitecture.Infrastructure.InMemory.StateStores
{
    public class MemoryCacheStateStore<T> : IStateStore<T> where T : class
    {
        private readonly IMemoryCache _client;

        public MemoryCacheStateStore(IMemoryCache client)
        {
            _client = client;
        }

        public async Task<IEnumerable<T>?> GetBulkAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
        {
            var result = new List<T>();

            foreach (var key in keys)
            {
                if (cancellationToken.IsCancellationRequested) throw new TaskCanceledException();

                var state = await GetAsync(key, cancellationToken);

                if (!(state is null)) result.Add(state);
            }

            return result;
        }

        public async Task<T?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            var item = _client.Get<T>(key);

            return await Task.FromResult(item);
        }

        public Task<T> GetOrCreateAsync(string key, Func<Task<T>> factory, CancellationToken cancellationToken, int ttlSeconds = -1)
        {
            return _client.GetOrCreateAsync<T>(key, entry =>
            {
                if (ttlSeconds > 0)
                {
                    entry.SlidingExpiration = TimeSpan.FromSeconds(ttlSeconds);
                }

                return factory();
            });
        }

        public Task AddAsync(string key, T item, CancellationToken cancellationToken, int ttlSeconds = -1)
        {
            if (ttlSeconds > 0)
            {
                _client.Set<T>(key, item, TimeSpan.FromSeconds(ttlSeconds));
            }
            else
            {
                _client.Set<T>(key, item);
            }

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            _client.Remove(key);

            return Task.CompletedTask;
        }
    }
}
