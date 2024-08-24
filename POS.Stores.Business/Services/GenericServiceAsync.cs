using AutoMapper;
using POS.Stores.Data.Models;
using POS.Stores.Business.Services.IServices;
using POS.Stores.Data.Interfaces;
using POS.Stores.Business.DTOs;
using Microsoft.EntityFrameworkCore;
using POS.Stores.Business.CustomExceptions;

namespace POS.Stores.Business.Services
{
    public class GenericServiceAsync<TEntity, TDto> : ReadServiceAsync<TEntity, TDto>, IGenericServiceAsync<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        private readonly IMapper _mapper;
        private readonly IStoreRepository _repository;


        public GenericServiceAsync(IStoreRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TDto> AddAsync(TDto dto)
        {
            try
            {
                Store entity = await _repository.Add(_mapper.Map<Store>(dto));
                await _repository.SaveChangesAsync();
                return _mapper.Map<TDto>(entity);
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
            Data.Models.Store entity = _mapper.Map<Store>(dto);
            await _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
