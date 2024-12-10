
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;
using SampleApplication.Models;
using SampleApplication.DTOs;

namespace SampleApplication.Repositories
{
    public class CustomIntelliSenseRepository : ICustomIntelliSenseRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public CustomIntelliSenseRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            this._mapper = mapper;
        }
        public async Task<IEnumerable<CustomIntelliSenseDTO>> GetAllCustomIntelliSensesAsync(int pageNumber, int pageSize)
        {
            using var context = _contextFactory.CreateDbContext();
            var CustomIntelliSenses = await context.CustomIntelliSenses
                //.Where(v => v.?==?)
                //.OrderBy(v => v.?)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            IEnumerable<CustomIntelliSenseDTO> CustomIntelliSensesDTO = _mapper.Map<List<CustomIntelliSense>, IEnumerable<CustomIntelliSenseDTO>>(CustomIntelliSenses);
            return CustomIntelliSensesDTO;
        }
        public async Task<IEnumerable<CustomIntelliSenseDTO>> SearchCustomIntelliSensesAsync(string serverSearchTerm)
        {
            using var context = _contextFactory.CreateDbContext();
            var CustomIntelliSenses = await context.CustomIntelliSenses
                //.Where(v => v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //||v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //)
                //.OrderBy(v => v.?)
                .Take(1000)
                .ToListAsync();
            IEnumerable<CustomIntelliSenseDTO> CustomIntelliSensesDTO = _mapper.Map<List<CustomIntelliSense>, IEnumerable<CustomIntelliSenseDTO>>(CustomIntelliSenses);
            return CustomIntelliSensesDTO;
        }

        public async Task<CustomIntelliSenseDTO?> GetCustomIntelliSenseByIdAsync(int Id)
        {
            using var context = _contextFactory.CreateDbContext();
            var result = await context.CustomIntelliSenses.AsNoTracking()
              .FirstOrDefaultAsync(c => c.Id == Id);
            if (result == null) return null;
            CustomIntelliSenseDTO customIntelliSenseDTO = _mapper.Map<CustomIntelliSense, CustomIntelliSenseDTO>(result);
            return customIntelliSenseDTO;
        }

        public async Task<CustomIntelliSenseDTO?> AddCustomIntelliSenseAsync(CustomIntelliSenseDTO customIntelliSenseDTO)
        {
            using var context = _contextFactory.CreateDbContext();
            CustomIntelliSense customIntelliSense = _mapper.Map<CustomIntelliSenseDTO, CustomIntelliSense>(customIntelliSenseDTO);
            var addedEntity = context.CustomIntelliSenses.Add(customIntelliSense);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
            CustomIntelliSenseDTO resultDTO = _mapper.Map<CustomIntelliSense, CustomIntelliSenseDTO>(customIntelliSense);
            return resultDTO;
        }

        public async Task<CustomIntelliSenseDTO?> UpdateCustomIntelliSenseAsync(CustomIntelliSenseDTO customIntelliSenseDTO)
        {
            CustomIntelliSense customIntelliSense = _mapper.Map<CustomIntelliSenseDTO, CustomIntelliSense>(customIntelliSenseDTO);
            using (var context = _contextFactory.CreateDbContext())
            {
                var foundCustomIntelliSense = await context.CustomIntelliSenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id == customIntelliSense.Id);

                if (foundCustomIntelliSense != null)
                {
                    var mappedCustomIntelliSense = _mapper.Map<CustomIntelliSense>(customIntelliSense);
                    context.CustomIntelliSenses.Update(mappedCustomIntelliSense);
                    await context.SaveChangesAsync();
                    CustomIntelliSenseDTO resultDTO = _mapper.Map<CustomIntelliSense, CustomIntelliSenseDTO>(mappedCustomIntelliSense);
                    return resultDTO;
                }
            }
            return null;
        }
        public async Task DeleteCustomIntelliSenseAsync(int Id)
        {
            using var context = _contextFactory.CreateDbContext();
            var foundCustomIntelliSense = context.CustomIntelliSenses.FirstOrDefault(e => e.Id == Id);
            if (foundCustomIntelliSense == null)
            {
                return;
            }
            context.CustomIntelliSenses.Remove(foundCustomIntelliSense);
            await context.SaveChangesAsync();
        }
        public async Task<int> GetTotalCountAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.CustomIntelliSenses.CountAsync();
        }

    }
}