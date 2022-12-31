using Ardalis.GuardClauses;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleApplication.Repositories;
using SampleApplication.DTOs;


namespace SampleApplication.Services
{
    public class CategoryDataService : ICategoryDataService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryDataService(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }
        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var Categories = await _categoryRepository.GetAllCategoriesAsync(300);
            return Categories.ToList();
        }
        public async Task<List<CategoryDTO>> SearchCategoriesAsync(string serverSearchTerm)
        {
            var Categories = await _categoryRepository.SearchCategoriesAsync(serverSearchTerm);
            return Categories.ToList();
        }

        public async Task<CategoryDTO?> GetCategoryById(int Id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(Id);
            return category;
        }
        public async Task<CategoryDTO?> AddCategory(CategoryDTO categoryDTO)
        {
            Guard.Against.Null(categoryDTO);
            var result = await _categoryRepository.AddCategoryAsync(categoryDTO);
            if (result == null)
            {
                throw new Exception($"Add of category failed ID: {categoryDTO.Id}");
            }
            return result;
        }
        public async Task<CategoryDTO> UpdateCategory(CategoryDTO categoryDTO, string? username)
        {
            Guard.Against.Null(categoryDTO);
            Guard.Against.Null(username);
            var result = await _categoryRepository.UpdateCategoryAsync(categoryDTO);
            if (result == null)
            {
                throw new Exception($"Update of category failed ID: {categoryDTO.Id}");
            }
            return result;
        }

        public async Task DeleteCategory(int Id)
        {
            await _categoryRepository.DeleteCategoryAsync(Id);
        }
    }
}