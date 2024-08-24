﻿using AutoMapper;
using POS.Stores.Business.Services.IServices;
using POS.Stores.Data.Interfaces;
using POS.Stores.Business.CustomExceptions;


namespace POS.Stores.Business.Services
{
    public class ReadServiceAsync<TEntity, TDto> : IReadServiceAsync<TEntity, TDto>
            where TEntity : class
            where TDto : class
    {
        private readonly IStoreRepository _repository;
        private readonly IMapper _mapper;

        public ReadServiceAsync(IStoreRepository repo, IMapper mapper) : base()
        {
            _repository = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAll();

                if (result.Any())
                {
                    return _mapper.Map<IEnumerable<TDto>>(result);
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

        public async Task<TDto> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetById(id);

                if (result is null)
                {
                    throw new EntityNotFoundException($"Entity with ID {id} not found.");
                }

                return _mapper.Map<TDto>(result);
            }
            catch (EntityNotFoundException ex)
            {
                var message = $"Error retrieving {typeof(TDto).Name} with Id: {id}";
                throw new EntityNotFoundException(message, ex);
            }
        }

        public async Task<bool> Exists(int id)
        {
            try
            {
                var result = await _repository.Exists(id);

                if (!result)
                {
                    throw new EntityNotFoundException($"Entity with ID {id} not found.");
                }

                return result;
            }
            catch (EntityNotFoundException ex)
            {
                var message = $"Error retrieving {typeof(TDto).Name} with Id: {id}";
                throw new EntityNotFoundException(message, ex);
            }
        }
    }
}
