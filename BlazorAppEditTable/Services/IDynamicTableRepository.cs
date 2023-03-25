
using System.Data;

namespace BlazorAppEditTable.Services
{
    public interface IDynamicTableRepository
    {
        bool AddDynamicTable(DataRow dataRow, ApplicationState mvcApplicationState);
        bool DeleteDynamicTable(object? id, ApplicationState mvcApplicationState);
        DataTable GetAllDynamicTables(string? sql = null, int maxRows = 500);
        Task<DataTable> SearchDynamicTablesAsync(string serverSearchTerm);
        Task<DataRow?> GetDynamicTableByIdAsync(object id);
        bool UpdateDynamicTableAsync(DataRow dataRow, ApplicationState mvcApplicationState);
        IEnumerable<DynamicDatabaseColumn>? GetColumnNames(string sql);
    }
}