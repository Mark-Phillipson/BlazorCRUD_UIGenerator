
using ARM_BlazorServer.DTOs;

namespace ARM_BlazorServer.Repositories
{
    public interface IATableEditRepository
    {
        Task<ATableEditDTO?> AddATableEditAsync(ATableEditDTO aTableEditDTO);
        Task DeleteATableEditAsync(int TableEditId);
        Task<IEnumerable<ATableEditDTO>> GetAllATableEditsAsync(int maxRows);
        Task<IEnumerable<ATableEditDTO>> SearchATableEditsAsync(string serverSearchTerm);
        Task<ATableEditDTO?> GetATableEditByIdAsync(int TableEditId);
        Task<ATableEditDTO?> UpdateATableEditAsync(ATableEditDTO aTableEditDTO);
    }
}