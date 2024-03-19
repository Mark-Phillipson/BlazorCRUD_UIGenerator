
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;
using SampleApplication.Models;
using SampleApplication.DTOs;

namespace SampleApplication.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly IDbContextFactory<MyDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public LanguageRepository(IDbContextFactory<MyDbContext> contextFactory,IMapper mapper)
        {
            _contextFactory = contextFactory;
            this._mapper = mapper;
        }
		        public async Task<IEnumerable<LanguageDTO>> GetAllLanguagesAsync(int maxRows= 400)
        {
            using var context = _contextFactory.CreateDbContext();
            var Languages= await context.Languages
                //.Where(v => v.?==?)
                //.OrderBy(v => v.?)
                .Take(maxRows)
                .ToListAsync();
            IEnumerable<LanguageDTO> LanguagesDTO = _mapper.Map<List<Language>, IEnumerable<LanguageDTO>>(Languages);
            return LanguagesDTO;
        }
        public async Task<IEnumerable<LanguageDTO>> SearchLanguagesAsync(string serverSearchTerm)
        {
            using var context = _contextFactory.CreateDbContext();
            var Languages= await context.Languages
                //.Where(v => v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //||v.Property!= null  && v.Property.ToLower().Contains(serverSearchTerm.ToLower())
                //)
                //.OrderBy(v => v.?)
                .Take(1000)
                .ToListAsync();
            IEnumerable<LanguageDTO> LanguagesDTO = _mapper.Map<List<Language>, IEnumerable<LanguageDTO>>(Languages);
            return LanguagesDTO;
        }

        public async Task<LanguageDTO?> GetLanguageByIdAsync(int Id)
        {
            using var context = _contextFactory.CreateDbContext();
            var result =await context.Languages.AsNoTracking()
              .FirstOrDefaultAsync(c => c.Id == Id);
            if (result == null) return null;
            LanguageDTO languageDTO=_mapper.Map<Language,LanguageDTO>(result);
            return languageDTO;
        }

        public async Task<LanguageDTO?> AddLanguageAsync(LanguageDTO languageDTO)
        {
            using var context = _contextFactory.CreateDbContext();
            Language language = _mapper.Map<LanguageDTO, Language>(languageDTO);
            var addedEntity = context.Languages.Add(language);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return null;
            }
            LanguageDTO resultDTO=_mapper.Map<Language, LanguageDTO>(language);
            return resultDTO;
        }

        public async Task<LanguageDTO?> UpdateLanguageAsync(LanguageDTO languageDTO)
        {
            Language language=_mapper.Map<LanguageDTO, Language>(languageDTO);
            using (var context = _contextFactory.CreateDbContext())
            {
                var foundLanguage = await context.Languages.AsNoTracking().FirstOrDefaultAsync(e => e.Id == language.Id);

                if (foundLanguage != null)
                {
                    var mappedLanguage = _mapper.Map<Language>(language);
                    context.Languages.Update(mappedLanguage);
                    await context.SaveChangesAsync();
                    LanguageDTO resultDTO = _mapper.Map<Language, LanguageDTO>(mappedLanguage);
                    return resultDTO;
                }
            }
            return null;
        }
        public async Task DeleteLanguageAsync(int Id)
        {
            using var context = _contextFactory.CreateDbContext();
            var foundLanguage = context.Languages.FirstOrDefault(e => e.Id == Id);
            if (foundLanguage == null)
            {
                return;
            }
            context.Languages.Remove(foundLanguage);
            await context.SaveChangesAsync();
        }
    }
}