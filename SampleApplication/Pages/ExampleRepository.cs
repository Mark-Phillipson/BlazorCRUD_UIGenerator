
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;
using SampleApplication.Models;
using SampleApplication.DTOs;

namespace SampleApplication.Repositories
{
    public class ExampleRepository : IExampleRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public ExampleRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            this._mapper = mapper;
        }
        public async Task<IEnumerable<ExampleDTO>> GetAllExamplesAsync(int pageNumber, int pageSize)
        {
            using var context = _contextFactory.CreateDbContext();
            var Examples = await context.Examples
                //.Where(v => v.?==?)
                //.OrderBy(v => v.?)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            IEnumerable<ExampleDTO> ExamplesDTO = _mapper.Map<List<Example>, IEnumerable<ExampleDTO>>(Examples);
            return ExamplesDTO;
        }
        public async Task<IEnumerable<ExampleDTO>> SearchExamplesAsync(string serverSearchTerm)
        {
            using var context = _contextFactory.CreateDbContext();
            var Examples = await context.Examples
                //.Where(v => v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //||v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //)
                //.OrderBy(v => v.?)
                .Take(1000)
                .ToListAsync();
            IEnumerable<ExampleDTO> ExamplesDTO = _mapper.Map<List<Example>, IEnumerable<ExampleDTO>>(Examples);
            return ExamplesDTO;
        }

        public async Task<ExampleDTO?> GetExampleByIdAsync(int Id)
        {
            using var context = _contextFactory.CreateDbContext();
            var result = await context.Examples.AsNoTracking()
              .FirstOrDefaultAsync(c => c.Id == Id);
            if (result == null) return null;
            ExampleDTO exampleDTO = _mapper.Map<Example, ExampleDTO>(result);
            return exampleDTO;
        }

        public async Task<ExampleDTO?> AddExampleAsync(ExampleDTO exampleDTO)
        {
            using var context = _contextFactory.CreateDbContext();
            Example example = _mapper.Map<ExampleDTO, Example>(exampleDTO);
            var addedEntity = context.Examples.Add(example);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
            ExampleDTO resultDTO = _mapper.Map<Example, ExampleDTO>(example);
            return resultDTO;
        }

        public async Task<ExampleDTO?> UpdateExampleAsync(ExampleDTO exampleDTO)
        {
            Example example = _mapper.Map<ExampleDTO, Example>(exampleDTO);
            using (var context = _contextFactory.CreateDbContext())
            {
                var foundExample = await context.Examples.AsNoTracking().FirstOrDefaultAsync(e => e.Id == example.Id);

                if (foundExample != null)
                {
                    var mappedExample = _mapper.Map<Example>(example);
                    context.Examples.Update(mappedExample);
                    await context.SaveChangesAsync();
                    ExampleDTO resultDTO = _mapper.Map<Example, ExampleDTO>(mappedExample);
                    return resultDTO;
                }
            }
            return null;
        }
        public async Task DeleteExampleAsync(int Id)
        {
            using var context = _contextFactory.CreateDbContext();
            var foundExample = context.Examples.FirstOrDefault(e => e.Id == Id);
            if (foundExample == null)
            {
                return;
            }
            context.Examples.Remove(foundExample);
            await context.SaveChangesAsync();
        }
        public async Task<int> GetTotalCountAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Examples.CountAsync();
        }

    }
}