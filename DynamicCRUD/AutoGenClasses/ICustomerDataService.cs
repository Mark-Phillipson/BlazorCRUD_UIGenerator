
using SampleApplication.DTOs;

namespace SampleApplication.Services
{
    public interface ICustomerDataService
    {
        Task<List<CustomerDTO>> GetAllCustomersAsync( );
        Task<List<CustomerDTO>> SearchCustomersAsync(string serverSearchTerm);
        Task<CustomerDTO?> AddCustomer(CustomerDTO customerDTO);
        Task<CustomerDTO?> GetCustomerById(int id);
        Task<CustomerDTO> UpdateCustomer(CustomerDTO customerDTO, string? username);
        Task DeleteCustomer(int id);
    }
}
