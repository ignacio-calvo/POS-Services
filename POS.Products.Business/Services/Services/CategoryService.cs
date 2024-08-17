using AutoMapper;
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

        public CategoryService(ICategoryRepository repository, IMapper mapper) : base(repository, mapper)
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

        public async Task<IEnumerable<CategoryDto>> GetAllWithProductsAsync()
        {
            try
            {
                var result = await _repository.GetAllCategoriesWithProductsAsync();

                if (result.Any())
                {
                    return _mapper.Map<IEnumerable<CategoryDto>>(result);
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
        public async Task<IEnumerable<CategoryDto>> GetAllWithProductsSizesAsync()
        {
            try
            {
                var result = await _repository.GetAllCategoriesWithProductsSizesAsync();

                if (result.Any())
                {
                    return _mapper.Map<IEnumerable<CategoryDto>>(result);
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

    }
} 
