
using ARM_BlazorServer.DTOs;

namespace ARM_BlazorServer.Services
{
    public interface IAUserDataService
    {
        Task<List<AUserDTO>> GetAllAUsersAsync( );
        Task<List<AUserDTO>> SearchAUsersAsync(string serverSearchTerm);
        Task<AUserDTO?> AddAUser(AUserDTO aUserDTO);
        Task<AUserDTO?> GetAUserById(int UserId);
        Task<AUserDTO> UpdateAUser(AUserDTO aUserDTO, string? username);
        Task DeleteAUser(int UserId);
    }
}
