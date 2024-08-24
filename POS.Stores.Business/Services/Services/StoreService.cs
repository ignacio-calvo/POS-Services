using AutoMapper;
using POS.Stores.Business.DTOs;
using POS.Stores.Business.Services.IServices.IServiceMappings;
using POS.Stores.Data.Interfaces;
using POS.Stores.Data.Models;

namespace POS.Stores.Business.Services.ServiceMappings
{
    public class StoreService : GenericServiceAsync<Store, StoreDto>, IStoreService
    {
        public StoreService(IStoreRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
} 
