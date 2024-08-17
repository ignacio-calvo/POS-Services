using AutoMapper;
using POS.Orders.Business.Services.IServices;
using POS.Orders.Data.Interfaces;

namespace POS.Orders.Business.Services
{
    public class GenericServiceAsync<TEntity, TDto> : ReadServiceAsync<TEntity, TDto>, IGenericServiceAsync<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TEntity> _repository;

        public GenericServiceAsync(IGenericRepository<TEntity> repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task AddAsync(TDto dto)
        {
            await _repository.AddAsync(_mapper.Map<TEntity>(dto));
            await _repository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteByIdAsync(id);
            await _repository.SaveAsync();
        }

        public async Task UpdateAsync(TDto dto)
        {
            TEntity entity = _mapper.Map<TEntity>(dto);
            await _repository.UpdateAsync(entity);
            await _repository.SaveAsync();
        }
    }
}
