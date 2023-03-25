using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BlazorAppEditTable.Services
{
    public class DynamicUpdates
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DynamicUpdates> _logger;
        private readonly string _connectionString;

        public DynamicUpdates(IConfiguration configuration, ILogger<DynamicUpdates> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? "?";
        }
        public (bool success, string message) UpdateTable(IFormCollection valuePairs, ApplicationState mvcApplicationState)
        {
            var success = false;
            string? message = "";
            string sqlText = "";
            if (mvcApplicationState.IsAddNew)
            {
                bool duplicate = CheckRecordExists(valuePairs, mvcApplicationState);
                if (duplicate)
                {
                    message = $"There is already a {mvcApplicationState.TableName} with a {mvcApplicationState.PrimaryKeyName} Of {valuePairs[mvcApplicationState?.PrimaryKeyName ?? ""]} Primary keys have to be unique. Please check and try again.";
                    return (false, message);
                }
                sqlText = BuildAddSql(valuePairs, mvcApplicationState);
            }
            else
            {
                sqlText = BuildUpdateSql(valuePairs, mvcApplicationState);
            }
            success = ExecuteSql(ref success, sqlText);
            return (success, message);
        }
        public bool UpdateTable(DataRow dataRow, ApplicationState mvcApplicationState)
        {
            var success = false;
            string sqlText = "";
            sqlText = BuildUpdateSql(dataRow, mvcApplicationState);
            return ExecuteSql(ref success, sqlText);
        }
        bool CheckRecordExists(IFormCollection valuePairs, ApplicationState mvcApplicationState)
        {
            string sqlText = "";
            if (mvcApplicationState.PrimaryKeyName == null)
            {
                return true;
            }
            if (mvcApplicationState.PrimaryKeyDataType == "int")
            {
                sqlText = $"SELECT Count(*) FROM [{mvcApplicationState.TableName}]  WHERE {mvcApplicationState.PrimaryKeyName}={valuePairs[mvcApplicationState.PrimaryKeyName]}";

            }
            else
            {
                sqlText = $"SELECT Count(*) FROM [{mvcApplicationState.TableName}]  WHERE {mvcApplicationState.PrimaryKeyName}='{valuePairs[mvcApplicationState.PrimaryKeyName]}'";
            }
            int result = GetRecordCountFromSql(sqlText);
            return result > 0;
        }


        private bool ExecuteSql(ref bool success, string sqlText)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var displayOnly = sqlText;
                Console.WriteLine(displayOnly);
                var command = new SqlCommand(sqlText, connection);
                try
                {
                    command.ExecuteNonQuery();
                    success = true;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "problem executing query in execute sql method");
                    throw;
                }
                finally
                {
                    connection.Close();
                }
                return success;
            }
        }

        private string BuildAddSql(IFormCollection valuePairs, ApplicationState mvcApplicationState)
        {
            if (mvcApplicationState.TableName == null)
            {
                throw new Exception(" Tablename empty when not expected ! ");
            }
            var sqlText = $"INSERT INTO [{mvcApplicationState.TableName.Replace("dbo.", "")}] ( ";
            var counter = 0;
            if (mvcApplicationState.Columns == null || mvcApplicationState.PrimaryKeyName == null)
            {
                throw new Exception(" columns is empty ! ");
            }
            foreach (var col in mvcApplicationState.Columns.Where(v => v.IsAutoIncrement == false && v.ColumnName != "CollectionName"))
            {
                counter++;
                if (col.ColumnName == null)
                {
                    break;
                }
                if (counter > 1)
                {
                    sqlText = $"{sqlText},";
                }
                sqlText = $"{sqlText} [{col.ColumnName}]";
            }
            sqlText = $"{sqlText} ) VALUES ( ";

            counter = 0;
            foreach (var col in mvcApplicationState.Columns.Where(v => v.IsAutoIncrement == false && v.ColumnName != "CollectionName"))
            {
                counter++;
                if (col.ColumnName == null)
                {
                    break;
                }
                if (counter > 1)
                {
                    sqlText = $"{sqlText},";
                }
                if (col.DataType == "nvarchar")
                {
                    sqlText = $"{sqlText} '{valuePairs[col.ColumnName].ToString().Trim()}'";
                }
                else if (col.DataType != null && col.DataType.ToLower().Contains("date"))
                {
                    string result = valuePairs[col.ColumnName];
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        sqlText = $"{sqlText} null ";
                    }
                    else
                    {
                        sqlText = $"{sqlText} '{result}'";
                    }
                }
                else if (col.DataType == "bit")
                {
                    sqlText = CalculateBitColumn(valuePairs, sqlText, col);
                }
                else
                {
                    string result = valuePairs[col.ColumnName].ToString().Trim();
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        result = "null";
                    }
                    sqlText = $"{sqlText} {result}";
                }
            }
            sqlText = $"{sqlText} ) ";
            return sqlText;
        }
        private string BuildAddSql(DataRow dataRow, ApplicationState mvcApplicationState)
        {
            if (mvcApplicationState.TableName == null)
            {
                throw new Exception(" Tablename empty when not expected ! ");
            }
            if (mvcApplicationState.Columns == null)
            {
                throw new Exception("Columns empty when not expected!");
            }
            var sqlText = $"INSERT INTO [{mvcApplicationState.TableName.Replace("dbo.", "")}] ( ";
            if (mvcApplicationState.PrimaryKeyName == null)
            {
                throw new Exception("Primary Key Name is empty ! ");
            }
            var counter = 0;
            foreach (var col in mvcApplicationState.Columns.Where(v => v.IsAutoIncrement == false))
            {
                counter++;
                if (col.ColumnName == null)
                {
                    break;
                }
                if (counter > 1)
                {
                    sqlText = $"{sqlText},";
                }
                sqlText = $"{sqlText} [{col.ColumnName}]";
            }
            sqlText = $"{sqlText} ) VALUES ( ";

            counter = 0;
            foreach (var col in mvcApplicationState.Columns.Where(v => v.IsAutoIncrement == false))
            {
                counter++;
                if (col.ColumnName == null)
                {
                    break;
                }
                if (counter > 1)
                {
                    sqlText = $"{sqlText},";
                }
                if (col.DataType == "nvarchar")
                {
                    sqlText = $"{sqlText} '{dataRow[col.ColumnName]?.ToString()?.Trim()}'";
                }
                else if (col.DataType != null && col.DataType.ToLower().Contains("date"))
                {
                    DateTime result = (DateTime)dataRow[col.ColumnName];
                    string date = result.ToString("yyyy-MM-dd HH:mm:ss");
                    if (string.IsNullOrWhiteSpace(date))
                    {
                        sqlText = $"{sqlText} null ";
                    }
                    else
                    {
                        sqlText = $"{sqlText} '{date}'";
                    }
                }
                else
                {
                    string? result = dataRow[col.ColumnName]?.ToString()?.Trim();
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        result = "null";
                    }
                    else if (result.ToLower() == "true")
                    {
                        result = "1";
                    }
                    else if (result.ToLower() == "false")
                    {
                        result = "0";
                    }
                    sqlText = $"{sqlText} {result}";
                }
            }
            sqlText = $"{sqlText} ) ";
            return sqlText;
        }
        private string BuildUpdateSql(IFormCollection valuePairs, ApplicationState mvcApplicationState)
        {
            if (mvcApplicationState.TableName == null)
            {
                throw new Exception(" Tablename empty when not expected ! ");
            }
            var sqlText = $"UPDATE [{mvcApplicationState.TableName?.Replace("dbo.", "")}] SET ";
            var counter = 0;
            if (mvcApplicationState.Columns == null || mvcApplicationState.PrimaryKeyName == null)
            {
                throw new Exception(" columns is empty ! ");
            }
            foreach (var col in mvcApplicationState.Columns.Where(v => v.IsAutoIncrement == false && v.ColumnName != "CollectionName"))
            {
                counter++;
                if (col.ColumnName == null)
                {
                    break;
                }
                if (counter > 1)
                {
                    sqlText = $"{sqlText},";
                }
                sqlText = $"{sqlText} [{col.ColumnName}]=";

                if (col.DataType == "nvarchar")
                {
                    sqlText = $"{sqlText} '{valuePairs[col.ColumnName].ToString().Trim().Replace("'", "''")}'";
                }
                else if (col.DataType != null && col.DataType.ToLower().Contains("date"))
                {
                    string result = valuePairs[col.ColumnName];
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        sqlText = $"{sqlText} null ";
                    }
                    else
                    {
                        sqlText = $"{sqlText} '{result}'";
                    }
                }
                else if (col.DataType == "bit")
                {
                    sqlText = CalculateBitColumn(valuePairs, sqlText, col);
                }
                else
                {
                    string result = valuePairs[col.ColumnName].ToString().Trim();
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        result = "null";
                    }
                    sqlText = $"{sqlText} {result}";
                }
            }
            if (mvcApplicationState.PrimaryKeyDataType == "nvarchar")
            {
                sqlText = $"{sqlText} WHERE [{mvcApplicationState.PrimaryKeyName}]='{valuePairs[mvcApplicationState.PrimaryKeyName].ToString()}'";
            }
            else
            {
                sqlText = $"{sqlText} WHERE [{mvcApplicationState.PrimaryKeyName}]={valuePairs[mvcApplicationState.PrimaryKeyName].ToString()}";
            }
            return sqlText;
        }
        private string BuildUpdateSql(DataRow dataRow, ApplicationState applicationState)
        {
            if (dataRow == null || dataRow.ItemArray.Length == 0 || applicationState == null || applicationState.TableName == null || applicationState.PrimaryKeyName == null)
            {
                throw new Exception(" Build update SQL have missing pieces! ");
            }
            var sqlText = $"UPDATE [{applicationState.TableName.Replace("dbo.", "")}] SET ";
            var counter = 0;
            if (applicationState.Columns == null || applicationState.PrimaryKeyName == null)
            {
                throw new Exception(" columns is empty ! ");
            }
            foreach (var col in applicationState.Columns.Where(v => v.IsAutoIncrement == false))
            {
                counter++;
                if (col.ColumnName == null)
                {
                    break;
                }
                if (counter > 1)
                {
                    sqlText = $"{sqlText},";
                }
                sqlText = $"{sqlText} [{col.ColumnName}]=";

                if (col.DataType == "nvarchar")
                {
                    sqlText = $"{sqlText} '{dataRow[col.ColumnName]?.ToString()?.Trim().Replace("'", "''")}'";
                }
                else if (col.DataType != null && col.DataType.ToLower().Contains("date"))
                {
                    string result = dataRow[col.ColumnName].ToString() ?? "";
                    string rawResult = result;
                    if (result.Length == 19)//UK format?
                    {
                        result = result.Substring(6, 4) + "-" + result.Substring(3, 2) + "-" +
                        result.Substring(0, 2);
                    }
                    else if (result.Length == 20)//USA format?
                    {
                        var dateParts = result.Split("/");
                        var year = dateParts[2].Substring(0, 4);
                        var month = dateParts[0].Length == 2 ? dateParts[0] : "0" + dateParts[0];
                        var day = dateParts[1].Length == 2 ? dateParts[1] : "0" + dateParts[1];
                        result = year + "-" + month + "-" + day;
                    }
                    else
                    {
                        result = "";
                    }
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        sqlText = $"{sqlText} null ";
                    }
                    else
                    {
                        sqlText = $"{sqlText} '{result}'";
                    }
                }
                else
                {
                    string? result = dataRow[col.ColumnName]?.ToString()?.Trim();
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        result = "null";
                    }
                    else if (result.ToLower() == "true")
                    {
                        result = "1";
                    }
                    else if (result.ToLower() == "false")
                    {
                        result = "0";
                    }
                    sqlText = $"{sqlText} {result}";
                }
            }
            if (applicationState.PrimaryKeyDataType == "nvarchar")
            {
                sqlText = $"{sqlText} WHERE [{applicationState.PrimaryKeyName}]='{dataRow[applicationState.PrimaryKeyName]}'";
            }
            else
            {
                sqlText = $"{sqlText} WHERE [{applicationState.PrimaryKeyName}]={dataRow[applicationState.PrimaryKeyName]}";
            }
            return sqlText;
        }

        private static string CalculateBitColumn(IFormCollection valuePairs, string sqlText, DynamicDatabaseColumn col)
        {
            if (valuePairs[col.ColumnName].ToString() == "1" || valuePairs[col.ColumnName].ToString().ToLower().Contains("true") || valuePairs[col.ColumnName].ToString().ToLower().Contains("yes"))
            {
                sqlText = $"{sqlText} 1";
            }
            else if (valuePairs[col.ColumnName].ToString().Trim() == "")
            {
                sqlText = $"{sqlText} null ";
            }
            else if (valuePairs[col.ColumnName].ToString().Trim() == "0" || valuePairs[col.ColumnName].ToString().ToLower() == "no" || valuePairs[col.ColumnName].ToString().ToLower() == "false")
            {
                sqlText = $"{sqlText} 0";
            }

            return sqlText;
        }

        public bool DeleteRecord(string? id, ApplicationState mvcApplicationState)
        {
            string sqlText = BuildDeleteSql(id, mvcApplicationState);
            bool success = false;
            return ExecuteSql(ref success, sqlText);
        }
        public (bool, string) DeleteMultipleRecords(string deletionIds, ApplicationState mvcApplicationState)
        {

            var sqlText = $"DELETE FROM [{mvcApplicationState.TableName}] ";
            List<string> columns = deletionIds.Split("|+|").ToList();

            if (mvcApplicationState.PrimaryKeyDataType?.ToLower() == "nvarchar")
            {
                string formatted = "";
                foreach (var item in columns)
                {
                    if (item.Length > 0)
                    {
                        formatted = $"{formatted}'{item}',";
                    }
                }
                formatted = formatted.Substring(0, formatted.Length - 1);
                sqlText = $"{sqlText} WHERE [{mvcApplicationState.PrimaryKeyName}] IN ({formatted})";
            }
            else
            {
                string formatted = "";
                foreach (var item in columns)
                {
                    if (item.Length > 0)
                    {
                        formatted = $"{formatted}{item},";
                    }
                }
                formatted = formatted.Substring(0, formatted.Length - 1);
                sqlText = $"{sqlText} WHERE [{mvcApplicationState.PrimaryKeyName}] IN ({formatted})";
            }
            bool success = false; string message = "";
            (success, message) = ExecuteSqlReturnMessage(sqlText);
            return (success, message);
        }

        private (bool success, string message) ExecuteSqlReturnMessage(string sqlText)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var displayOnly = sqlText;
                Console.WriteLine(displayOnly);
                var command = new SqlCommand(sqlText, connection);
                var success = false;
                string message = "";
                try
                {
                    var deleted = command.ExecuteNonQuery();
                    if (deleted > 1)
                    {
                        message = $"{deleted} records have been successfully deleted";
                    }
                    else
                    {
                        message = $"{deleted} record has been successfully deleted";
                    }
                    success = true;
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, $"problem executing sql with return message {sqlText}");
                    message = exception.Message;
                    success = false;
                }
                finally
                {
                    connection.Close();
                }
                return (success, message);
            }
        }

        private string BuildDeleteSql(string? id, ApplicationState mvcApplicationState)
        {
            var sqlText = $"DELETE FROM [{mvcApplicationState.TableName}] ";
            if (mvcApplicationState.PrimaryKeyDataType == "nvarchar")
            {
                sqlText = $"{sqlText} WHERE [{mvcApplicationState.PrimaryKeyName}]='{id}'";
            }
            else
            {
                sqlText = $"{sqlText} WHERE [{mvcApplicationState.PrimaryKeyName}]={id}";
            }
            return sqlText;
        }

        public bool AddRecord(DataRow dataRow, ApplicationState mvcApplicationState)
        {
            var success = false;
            string sqlText = "";
            sqlText = BuildAddSql(dataRow, mvcApplicationState);
            return ExecuteSql(ref success, sqlText);
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
    }
}