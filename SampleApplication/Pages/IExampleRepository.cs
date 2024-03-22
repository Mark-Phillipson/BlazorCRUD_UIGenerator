
using SampleApplication.DTOs;

namespace SampleApplication.Repositories
{
    public interface IExampleRepository
    {
        Task<ExampleDTO?> AddExampleAsync(ExampleDTO exampleDTO);
        Task DeleteExampleAsync(int Id);
        Task<IEnumerable<ExampleDTO>> GetAllExamplesAsync(int pageNumber, int pageSize);
        Task<IEnumerable<ExampleDTO>> SearchExamplesAsync(string serverSearchTerm);
        Task<ExampleDTO?> GetExampleByIdAsync(int Id);
        Task<ExampleDTO?> UpdateExampleAsync(ExampleDTO exampleDTO);
        Task<int> GetTotalCountAsync();
    }
}