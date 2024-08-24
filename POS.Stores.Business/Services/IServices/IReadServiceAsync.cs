﻿namespace POS.Stores.Business.Services.IServices
{
    public interface IReadServiceAsync<TEntity, TDto>
            where TEntity : class
            where TDto : class
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> GetByIdAsync(int id);
        Task<bool> Exists(int id);
    }
}
