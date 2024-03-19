
using SampleApplication.DTOs;

namespace SampleApplication.Repositories
{
    public interface ICustomerRepository
    {
        Task<CustomerDTO?> AddCustomerAsync(CustomerDTO customerDTO);
        Task DeleteCustomerAsync(int id);
        Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync(int maxRows);
        Task<IEnumerable<CustomerDTO>> SearchCustomersAsync(string serverSearchTerm);
        Task<CustomerDTO?> GetCustomerByIdAsync(int id);
        Task<CustomerDTO?> UpdateCustomerAsync(CustomerDTO customerDTO);
    }
}