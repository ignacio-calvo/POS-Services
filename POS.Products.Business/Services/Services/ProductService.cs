using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using POS.Products.Business.Configuration;
using POS.Products.Business.CustomExceptions;
using POS.Products.Business.DTOs;
using POS.Products.Business.Services.IServices.IServiceMappings;
using POS.Products.Data.Models;

namespace POS.Products.Business.Services.ServiceMappings
{
    public class ProductService : GenericServiceAsync<Product, ProductDto>, IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _repository;
        private readonly IMemoryCache _cache;

        public ProductService(IProductRepository repository, IMapper mapper, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
            : base(repository, mapper, cache, cacheSettings)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
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
                var message = $"Error retrieving {typeof(ProductDto).Name} with Id: {id}";
                throw new EntityNotFoundException(message, ex);
            }
        }
    }
} 
