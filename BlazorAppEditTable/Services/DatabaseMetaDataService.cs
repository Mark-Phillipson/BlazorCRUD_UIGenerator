using Ardalis.GuardClauses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace BlazorAppEditTable.Services
{
    public class DatabaseMetaDataService
    {
        private readonly IConfiguration _configuration;
        private string _connectionString;
        private readonly TableStructureServices _tableStructureServices;
        private readonly ApplicationState _mvcApplicationState;
        private readonly ILogger<DatabaseMetaDataService> _logger;

        public DatabaseMetaDataService(IConfiguration configuration, TableStructureServices tableStructureServices, ApplicationState mvcApplicationState, ILogger<DatabaseMetaDataService> logger)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";
            _tableStructureServices = tableStructureServices;
            _mvcApplicationState = mvcApplicationState;
            _logger = logger;
        }
        public IEnumerable<DynamicDatabaseColumn>? GetColumnNamesFromSql(string sql)
        {
            Guard.Against.Null(sql);
            var columns = new List<DynamicDatabaseColumn>();
            DataTable dataTable;
            using (var sqlCon = new SqlConnection(_connectionString))
            {
                sqlCon.Open();
                var sqlCmd = sqlCon.CreateCommand();

                sqlCmd.CommandText = $"SET FMTONLY ON; {sql}; SET FMTONLY OFF"; // No data wanted, only schema  
                sqlCmd.CommandType = CommandType.Text;
                var reader = sqlCmd.ExecuteReader(CommandBehavior.SingleRow);
                dataTable = reader.GetSchemaTable();
            }

            foreach (DataRow row in dataTable.Rows)
            {
                DynamicDatabaseColumn column = new();
                column.ColumnName = row.Field<string>("ColumnName") ?? "ColumnNameNotFound";
                column.ColumnSize = row.Field<int>("ColumnSize");
                //var type = (SqlDbType)(int)row["ProviderType"];
                column.DataType = row.Field<string>("DataTypeName");
                column.Required = !row.Field<bool>("AllowDBNull");
                column.IsAutoIncrement = row.Field<bool>("IsAutoIncrement");
                column.IsIdentity = row.Field<bool>("IsIdentity");
                column.IsKey = row.Field<bool?>("IsKey") ?? false;
                column.Label = StringHelperService.AddSpacesToSentence(column.ColumnName ?? "");
                column.PropertyName = StringHelperService.RemoveUnsupportedCharacters(column.ColumnName ?? "").Replace("ID", "Id");
                columns.Add(column);
            }
            return columns.ToList();
        }

        public IEnumerable<DynamicDatabaseColumn> GetColumnNamesFromTable(string tableName)
        {
            Guard.Against.Null(tableName);
            var columns = new List<DynamicDatabaseColumn>();
            List<string>? styles = new List<string>() { "STD" };
            using (var sqlCon = new SqlConnection(_connectionString))
            {
                sqlCon.Open();
                var sqlCmd = sqlCon.CreateCommand();
                tableName = IncludeSquareBrackets(tableName);
                DataTable dataTable;
                SqlDataReader? sqlDataReader;
                if (IsThisATable(tableName))
                {
                    sqlCmd.CommandText = $"SET FMTONLY ON; select distinct * from {tableName}; SET FMTONLY OFF"; // No data wanted, only schema  
                    sqlCmd.CommandType = CommandType.Text;
                    sqlDataReader = sqlCmd.ExecuteReader(CommandBehavior.KeyInfo);
                    dataTable = sqlDataReader.GetSchemaTable();
                }
                else
                {
                    sqlCmd.CommandText = $"select top 1 * from {tableName};"; // This is a SQL view not a table
                    sqlCmd.CommandType = CommandType.Text;
                    sqlDataReader = sqlCmd.ExecuteReader(CommandBehavior.SingleRow);
                    dataTable = sqlDataReader.GetSchemaTable();
                }
                var tableStructures = _tableStructureServices.GetData(tableName, "ARM_CORE");
                foreach (DataRow row in dataTable.Rows)
                {
                    DynamicDatabaseColumn column = new();
                    column.ColumnName = row.Field<string>("ColumnName") ?? "ColumnNameNotFound";
                    column.ColumnSize = row.Field<int>("ColumnSize");
                    //var type = (SqlDbType)(int)row["ProviderType"];
                    column.DataType = row.Field<string>("DataTypeName");
                    column.Required = !row.Field<bool>("AllowDBNull");
                    column.IsAutoIncrement = row.Field<bool>("IsAutoIncrement");
                    column.IsIdentity = row.Field<bool>("IsIdentity");
                    column.IsKey = row.Field<bool?>("IsKey") ?? false;
                    column.Label = StringHelperService.AddSpacesToSentence(column.ColumnName ?? "");
                    column.PropertyName = StringHelperService.RemoveUnsupportedCharacters(column.ColumnName ?? "").Replace("ID", "Id");


                    var tableStructure = tableStructures.FirstOrDefault(f => f.Column == column.ColumnName);
                    if (tableStructure != null)
                    {
                        if (tableStructure.FieldLabel != null && !string.IsNullOrWhiteSpace(tableStructure.FieldLabel))
                        {
                            column.Label = tableStructure.FieldLabel;
                        }
                        column.Order = tableStructure.AdjustedOrder;
                        column.Width = tableStructure.Width;
                        column.Height = tableStructure.Height;
                        column.Property = tableStructure.Property;
                        column.HelpText = tableStructure.HelpText;
                        column.TextEditor = tableStructure.TextEditor;
                        if (!string.IsNullOrWhiteSpace(tableStructure.Group))
                        {
                            column.Group = tableStructure.Group;
                            column.GroupLabel = _tableStructureServices.RemoveLeadingNumber(column.Group);
                        }
                        column.LookUp = tableStructure.Lookup;
                        column.LookUpOrder = tableStructure.Order;

                        if (!tableStructure.Exclude)
                        {
                            columns.Add(column);
                        }
                    }
                    else
                    {

                        column.Exclude = true;
                        column.Property = "HIDDEN";
                        columns.Add(column);
                    }
                }
            }
            return columns.OrderBy(o => o.Group).ThenBy(f => f.Order).ToList();
        }
        public int GetRecordCount(string conStr, string tableName)
        {
            using (var sqlCon = new SqlConnection(conStr))
            {
                sqlCon.Open();
                var sqlCmd = sqlCon.CreateCommand();
                tableName = IncludeSquareBrackets(tableName);
                var sqlText = $"select Count (*) from {tableName}";
                sqlCmd.CommandText = sqlText;
                sqlCmd.CommandType = CommandType.Text;
                int rows = (int)sqlCmd.ExecuteScalar();
                return rows;
            }
        }
        public int GetRecordCountFromSql(string sqlText)
        {
            using (var sqlCon = new SqlConnection(_connectionString))
            {
                sqlCon.Open();
                var sqlCmd = sqlCon.CreateCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = sqlText;
                int rows = (int)sqlCmd.ExecuteScalar();
                return rows;
            }
        }
        public DataTable GetDataIntoDataTable(string sql, string? searchTerm = null, string db = "VoiceLauncher", int maxRows = 500)
        {
            using (var sqlCon = new SqlConnection(_connectionString))
            {
                sqlCon.Open();
                SqlCommand? sqlCommand = sqlCon.CreateCommand();
                if (!sql.ToLower().Contains(" top ") && Regex.Matches(sql.ToLower(), "select").Count == 1)
                {
                    sql = sql.ToLower().Replace("select", $"select top {maxRows} ");
                }
                string? sqlText = $"{sql}";
                sqlCommand.CommandText = sqlText;
                sqlCommand.CommandType = CommandType.Text;
                SqlDataReader? sqlDataReader = null;
                try
                {
                    sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleResult);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Problem running command in get data into data table method");
                    throw;
                }
                var dataTable = new DataTable();
                if (sqlDataReader != null)
                    dataTable.Load(sqlDataReader);
                return dataTable;
            }
        }

        private string BuildFilterForColumnsForDataTable(string[] colSearches, IEnumerable<DynamicDatabaseColumn> cols)
        {
            var andOperator = "";
            var found = 0;
            string columnSearch = "";
            foreach (var colSearch in colSearches)
            {
                if (!string.IsNullOrWhiteSpace(colSearch))
                {
                    var values = colSearch.Split('|');
                    var searchValue = values[0];
                    var columnName = values[1];
                    foreach (var col in cols)
                    {
                        if (col.ColumnName.ToLower() == columnName.ToLower() && !string.IsNullOrWhiteSpace(searchValue))
                        {
                            found++;
                            if (found > 1)
                            {
                                andOperator = " AND ";
                            }
                            string temp = "";
                            if (col.DataType == "nvarchar")
                            {
                                temp = $"{andOperator} [{col.ColumnName}] Like '%{searchValue}%'";
                            }
                            else
                            {
                                if (searchValue.All(char.IsDigit))
                                {
                                    temp = $"{andOperator} [{col.ColumnName}] = {searchValue}";
                                }
                            }
                            columnSearch = $"{columnSearch}{temp}";
                        }
                    }
                }
            }
            return columnSearch;
        }

        public string BuildSqlQuery(string tableName, int maxRows, string? searchTerm, IEnumerable<DynamicDatabaseColumn> cols, string? sortColumn = null, string? sortDirection = null, string[]? colSearches = null, bool columnSearchActive = false, bool exactMatch = false, string tableNameRaw = "")
        {
            var sqlText = "";
            if (tableName.StartsWith("[") && tableName.EndsWith("]"))
            {
                sqlText = $"SELECT TOP {maxRows} <<fieldNames>> FROM {tableName}";
            }
            else
            {
                sqlText = $"SELECT TOP {maxRows} <<fieldNames>> FROM [{tableName}]";
            }
            var fieldNames = "";
            var counter = 0;
            string columnSearch = "";
            var orOperator = "";
            foreach (var col in cols.Where(v => v.ColumnName != "CollectionName"))
            {
                counter++;
                if (counter > 1)
                {
                    fieldNames = $"{fieldNames}, [{col.ColumnName}] ";
                }
                else
                {
                    fieldNames = $" [{col.ColumnName}] ";
                }
            }
            string searchTermFilter = "";
            if (!string.IsNullOrWhiteSpace(searchTerm) || columnSearchActive )
            {

                counter = 0; orOperator = "";
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    foreach (var col in cols.Where(w => w.DataType == "nvarchar"))
                    {
                        counter++;
                        if (counter > 1)
                        {
                            orOperator = " OR ";
                        }
                        var temp = $"{orOperator} UPPER({tableName}.[{col.ColumnName}]) LIKE '%{searchTerm.ToUpper()}%'";
                        searchTermFilter = $"{searchTermFilter} {temp} ";
                    }
                }
                if (columnSearchActive && colSearches != null)
                {
                    var andOperator = "";
                    counter = 0;
                    foreach (var colSearch in colSearches)
                    {
                        if (!string.IsNullOrWhiteSpace(colSearch))
                        {
                            var values = colSearch.Split('|');
                            var searchValue = values[0];
                            var columnName = values[1];

                            foreach (var col in cols.Where(v => v.ColumnName == columnName))
                            {
                                if (col.ColumnName.ToLower() == columnName.ToLower() && !string.IsNullOrWhiteSpace(searchValue))
                                {
                                    counter++;
                                    if (counter > 1)
                                    {
                                        andOperator = " AND ";
                                    }
                                    string temp = "";
                                    if (col.DataType == "nvarchar")
                                    {
                                        string temporaryTablename = tableName ?? "";
                                        if (col.ColumnName == "CollectionName")
                                        {
                                            temporaryTablename = "[collection]";
                                        }
                                        if (exactMatch) //Applies for initial filter only
                                        {
                                            temp = $"{andOperator} UPPER({temporaryTablename}.[{col.ColumnName}]) = '{searchValue!.ToUpper()}'";
                                        }
                                        else
                                        {
                                            temp = $"{andOperator} UPPER({temporaryTablename}.[{col.ColumnName}]) LIKE '%{searchValue!.ToUpper()}%'";
                                        }
                                    }
                                    else
                                    {
                                        if (searchValue.All(char.IsDigit))
                                        {
                                            temp = $"{andOperator} {tableName}.[{col.ColumnName}] = {searchValue}";
                                        }
                                    }
                                    columnSearch = $"{columnSearch}{temp}";
                                }
                            }
                        }
                    }
                }
                string firstAnd = "";
                string secondAnd = "";
                if (!string.IsNullOrWhiteSpace(searchTermFilter) && !string.IsNullOrWhiteSpace(columnSearch))
                {
                    firstAnd = " AND ";
                }
                searchTermFilter = SurroundWithBracketsIfNotNull(searchTermFilter);
                columnSearch = SurroundWithBracketsIfNotNull(columnSearch);
                columnSearch = SurroundWithBracketsIfNotNull(columnSearch);
                if (!string.IsNullOrWhiteSpace(searchTermFilter) || !string.IsNullOrWhiteSpace(columnSearch))
                {
                    sqlText = $"{sqlText} WHERE ( {searchTermFilter} {firstAnd} {columnSearch} {secondAnd} )";
                }
            }

            if (sortColumn != null && sortDirection != null)
            {
                sqlText = $"{sqlText} ORDER BY [{sortColumn}] {sortDirection}";
            }
            sqlText = sqlText.Replace("<<fieldNames>>", fieldNames);
            //Console.WriteLine(sqlText);
            if (Environment.MachineName == "J40L4V3")
            {
                try
                {
                    File.WriteAllText(@"C:\Users\MPhil\Documents\sqlLatest.sql", sqlText);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "problem writing out sql text file");
                }
            }
            return $"{sqlText};";
        }

        private static string SurroundWithBracketsIfNotNull(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                value = $" ( {value} )";
            }

            return value;
        }

        private static string IncludeSquareBrackets(string tableName)
        {
            if (tableName.ToLower().StartsWith("dbo."))
            {
                tableName = tableName.Replace("dbo.", "");
                tableName = $"dbo.[{tableName}]";
            }
            else
            {
                tableName = $"[{tableName}]";
            }
            return tableName.Replace("[[", "[").Replace("]]", "]");
        }

        public void ExecuteCommand(string Command)
        {
            ProcessStartInfo ProcessInfo;
            ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + Command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = true;
            ProcessInfo.WorkingDirectory = Environment.CurrentDirectory;
            if (ProcessInfo != null)
            {
                Process.Start(ProcessInfo);
            }
        }
        private bool IsThisATable(string tableName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var rawTableName = tableName.Replace("[", "").Replace("]", "");
                try
                {
                    var command = new SqlCommand($"select * from sys.tables WHERE [name]='{rawTableName}'", connection);
                    SqlDataReader? sqlDataReader = command.ExecuteReader(CommandBehavior.SingleRow);
                    sqlDataReader.Read();
                    if (sqlDataReader.HasRows)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError($"Error occurred is this a table method: {rawTableName}", exception);
                    throw;
                }
            }
        }
    }
}
