using Ardalis.GuardClauses;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARM_BlazorServer.Repositories;
using ARM_BlazorServer.DTOs;


namespace ARM_BlazorServer.Services
{
    public class ATableEditDataService : IATableEditDataService
    {
        private readonly IATableEditRepository _aTableEditRepository;

        public ATableEditDataService(IATableEditRepository aTableEditRepository)
        {
            this._aTableEditRepository = aTableEditRepository;
        }
        public async Task<List<ATableEditDTO>> GetAllATableEditsAsync()
        {
            var ATableEdits = await _aTableEditRepository.GetAllATableEditsAsync(300);
            return ATableEdits.ToList();
        }
        public async Task<List<ATableEditDTO>> SearchATableEditsAsync(string serverSearchTerm)
        {
            var ATableEdits = await _aTableEditRepository.SearchATableEditsAsync(serverSearchTerm);
            return ATableEdits.ToList();
        }

        public async Task<ATableEditDTO?> GetATableEditById(int TableEditId)
        {
            var aTableEdit = await _aTableEditRepository.GetATableEditByIdAsync(TableEditId);
            return aTableEdit;
        }
        public async Task<ATableEditDTO?> AddATableEdit(ATableEditDTO aTableEditDTO)
        {
            Guard.Against.Null(aTableEditDTO);
            var result = await _aTableEditRepository.AddATableEditAsync(aTableEditDTO);
            if (result == null)
            {
                throw new Exception($"Add of aTableEdit failed ID: {aTableEditDTO.TableEditId}");
            }
            return result;
        }
        public async Task<ATableEditDTO> UpdateATableEdit(ATableEditDTO aTableEditDTO, string? username)
        {
            Guard.Against.Null(aTableEditDTO);
            Guard.Against.Null(username);
            var result = await _aTableEditRepository.UpdateATableEditAsync(aTableEditDTO);
            if (result == null)
            {
                throw new Exception($"Update of aTableEdit failed ID: {aTableEditDTO.TableEditId}");
            }
            return result;
        }

        public async Task DeleteATableEdit(int TableEditId)
        {
            await _aTableEditRepository.DeleteATableEditAsync(TableEditId);
        }
    }
}