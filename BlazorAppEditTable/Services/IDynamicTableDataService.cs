using System.Data;

namespace BlazorAppEditTable.Services
{
    public interface IDynamicTableDataService
    {
        DataTable GetAllDynamicTables(string? sql);
        Task<DataTable> SearchDynamicTablesAsync(string serverSearchTerm);
        bool AddDynamicTable(DataRow dataRow, ApplicationState applicationState);
        Task<DataRow?> GetDynamicTableById(object id);
        bool UpdateDynamicTable(DataRow dataRow, ApplicationState applicationState);
        bool DeleteDynamicTable(object? id, ApplicationState applicationState);
        IEnumerable<DynamicDatabaseColumn>? GetColumnNames(string sql);
        List<DynamicDatabaseTable> GetListOfTables();
    }
}
