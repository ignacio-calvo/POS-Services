using AutoMapper;
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

        public ProductService(IProductRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
    }
} 
