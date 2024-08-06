
using SampleApplication.DTOs;

namespace SampleApplication.Services
{
    public interface ICustomIntelliSenseDataService
    {
        Task<List<CustomIntelliSenseDTO>> GetAllCustomIntelliSensesAsync(int pageNumber, int pageSize);
        Task<List<CustomIntelliSenseDTO>> SearchCustomIntelliSensesAsync(string serverSearchTerm);
        Task<CustomIntelliSenseDTO?> AddCustomIntelliSense(CustomIntelliSenseDTO customIntelliSenseDTO);
        Task<CustomIntelliSenseDTO?> GetCustomIntelliSenseById(int Id);
        Task<CustomIntelliSenseDTO> UpdateCustomIntelliSense(CustomIntelliSenseDTO customIntelliSenseDTO, string? username);
        Task DeleteCustomIntelliSense(int Id);
        Task<int> GetTotalCount();
    }
}