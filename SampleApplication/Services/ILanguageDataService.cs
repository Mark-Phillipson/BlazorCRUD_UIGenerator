
using SampleApplication.DTOs;

namespace SampleApplication.Services
{
    public interface ILanguageDataService
    {
        Task<List<LanguageDTO>> GetAllLanguagesAsync( );
        Task<List<LanguageDTO>> SearchLanguagesAsync(string serverSearchTerm);
        Task<LanguageDTO?> AddLanguage(LanguageDTO languageDTO);
        Task<LanguageDTO?> GetLanguageById(int Id);
        Task<LanguageDTO> UpdateLanguage(LanguageDTO languageDTO, string? username);
        Task DeleteLanguage(int Id);
    }
}
