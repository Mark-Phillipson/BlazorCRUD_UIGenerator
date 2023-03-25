
using ARM_BlazorServer.DTOs;

namespace ARM_BlazorServer.Repositories
{
    public interface IAUserRepository
    {
        Task<AUserDTO?> AddAUserAsync(AUserDTO aUserDTO);
        Task DeleteAUserAsync(int UserId);
        Task<IEnumerable<AUserDTO>> GetAllAUsersAsync(int maxRows);
        Task<IEnumerable<AUserDTO>> SearchAUsersAsync(string serverSearchTerm);
        Task<AUserDTO?> GetAUserByIdAsync(int UserId);
        Task<AUserDTO?> UpdateAUserAsync(AUserDTO aUserDTO);
    }
}