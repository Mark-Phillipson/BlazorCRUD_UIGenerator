
using SampleApplication.DTOs;

namespace SampleApplication.Repositories
{
    public interface IGeneralLookupRepository
    {
        Task<GeneralLookupDTO?> AddGeneralLookupAsync(GeneralLookupDTO generalLookupDTO);
        Task DeleteGeneralLookupAsync(int Id);
        Task<IEnumerable<GeneralLookupDTO>> GetAllGeneralLookupsAsync(int maxRows);
        Task<IEnumerable<GeneralLookupDTO>> SearchGeneralLookupsAsync(string serverSearchTerm);
        Task<GeneralLookupDTO?> GetGeneralLookupByIdAsync(int Id);
        Task<GeneralLookupDTO?> UpdateGeneralLookupAsync(GeneralLookupDTO generalLookupDTO);
    }
}