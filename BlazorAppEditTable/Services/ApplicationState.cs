using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppEditTable.Services
{
    public class ApplicationState
    {
        public IEnumerable<DynamicDatabaseColumn>? Columns = null;

        public DataTable? DataTable = null;

        public string? TableName { get; set; }
        public string? LookupTableName { get; set; }
        public string Database { get; set; } = "VoiceLauncher";
        public string? InitialFilter { get; set; }
        public string? InitialFilterTableName { get; set; }
        public string? PrimaryKeyName { get; set; }
        public string? LookupPrimaryKeyName { get; set; }
        public string? PrimaryKeyDataType { get; set; }
        public string? LookupPrimaryKeyDataType { get; set; }
        public bool IsAddNew { get; set; } = false;
        public int MultiResultsLimit { get; set; } = 500;
    }
}
