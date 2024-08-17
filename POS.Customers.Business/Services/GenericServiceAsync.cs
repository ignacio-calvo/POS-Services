using AutoMapper;
using POS.Customers.Data.Models;
using POS.Customers.Business.Services.IServices;
using POS.Customers.Data.Interfaces;
using POS.Customers.Business.DTOs;
using Microsoft.EntityFrameworkCore;
using POS.Customers.Business.CustomExceptions;

namespace POS.Customers.Business.Services
{
    public class GenericServiceAsync<TEntity, TDto> : ReadServiceAsync<TEntity, TDto>, IGenericServiceAsync<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _repository;


        public GenericServiceAsync(ICustomerRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> AddAsync(TDto dto)
        {
            try
            {
                Customer entity = await _repository.Add(_mapper.Map<Customer>(dto));
                await _repository.SaveChangesAsync();
                return _mapper.Map<TDto>(entity);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("Cannot insert duplicate key") == true &&
                                                ex.InnerException?.Message.Contains("unique index") == true)
            {
                if (ex.InnerException.Message.Contains("Email"))
                    throw new UniqueConstraintViolationException("Email", "A customer with the same email already exists.", ex); // Throw custom exception for unique constraint violation with specific field name
                else throw new InvalidOperationException("A record with the same unique key already exists.", ex);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                throw new InvalidOperationException("An error occurred while adding the record.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteById(id);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(TDto dto)
        {
            Data.Models.Customer entity = _mapper.Map<Customer>(dto);
            await _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
