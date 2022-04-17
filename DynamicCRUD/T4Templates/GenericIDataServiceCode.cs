
using DynamicCRUD.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCRUD.T4Templates
{
    public partial class GenericIDataService
    {
        private readonly IEnumerable<ClientDatabaseColumn> DatabaseColumns;
        string PrimaryKeyDataType { get; set; }

        string ModelName { get; set; }
        string PluralTablename { get; set; }
        string Namespace { get; set; } = "";
        string ModelNameCamelCase { get; }
        string PrimaryKeyName { get; set; } = "";
        string ForeignKeyName { get; set; } = "";
        string ForeignKeyDataType { get; set; }= "";

        public GenericIDataService(IEnumerable<ClientDatabaseColumn> databaseColumns, string modelName, string modelNameCamelCase, string pluralTablename, string primaryKeyName, string primaryKeyDataType, string Namespace ,string foreignKeyName, string foreignKeyDataType)
        {
            this.Namespace = Namespace;
            DatabaseColumns = databaseColumns;
            ModelName = modelName;
            ModelNameCamelCase = modelNameCamelCase;
            PluralTablename = pluralTablename;
            PrimaryKeyDataType = primaryKeyDataType;
            PrimaryKeyName = primaryKeyName;
			ForeignKeyName = foreignKeyName;
			ForeignKeyDataType = foreignKeyDataType;
        }
    }
}
