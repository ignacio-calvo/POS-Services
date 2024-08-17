using POS.Customers.Business.DTOs;
using POS.Customers.Data.Models;


namespace POS.Customers.Business.Services.IServices.IServiceMappings
{

    public interface ICustomerService: IGenericServiceAsync<Customer, CustomerDto>
    {
    }
}
