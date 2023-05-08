
using ARM_BlazorServer.DTOs;

namespace ARM_BlazorServer.Services
{
    public interface IATableEditDataService
    {
        Task<List<ATableEditDTO>> GetAllATableEditsAsync( );
        Task<List<ATableEditDTO>> SearchATableEditsAsync(string serverSearchTerm);
        Task<ATableEditDTO?> AddATableEdit(ATableEditDTO aTableEditDTO);
        Task<ATableEditDTO?> GetATableEditById(int TableEditId);
        Task<ATableEditDTO> UpdateATableEdit(ATableEditDTO aTableEditDTO, string? username);
        Task DeleteATableEdit(int TableEditId);
    }
}
