using System.Data;

using Ardalis.GuardClauses;
using BlazorAppEditTable.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BlazorAppEditTable.Services
{
    public class TableStructureServices
    {

        private readonly IConfiguration _configuration;
        MyDbContext _context;

        public TableStructureServices(IConfiguration configuration, MyDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public List<TableStructure> GetData(string tableName, string dataBase)
        {
            //This method gets a stored procedure into a model
            Guard.Against.Null(dataBase, nameof(dataBase));
            Guard.Against.Null(tableName, nameof(tableName));
            tableName = tableName.Replace("[", "").Replace("]", "");
            using (var sqlCon = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                sqlCon.Open();
                SqlCommand? sqlCmd = sqlCon.CreateCommand();
                sqlCmd.CommandText = $"Execute [dbo].[SYS-TableStructure] '{tableName}', '{dataBase}'"; // No data wanted, only schema  
                sqlCmd.CommandType = CommandType.Text;
                SqlDataReader? SqlDataReader = sqlCmd.ExecuteReader(CommandBehavior.Default);
                DataTable? dataTable = SqlDataReader.GetSchemaTable();
                List<Temporary> list = new List<Temporary>();
                foreach (DataRow row in dataTable.Rows)
                {
                    Temporary temporary = new();
                    var ColumnName = row.Field<string>("ColumnName");
                    if (ColumnName != null)
                    {
                        temporary.Name = ColumnName;
                    }
                    list.Add(temporary);
                }
                var result = _context.TableStructures.
            FromSqlInterpolated($"Execute [dbo].[SYS-TableStructure] {tableName}, {dataBase}").AsNoTracking().ToList();
                return result;
            }

        }

        public string RemoveLeadingNumber(string group)
        {
            if (string.IsNullOrWhiteSpace(group))
            {
                return group;
            }
            string result = "";
            foreach (char item in group)
            {
                if (!char.IsDigit(item))
                {
                    result = $"{result}{item}";
                }
            }
            return result;
        }
    }
}