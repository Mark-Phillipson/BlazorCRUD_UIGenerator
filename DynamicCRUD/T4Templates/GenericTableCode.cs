
using DynamicCRUD.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCRUD.T4Templates
{
    public partial class GenericTable
    {
        private readonly IEnumerable<ClientDatabaseColumn> DatabaseColumns;
        string PrimaryKeyDataType { get; set; }
        string ModelName { get; set; }
        public string ModelNameWithSpaces { get; set; } = "";
        string PluralTablename { get; set; }
        string Namespace { get; set; } = "";
        string ModelNameCamelCase { get; }
        string PrimaryKeyName { get; set; } = "";
        string FilterColumns { get; set; }
        public string ForeignKeyName { get; set; } = "";
        public string ForeignKeyDataType { get; set; } = "";
        public bool UseBlazored { get; set; } = true;
        public bool UseRadzen { get; set; } = false;
        public string DataServiceNamespace { get; set; } = "TBC";
        public string RepositoryNamespace { get; set; } = "TBC";
        public GenericTable(IEnumerable<ClientDatabaseColumn> databaseColumns, string modelName, string modelNameCamelCase, string pluralTablename, string primaryKeyName, string primaryKeyDataType, string Namespace, string filterColumns, string foreignKeyName, string foreignKeyDataType, bool useBlazored, bool useRadzen, string dataServiceNamespace, string repositoryNamespace)
        {
            this.Namespace = Namespace;
            DatabaseColumns = databaseColumns;
            ModelName = modelName;
            ModelNameWithSpaces = StringHelperService.AddSpacesToSentence(modelName);
            ModelNameCamelCase = modelNameCamelCase;
            PluralTablename = pluralTablename;
            PrimaryKeyName = primaryKeyName;
            PrimaryKeyDataType = primaryKeyDataType;
            FilterColumns = filterColumns;
            ForeignKeyName = foreignKeyName;
            ForeignKeyDataType = foreignKeyDataType;
            UseBlazored = useBlazored;
            UseRadzen = useRadzen;
            DataServiceNamespace = dataServiceNamespace;
            RepositoryNamespace = repositoryNamespace;
        }

    }
}
