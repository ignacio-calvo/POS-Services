using AutoMapper;
using POS.Products.Business.Services.IServices;
using POS.Products.Business.CustomExceptions;
using POS.Products.Data.Models;
using POS.Products.Data.Interfaces;
using Microsoft.Extensions.Caching.Memory;


namespace POS.Products.Business.Services
{
    public class ReadServiceAsync<TEntity, TDto> : IReadServiceAsync<TEntity, TDto>
            where TEntity : class
            where TDto : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromDays(1);

        public ReadServiceAsync(IGenericRepository<TEntity> repo, IMapper mapper, IMemoryCache cache) : base()
        {
            _repository = repo;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            const string cacheKey = "all_entities";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<TDto> cachedEntities))
            {
                try
                {
                    var result = await _repository.GetAllAsync();

                    if (result.Any())
                    {
                        cachedEntities = _mapper.Map<IEnumerable<TDto>>(result);

                        var cacheEntryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = _cacheDuration
                        };

                        _cache.Set(cacheKey, cachedEntities, cacheEntryOptions);
                    }
                    else
                    {
                        throw new EntityNotFoundException($"No {typeof(TDto).Name}s were found");
                    }
                }
                catch (EntityNotFoundException ex)
                {
                    var message = $"Error retrieving all {typeof(TDto).Name}s";
                    throw new EntityNotFoundException(message, ex);
                }
            }

            return cachedEntities;
        }

        public async Task<TDto> GetByIdAsync(int id)
        {
            if (!_cache.TryGetValue(id, out TDto cachedEntity))
            {
                try
                {
                    var result = await _repository.GetByIdAsync(id);

                    if (result is null)
                    {
                        throw new EntityNotFoundException($"Entity with ID {id} not found.");
                    }

                    cachedEntity = _mapper.Map<TDto>(result);

                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = _cacheDuration
                    };

                    _cache.Set(id, cachedEntity, cacheEntryOptions);
                }
                catch (EntityNotFoundException ex)
                {
                    var message = $"Error retrieving {typeof(TDto).Name} with Id: {id}";
                    throw new EntityNotFoundException(message, ex);
                }
            }

            return cachedEntity;
        }
    }
}
