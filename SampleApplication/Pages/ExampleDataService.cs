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
    public class ExampleDataService : IExampleDataService
    {
        private readonly IExampleRepository _exampleRepository;

        public ExampleDataService(IExampleRepository exampleRepository)
        {
            this._exampleRepository = exampleRepository;
        }
        public async Task<List<ExampleDTO>> GetAllExamplesAsync(int pageNumber, int pageSize)
        {
            var Examples = await _exampleRepository.GetAllExamplesAsync( pageNumber, pageSize);
            return Examples.ToList();
        }
        public async Task<List<ExampleDTO>> SearchExamplesAsync(string serverSearchTerm)
        {
            var Examples = await _exampleRepository.SearchExamplesAsync(serverSearchTerm);
            return Examples.ToList();
        }

        public async Task<ExampleDTO?> GetExampleById(int Id)
        {
            var example = await _exampleRepository.GetExampleByIdAsync(Id);
            return example;
        }
        public async Task<ExampleDTO?> AddExample(ExampleDTO exampleDTO)
        {
            Guard.Against.Null(exampleDTO);
            var result = await _exampleRepository.AddExampleAsync(exampleDTO);
            if (result == null)
            {
                throw new Exception($"Add of example failed ID: {exampleDTO.Id}");
            }
            return result;
        }
        public async Task<ExampleDTO> UpdateExample(ExampleDTO exampleDTO, string? username)
        {
            Guard.Against.Null(exampleDTO);
            Guard.Against.Null(username);
            var result = await _exampleRepository.UpdateExampleAsync(exampleDTO);
            if (result == null)
            {
                throw new Exception($"Update of example failed ID: {exampleDTO.Id}");
            }
            return result;
        }

        public async Task DeleteExample(int Id)
        {
            await _exampleRepository.DeleteExampleAsync(Id);
        }
        public async Task<int> GetTotalCount()
        {
            int result = await _exampleRepository.GetTotalCountAsync();
            return result;
        }
    }
}