using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;

namespace DynamicCRUD.Services
{
    public class DatabaseMetaDataService
    {
        public IEnumerable<ClientDatabaseTable> GetDatabaseList(string conString)
        {
            List<ClientDatabaseTable> list = new List<ClientDatabaseTable>();
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT name from sys.databases", con))
                { //List<string> tables = new List<string>();  
                    DataTable dt = con.GetSchema("Tables");
                    foreach (DataRow row in dt.Rows)
                    {
                        string tablename = (string)row[1] + "." + (string)row[2];
                        ClientDatabaseTable database = new ClientDatabaseTable();
                        database.Tablename = tablename;
                        list.Add(database);
                    }
                }
            }
            return list.OrderBy(o => o.Tablename).ToList();
        }
        public IEnumerable<ClientDatabaseColumn> GetColumnNames(string conStr, string tableName, string schemaName)
        {
            var columns = new List<ClientDatabaseColumn>();
            using (var sqlCon = new SqlConnection(conStr))
            {
                sqlCon.Open();
                var sqlCmd = sqlCon.CreateCommand();
                tableName = IncludeSquareBrackets(tableName, schemaName);
                sqlCmd.CommandText = $"SET FMTONLY ON; select * from {tableName}; SET FMTONLY OFF"; // No data wanted, only schema  
                sqlCmd.CommandType = CommandType.Text;
                var sqlDR = sqlCmd.ExecuteReader(CommandBehavior.KeyInfo);
                var dataTable = sqlDR.GetSchemaTable();
                foreach (DataRow row in dataTable.Rows)
                {
                    ClientDatabaseColumn column = new ClientDatabaseColumn();
                    column.ColumnName = row.Field<string>("ColumnName");
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
            }
            //https://docs.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqldatareader.getschematable?f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(Microsoft.Data.SqlClient.SqlDataReader.GetSchemaTable);k(DevLang-csharp)%26rd%3Dtrue&view=sqlclient-dotnet-standard-4.1
            return columns; //.OrderBy(o => o.ColumnName).ToList();
        }
        public DataTable GetData(string conStr, string tableName, int maxRows, string? searchTerm = null, string schemaName = "dbo")
        {
            using (var sqlCon = new SqlConnection(conStr))
            {
                sqlCon.Open();
                var sqlCmd = sqlCon.CreateCommand();
                var cols = GetColumnNames(conStr, tableName, schemaName);
                tableName = IncludeSquareBrackets(tableName, schemaName);
                string sqlText = BuildSqlQuery(tableName, maxRows, searchTerm, cols);
                sqlCmd.CommandText = sqlText;
                sqlCmd.CommandType = CommandType.Text;
                var sqlDR = sqlCmd.ExecuteReader(CommandBehavior.SingleResult);
                var dataTable = new DataTable();
                dataTable.Load(sqlDR);
                return dataTable;
            }
        }

        private static string BuildSqlQuery(string tableName, int maxRows, string? searchTerm, IEnumerable<ClientDatabaseColumn> cols)
        {
            var sqlText = $"select top {maxRows} <<fieldNames>> from {tableName}";
            var fieldNames = "";
            var counter = 0;
            foreach (var col in cols)
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
            if (searchTerm != null)
            {
                sqlText = $"{sqlText} where (";
                counter = 0; var orOperator = "";
                foreach (var col in cols.Where(w => w.DataType == "nvarchar"))
                {
                    counter++;
                    if (counter > 1)
                    {
                        orOperator = " OR ";
                    }
                    var temp = $"{orOperator} UPPER({tableName}.[{col.ColumnName}]) Like '%{searchTerm.ToUpper()}%'";
                    sqlText = $"{sqlText} {temp} ";
                }
                sqlText = $"{sqlText}  );";
            }
            sqlText = sqlText.Replace("<<fieldNames>>", fieldNames);

            return sqlText;
        }

        private static string IncludeSquareBrackets(string tableName, string schemaName)
        {
            if (tableName.ToLower().StartsWith($"{schemaName.ToLower()}."))
            {
                tableName = tableName.Replace($"{schemaName}.", "");
                tableName = $"{schemaName}.[{tableName}]";
            }

            return tableName;
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
    }
}
