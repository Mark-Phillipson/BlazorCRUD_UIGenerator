
using SampleApplication.DTOs;

namespace SampleApplication.Services
{
    public interface IGeneralLookupDataService
    {
        Task<List<GeneralLookupDTO>> GetAllGeneralLookupsAsync( );
        Task<List<GeneralLookupDTO>> SearchGeneralLookupsAsync(string serverSearchTerm);
        Task<GeneralLookupDTO?> AddGeneralLookup(GeneralLookupDTO generalLookupDTO);
        Task<GeneralLookupDTO?> GetGeneralLookupById(int Id);
        Task<GeneralLookupDTO> UpdateGeneralLookup(GeneralLookupDTO generalLookupDTO, string? username);
        Task DeleteGeneralLookup(int Id);
    }
}
