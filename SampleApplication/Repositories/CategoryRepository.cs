
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;
using SampleApplication.Models;
using SampleApplication.DTOs;

namespace SampleApplication.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public CategoryRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            this._mapper = mapper;
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(int maxRows = 400)
        {
            using var context = _contextFactory.CreateDbContext();
            var Categories = await context.Categories
                //.Where(v => v.?==?)
                //.OrderBy(v => v.?)
                .Take(maxRows)
                .ToListAsync();
            IEnumerable<CategoryDTO> CategoriesDTO = _mapper.Map<List<Category>, IEnumerable<CategoryDTO>>(Categories);
            return CategoriesDTO;
        }
        public async Task<IEnumerable<CategoryDTO>> SearchCategoriesAsync(string serverSearchTerm)
        {
            using var context = _contextFactory.CreateDbContext();
            var Categories = await context.Categories
                //.Where(v => v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //||v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //)
                //.OrderBy(v => v.?)
                .Take(1000)
                .ToListAsync();
            IEnumerable<CategoryDTO> CategoriesDTO = _mapper.Map<List<Category>, IEnumerable<CategoryDTO>>(Categories);
            return CategoriesDTO;
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int Id)
        {
            using var context = _contextFactory.CreateDbContext();
            var result = await context.Categories.AsNoTracking()
              .FirstOrDefaultAsync(c => c.Id == Id);
            if (result == null) return null;
            CategoryDTO categoryDTO = _mapper.Map<Category, CategoryDTO>(result);
            return categoryDTO;
        }

        public async Task<CategoryDTO?> AddCategoryAsync(CategoryDTO categoryDTO)
        {
            using var context = _contextFactory.CreateDbContext();
            Category category = _mapper.Map<CategoryDTO, Category>(categoryDTO);
            var addedEntity = context.Categories.Add(category);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
            CategoryDTO resultDTO = _mapper.Map<Category, CategoryDTO>(category);
            return resultDTO;
        }

        public async Task<CategoryDTO?> UpdateCategoryAsync(CategoryDTO categoryDTO)
        {
            Category category = _mapper.Map<CategoryDTO, Category>(categoryDTO);
            using (var context = _contextFactory.CreateDbContext())
            {
                var foundCategory = await context.Categories.AsNoTracking().FirstOrDefaultAsync(e => e.Id == category.Id);

                if (foundCategory != null)
                {
                    var mappedCategory = _mapper.Map<Category>(category);
                    context.Categories.Update(mappedCategory);
                    await context.SaveChangesAsync();
                    CategoryDTO resultDTO = _mapper.Map<Category, CategoryDTO>(mappedCategory);
                    return resultDTO;
                }
            }
            return null;
        }
        public async Task DeleteCategoryAsync(int Id)
        {
            using var context = _contextFactory.CreateDbContext();
            var foundCategory = context.Categories.FirstOrDefault(e => e.Id == Id);
            if (foundCategory == null)
            {
                return;
            }
            context.Categories.Remove(foundCategory);
            await context.SaveChangesAsync();
        }
    }
}