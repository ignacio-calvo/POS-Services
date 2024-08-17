using AutoMapper;
using POS.Customers.Business.DTOs;
using POS.Customers.Business.Services.IServices.IServiceMappings;
using POS.Customers.Data.Interfaces;
using POS.Customers.Data.Models;

namespace POS.Customers.Business.Services.ServiceMappings
{
    public class CustomerService : GenericServiceAsync<Customer, CustomerDto>, ICustomerService
    {
        public CustomerService(ICustomerRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
} 
