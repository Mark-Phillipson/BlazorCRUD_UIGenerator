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
    public class LanguageDataService : ILanguageDataService
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguageDataService(ILanguageRepository languageRepository)
        {
            this._languageRepository = languageRepository;
        }
        public async Task<List<LanguageDTO>> GetAllLanguagesAsync()
        {
            var Languages = await _languageRepository.GetAllLanguagesAsync(300);
            return Languages.ToList();
        }
        public async Task<List<LanguageDTO>> SearchLanguagesAsync(string serverSearchTerm)
        {
            var Languages = await _languageRepository.SearchLanguagesAsync(serverSearchTerm);
            return Languages.ToList();
        }

        public async Task<LanguageDTO?> GetLanguageById(int Id)
        {
            var language = await _languageRepository.GetLanguageByIdAsync(Id);
            return language;
        }
        public async Task<LanguageDTO?> AddLanguage(LanguageDTO languageDTO)
        {
            Guard.Against.Null(languageDTO);
            var result = await _languageRepository.AddLanguageAsync(languageDTO);
            if (result == null)
            {
                throw new Exception($"Add of language failed ID: {languageDTO.Id}");
            }
            return result;
        }
        public async Task<LanguageDTO> UpdateLanguage(LanguageDTO languageDTO, string? username)
        {
            Guard.Against.Null(languageDTO);
            Guard.Against.Null(username);
            var result = await _languageRepository.UpdateLanguageAsync(languageDTO);
            if (result == null)
            {
                throw new Exception($"Update of language failed ID: {languageDTO.Id}");
            }
            return result;
        }

        public async Task DeleteLanguage(int Id)
        {
            await _languageRepository.DeleteLanguageAsync(Id);
        }
    }
}