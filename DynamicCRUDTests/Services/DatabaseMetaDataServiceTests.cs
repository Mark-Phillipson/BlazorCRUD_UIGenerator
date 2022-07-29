using DynamicCRUD.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace DynamicCRUDTests.Services
{
    public class DatabaseMetaDataServiceTests
    {
        readonly WebApplicationBuilder? _builder;
        readonly DatabaseMetaDataService _databaseMetaDataService = new();
        string connectionString = "";
        public DatabaseMetaDataServiceTests()
        {
            _builder = WebApplication.CreateBuilder();
            connectionString = _builder.Configuration.GetConnectionString("DefaultConnection");
        }
        [Fact]
        public void GetDatabaseListTest()
        {
            if (_builder == null) { Assert.True(false); return; }

            var result = _databaseMetaDataService.GetDatabaseList(connectionString);
            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
            var record = result.FirstOrDefault(f => f.Tablename == "dbo.__EFMigrationsHistory");
            Assert.NotNull(record);
        }
        [Fact]
        public void GetDatabaseListBadConnectionStringTest()
        {
            if (_builder == null) { Assert.True(false); return; }
            var connectionString = _builder.Configuration.GetConnectionString("XARM_CUSTOM");
            try
            {
                var result = _databaseMetaDataService.GetDatabaseList(connectionString);
            }
            catch (Exception exception)
            {
                Assert.Contains("The ConnectionString property has not been initialized.", exception.Message);
            }
        }
        [Fact]
        public void GetColumnNamesTest()
        {
            if (_builder == null) { Assert.True(false); return; }
                var result = _databaseMetaDataService.GetColumnNames(connectionString, "[User]", "dbo");
                Assert.NotNull(result);
                Assert.Contains(result, c => c.IsKey == true);
        }
    }
}