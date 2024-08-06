
using SampleApplication.DTOs;

namespace SampleApplication.Repositories
{
    public interface ICustomIntelliSenseRepository
    {
        Task<CustomIntelliSenseDTO?> AddCustomIntelliSenseAsync(CustomIntelliSenseDTO customIntelliSenseDTO);
        Task DeleteCustomIntelliSenseAsync(int Id);
        Task<IEnumerable<CustomIntelliSenseDTO>> GetAllCustomIntelliSensesAsync(int pageNumber, int pageSize);
        Task<IEnumerable<CustomIntelliSenseDTO>> SearchCustomIntelliSensesAsync(string serverSearchTerm);
        Task<CustomIntelliSenseDTO?> GetCustomIntelliSenseByIdAsync(int Id);
        Task<CustomIntelliSenseDTO?> UpdateCustomIntelliSenseAsync(CustomIntelliSenseDTO customIntelliSenseDTO);
        Task<int> GetTotalCountAsync();
    }
}