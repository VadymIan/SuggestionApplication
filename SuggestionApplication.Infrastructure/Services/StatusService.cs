using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SuggestionApplication.Application.Infrastructure;
using SuggestionApplication.Application.Persistance;
using SuggestionApplication.Domain.Entities;
using System;

namespace SuggestionApplication.Infrastructure.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        private readonly ILogger<StatusService> _logger;
        private readonly IMemoryCache _cache;

        private const string CacheName = "StatusData";

        public StatusService(IStatusRepository statusRepository, ILogger<StatusService> logger, IMemoryCache cache)
        {
            _statusRepository = statusRepository;
            _logger = logger;
            _cache = cache;
        }

        public async Task CreateStatusAsync(Status status)
        {
            await _statusRepository.AddAsync(status);
        }

        public async Task<List<Status>> GetAllStatusesAsync()
        {
            var output = _cache.Get<List<Status>>(CacheName);

            if (output is null)
            {
                var statuses = await _statusRepository.GetAllAsync();

                output = statuses.ToList();

                _cache.Set(CacheName, output, TimeSpan.FromDays(1));
            }

            return output;
        }
    }
}
