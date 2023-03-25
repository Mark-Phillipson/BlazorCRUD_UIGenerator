

using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;

using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;

namespace BlazorAppEditTable.Services
{
    public class DynamicTableRepository : IDynamicTableRepository
    {
        private readonly DatabaseMetaDataService _databaseMetaDataService;
        private readonly ApplicationState _mvcApplicationState;
        private readonly DynamicUpdates _dynamicUpdates;

        public DynamicTableRepository(DatabaseMetaDataService databaseMetaDataService, ApplicationState mvcApplicationState, DynamicUpdates dynamicUpdates)
        {
            _databaseMetaDataService = databaseMetaDataService;
            _mvcApplicationState = mvcApplicationState;
            _dynamicUpdates = dynamicUpdates;
        }

        public bool AddDynamicTable(DataRow dataRow, ApplicationState mvcApplicationState)
        {
            var result = _dynamicUpdates.AddRecord(dataRow, mvcApplicationState);
            return result;
        }
        public bool DeleteDynamicTable(object? id, ApplicationState mvcApplicationState)
        {
            if (id != null && id.ToString() != null)
            {
                var result = _dynamicUpdates.DeleteRecord(id.ToString(), mvcApplicationState);
                return result;
            }
            return false;
        }

        public DataTable GetAllDynamicTables(string? sql = null, int maxRows = 500)
        {
            if (sql == null || sql.Length == 0)
            {
                sql = $"SELECT TOP {maxRows} * FROM [{_mvcApplicationState.TableName}]";
            }
            var result = _databaseMetaDataService.GetDataIntoDataTable(sql, null, "ARM_CORE",maxRows);
            return result;
        }

        public IEnumerable<DynamicDatabaseColumn>? GetColumnNames(string sql)
        {
            var result = _databaseMetaDataService.GetColumnNamesFromSql(sql);
            return result;
        }

        public Task<DataRow?> GetDynamicTableByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<DataTable> SearchDynamicTablesAsync(string serverSearchTerm)
        {
            throw new NotImplementedException();
        }

        public bool UpdateDynamicTableAsync(DataRow dataRow, ApplicationState mvcApplicationState)
        {
            var result = _dynamicUpdates.UpdateTable(dataRow, _mvcApplicationState);
            return result;
        }
    }
}