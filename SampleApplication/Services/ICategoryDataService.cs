
using SampleApplication.DTOs;

namespace SampleApplication.Services
{
    public interface ICategoryDataService
    {
        Task<List<CategoryDTO>> GetAllCategoriesAsync( );
        Task<List<CategoryDTO>> SearchCategoriesAsync(string serverSearchTerm);
        Task<CategoryDTO?> AddCategory(CategoryDTO categoryDTO);
        Task<CategoryDTO?> GetCategoryById(int Id);
        Task<CategoryDTO> UpdateCategory(CategoryDTO categoryDTO, string? username);
        Task DeleteCategory(int Id);
    }
}
