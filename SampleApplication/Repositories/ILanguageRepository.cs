
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using SampleApplication.DTOs;

namespace SampleApplication.Repositories
{
    public interface ILanguageRepository
    {
        Task<LanguageDTO?> AddLanguageAsync(LanguageDTO languageDTO);
        Task DeleteLanguageAsync(int Id);
        Task<IEnumerable<LanguageDTO>> GetAllLanguagesAsync(int pageNumber, int pageSize, string? serverSearchTerm);
        Task<IEnumerable<LanguageDTO>> SearchLanguagesAsync(string serverSearchTerm);
        Task<LanguageDTO?> GetLanguageByIdAsync(int Id);
        Task<LanguageDTO?> UpdateLanguageAsync(LanguageDTO languageDTO);
        Task<int> GetTotalCountAsync();
    }
}