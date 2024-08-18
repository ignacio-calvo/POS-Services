using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using POS.Products.Business.Configuration;
using POS.Products.Business.CustomExceptions;
using POS.Products.Business.DTOs;
using POS.Products.Business.Services.IServices.IServiceMappings;
using POS.Products.Data.Interfaces;
using POS.Products.Data.Models;
using POS.Products.Data.Repositories;

namespace POS.Products.Business.Services.ServiceMappings
{
    public class CategoryService : GenericServiceAsync<Category, CategoryDto>, ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration;

        public CategoryService(ICategoryRepository repository, IMapper mapper, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
            : base(repository, mapper, cache, cacheSettings)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
            _cacheDuration = TimeSpan.FromHours(cacheSettings.Value.CacheDurationInHours);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                var result = await _repository.ExistsAsync(id);

                if (!result)
                {
                    throw new EntityNotFoundException($"Entity with ID {id} not found.");
                }

                return result;
            }
            catch (EntityNotFoundException ex)
            {
                var message = $"Error retrieving {typeof(CategoryDto).Name} with Id: {id}";
                throw new EntityNotFoundException(message, ex);
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetAllWithProductsAsync()
        {
            const string cacheKey = "all_categories_with_products";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<CategoryDto>? cachedCategories))
            {
                try
                {
                    var result = await _repository.GetAllCategoriesWithProductsAsync();

                    if (result.Any())
                    {
                        cachedCategories = _mapper.Map<IEnumerable<CategoryDto>>(result);

                        var cacheEntryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = _cacheDuration
                        };

                        _cache.Set(cacheKey, cachedCategories, cacheEntryOptions);
                    }
                    else
                    {
                        throw new EntityNotFoundException($"No {typeof(CategoryDto).Name}s were found");
                    }
                }
                catch (EntityNotFoundException ex)
                {
                    var message = $"Error retrieving all {typeof(CategoryDto).Name}s";
                    throw new EntityNotFoundException(message, ex);
                }
            }

            return cachedCategories ?? Enumerable.Empty<CategoryDto>();
        }

        public async Task<IEnumerable<CategoryDto>> GetAllWithProductsSizesAsync()
        {
            const string cacheKey = "all_categories_with_products_sizes";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<CategoryDto>? cachedCategories))
            {
                try
                {
                    var result = await _repository.GetAllCategoriesWithProductsSizesAsync();

                    if (result.Any())
                    {
                        cachedCategories = _mapper.Map<IEnumerable<CategoryDto>>(result);

                        var cacheEntryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = _cacheDuration
                        };

                        _cache.Set(cacheKey, cachedCategories, cacheEntryOptions);
                    }
                    else
                    {
                        throw new EntityNotFoundException($"No {typeof(CategoryDto).Name}s were found");
                    }
                }
                catch (EntityNotFoundException ex)
                {
                    var message = $"Error retrieving all {typeof(CategoryDto).Name}s";
                    throw new EntityNotFoundException(message, ex);
                }
            }

            return cachedCategories ?? Enumerable.Empty<CategoryDto>();
        }
    }
} 
