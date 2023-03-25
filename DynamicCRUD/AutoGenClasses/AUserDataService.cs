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
    public class AUserDataService : IAUserDataService
    {
        private readonly IAUserRepository _aUserRepository;

        public AUserDataService(IAUserRepository aUserRepository)
        {
            this._aUserRepository = aUserRepository;
        }
        public async Task<List<AUserDTO>> GetAllAUsersAsync()
        {
            var AUsers = await _aUserRepository.GetAllAUsersAsync(300);
            return AUsers.ToList();
        }
        public async Task<List<AUserDTO>> SearchAUsersAsync(string serverSearchTerm)
        {
            var AUsers = await _aUserRepository.SearchAUsersAsync(serverSearchTerm);
            return AUsers.ToList();
        }

        public async Task<AUserDTO?> GetAUserById(int UserId)
        {
            var aUser = await _aUserRepository.GetAUserByIdAsync(UserId);
            return aUser;
        }
        public async Task<AUserDTO?> AddAUser(AUserDTO aUserDTO)
        {
            Guard.Against.Null(aUserDTO);
            var result = await _aUserRepository.AddAUserAsync(aUserDTO);
            if (result == null)
            {
                throw new Exception($"Add of aUser failed ID: {aUserDTO.UserId}");
            }
            return result;
        }
        public async Task<AUserDTO> UpdateAUser(AUserDTO aUserDTO, string? username)
        {
            Guard.Against.Null(aUserDTO);
            Guard.Against.Null(username);
            var result = await _aUserRepository.UpdateAUserAsync(aUserDTO);
            if (result == null)
            {
                throw new Exception($"Update of aUser failed ID: {aUserDTO.UserId}");
            }
            return result;
        }

        public async Task DeleteAUser(int UserId)
        {
            await _aUserRepository.DeleteAUserAsync(UserId);
        }
    }
}