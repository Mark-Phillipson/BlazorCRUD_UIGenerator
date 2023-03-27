using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace BlazorAppEditTable.Services
{
    public class DynamicTableDataService : IDynamicTableDataService
    {
        private readonly IDynamicTableRepository _dynamicTableRepository;

        public DynamicTableDataService(IDynamicTableRepository dynamicTableRepository)
        {
            this._dynamicTableRepository = dynamicTableRepository;
        }

        public  bool  AddDynamicTable(DataRow dataRow,ApplicationState applicationState)
        {
            var result = _dynamicTableRepository.AddDynamicTable(dataRow, applicationState);
            return result;
        }

        public  bool  DeleteDynamicTable(object? id,ApplicationState applicationState)
        {
            var result = _dynamicTableRepository.DeleteDynamicTable(id,applicationState);
            return result;
        }

        public DataTable GetAllDynamicTables(string? sql = null)
        {
            var result = _dynamicTableRepository.GetAllDynamicTables(sql, 500);
            return result;
        }

        public IEnumerable<DynamicDatabaseColumn>? GetColumnNames(string sql)
        {
            var result = _dynamicTableRepository.GetColumnNames(sql);
            return result;
        }

        public Task<DataRow?> GetDynamicTableById(object id)
        {
            throw new NotImplementedException();
        }

        public Task<DataTable> SearchDynamicTablesAsync(string serverSearchTerm)
        {
            throw new NotImplementedException();
        }

        public bool UpdateDynamicTable(DataRow dataRow, ApplicationState applicationState)
        {
            var result = _dynamicTableRepository.UpdateDynamicTableAsync(dataRow, applicationState);
            return result;
        }
        public List<DynamicDatabaseTable> GetListOfTables()
        {
            var result = _dynamicTableRepository.GetListOfTables();
            return result;
        }
    }
}