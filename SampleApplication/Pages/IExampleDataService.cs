
using SampleApplication.DTOs;

namespace SampleApplication.Services
{
    public interface IExampleDataService
    {
        Task<List<ExampleDTO>> GetAllExamplesAsync( );
        Task<List<ExampleDTO>> SearchExamplesAsync(string serverSearchTerm);
        Task<ExampleDTO?> AddExample(ExampleDTO exampleDTO);
        Task<ExampleDTO?> GetExampleById(int Id);
        Task<ExampleDTO> UpdateExample(ExampleDTO exampleDTO, string? username);
        Task DeleteExample(int Id);
    }
}