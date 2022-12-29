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
    public class GeneralLookupDataService : IGeneralLookupDataService
    {
        private readonly IGeneralLookupRepository _generalLookupRepository;

        public GeneralLookupDataService(IGeneralLookupRepository generalLookupRepository)
        {
            this._generalLookupRepository = generalLookupRepository;
        }
        public async Task<List<GeneralLookupDTO>> GetAllGeneralLookupsAsync()
        {
            var GeneralLookups = await _generalLookupRepository.GetAllGeneralLookupsAsync(300);
            return GeneralLookups.ToList();
        }
        public async Task<List<GeneralLookupDTO>> SearchGeneralLookupsAsync(string serverSearchTerm)
        {
            var GeneralLookups = await _generalLookupRepository.SearchGeneralLookupsAsync(serverSearchTerm);
            return GeneralLookups.ToList();
        }

        public async Task<GeneralLookupDTO?> GetGeneralLookupById(int Id)
        {
            var generalLookup = await _generalLookupRepository.GetGeneralLookupByIdAsync(Id);
            return generalLookup;
        }
        public async Task<GeneralLookupDTO?> AddGeneralLookup(GeneralLookupDTO generalLookupDTO)
        {
            Guard.Against.Null(generalLookupDTO);
            var result = await _generalLookupRepository.AddGeneralLookupAsync(generalLookupDTO);
            if (result == null)
            {
                throw new Exception($"Add of generalLookup failed ID: {generalLookupDTO.Id}");
            }
            return result;
        }
        public async Task<GeneralLookupDTO> UpdateGeneralLookup(GeneralLookupDTO generalLookupDTO, string? username)
        {
            Guard.Against.Null(generalLookupDTO);
            Guard.Against.Null(username);
            var result = await _generalLookupRepository.UpdateGeneralLookupAsync(generalLookupDTO);
            if (result == null)
            {
                throw new Exception($"Update of generalLookup failed ID: {generalLookupDTO.Id}");
            }
            return result;
        }

        public async Task DeleteGeneralLookup(int Id)
        {
            await _generalLookupRepository.DeleteGeneralLookupAsync(Id);
        }
    }
}