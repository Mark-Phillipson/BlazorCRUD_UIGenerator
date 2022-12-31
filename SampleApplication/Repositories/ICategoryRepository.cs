
using SampleApplication.DTOs;

namespace SampleApplication.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryDTO?> AddCategoryAsync(CategoryDTO categoryDTO);
        Task DeleteCategoryAsync(int Id);
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(int maxRows);
        Task<IEnumerable<CategoryDTO>> SearchCategoriesAsync(string serverSearchTerm);
        Task<CategoryDTO?> GetCategoryByIdAsync(int Id);
        Task<CategoryDTO?> UpdateCategoryAsync(CategoryDTO categoryDTO);
    }
}