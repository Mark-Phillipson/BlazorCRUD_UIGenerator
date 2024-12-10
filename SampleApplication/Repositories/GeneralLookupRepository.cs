
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;
using SampleApplication.Models;
using SampleApplication.DTOs;

namespace SampleApplication.Repositories
{
    public class GeneralLookupRepository : IGeneralLookupRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public GeneralLookupRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            this._mapper = mapper;
        }
        public async Task<IEnumerable<GeneralLookupDTO>> GetAllGeneralLookupsAsync(int maxRows = 400)
        {
            using var context = _contextFactory.CreateDbContext();
            var GeneralLookups = await context.GeneralLookups
                //.Where(v => v.?==?)
                //.OrderBy(v => v.?)
                .Take(maxRows)
                .ToListAsync();
            IEnumerable<GeneralLookupDTO> GeneralLookupsDTO = _mapper.Map<List<GeneralLookup>, IEnumerable<GeneralLookupDTO>>(GeneralLookups);
            return GeneralLookupsDTO;
        }
        public async Task<IEnumerable<GeneralLookupDTO>> SearchGeneralLookupsAsync(string serverSearchTerm)
        {
            using var context = _contextFactory.CreateDbContext();
            var GeneralLookups = await context.GeneralLookups
                //.Where(v => v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //||v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //)
                //.OrderBy(v => v.?)
                .Take(1000)
                .ToListAsync();
            IEnumerable<GeneralLookupDTO> GeneralLookupsDTO = _mapper.Map<List<GeneralLookup>, IEnumerable<GeneralLookupDTO>>(GeneralLookups);
            return GeneralLookupsDTO;
        }

        public async Task<GeneralLookupDTO?> GetGeneralLookupByIdAsync(int Id)
        {
            using var context = _contextFactory.CreateDbContext();
            var result = await context.GeneralLookups.AsNoTracking()
              .FirstOrDefaultAsync(c => c.Id == Id);
            if (result == null) return null;
            GeneralLookupDTO generalLookupDTO = _mapper.Map<GeneralLookup, GeneralLookupDTO>(result);
            return generalLookupDTO;
        }

        public async Task<GeneralLookupDTO?> AddGeneralLookupAsync(GeneralLookupDTO generalLookupDTO)
        {
            using var context = _contextFactory.CreateDbContext();
            GeneralLookup generalLookup = _mapper.Map<GeneralLookupDTO, GeneralLookup>(generalLookupDTO);
            var addedEntity = context.GeneralLookups.Add(generalLookup);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
            GeneralLookupDTO resultDTO = _mapper.Map<GeneralLookup, GeneralLookupDTO>(generalLookup);
            return resultDTO;
        }

        public async Task<GeneralLookupDTO?> UpdateGeneralLookupAsync(GeneralLookupDTO generalLookupDTO)
        {
            GeneralLookup generalLookup = _mapper.Map<GeneralLookupDTO, GeneralLookup>(generalLookupDTO);
            using (var context = _contextFactory.CreateDbContext())
            {
                var foundGeneralLookup = await context.GeneralLookups.AsNoTracking().FirstOrDefaultAsync(e => e.Id == generalLookup.Id);

                if (foundGeneralLookup != null)
                {
                    var mappedGeneralLookup = _mapper.Map<GeneralLookup>(generalLookup);
                    context.GeneralLookups.Update(mappedGeneralLookup);
                    await context.SaveChangesAsync();
                    GeneralLookupDTO resultDTO = _mapper.Map<GeneralLookup, GeneralLookupDTO>(mappedGeneralLookup);
                    return resultDTO;
                }
            }
            return null;
        }
        public async Task DeleteGeneralLookupAsync(int Id)
        {
            using var context = _contextFactory.CreateDbContext();
            var foundGeneralLookup = context.GeneralLookups.FirstOrDefault(e => e.Id == Id);
            if (foundGeneralLookup == null)
            {
                return;
            }
            context.GeneralLookups.Remove(foundGeneralLookup);
            await context.SaveChangesAsync();
        }
    }
}