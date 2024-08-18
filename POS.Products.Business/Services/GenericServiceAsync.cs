using AutoMapper;
using POS.Products.Business.DTOs;
using POS.Products.Data.Models;
using POS.Products.Business.Services.IServices;
using POS.Products.Data.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using POS.Products.Business.Configuration;
using Microsoft.Extensions.Options;

namespace POS.Products.Business.Services
{
    public class GenericServiceAsync<TEntity, TDto> : ReadServiceAsync<TEntity, TDto>, IGenericServiceAsync<TEntity, TDto>
                where TEntity : class
                where TDto : class
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IMemoryCache _cache;

        public GenericServiceAsync(IGenericRepository<TEntity> repository, IMapper mapper, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
            : base(repository, mapper, cache, cacheSettings)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }

        private void InvalidateCache(object key)
        {
            _cache.Remove(key);
        }

        public async Task AddAsync(TDto dto)
        {
            await _repository.AddAsync(_mapper.Map<TEntity>(dto));
            await _repository.SaveAsync();
            InvalidateCache("all_entities"); // Invalidate cache for all entities
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteByIdAsync(id);
            await _repository.SaveAsync();
            InvalidateCache(id); // Invalidate cache for the specific entity
            InvalidateCache("all_entities"); // Invalidate cache for all entities
        }

        public async Task UpdateAsync(TDto dto)
        {
            TEntity entity = _mapper.Map<TEntity>(dto);
            await _repository.UpdateAsync(entity);
            await _repository.SaveAsync();
            InvalidateCache(entity); // Invalidate cache for the specific entity
            InvalidateCache("all_entities"); // Invalidate cache for all entities
        }
    }
}
