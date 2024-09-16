
using DynamicCRUD.Services;

using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCRUD.T4Templates
{
    public partial class GenericTableCodeBehind
    {
        private readonly IEnumerable<ClientDatabaseColumn> DatabaseColumns;
        string PrimaryKeyDataType { get; set; }
        string ModelName { get; set; }
        string PluralTablename { get; set; }
        string Namespace { get; set; } = "";
        string ModelNameCamelCase { get; }
        string PrimaryKeyName { get; set; } = "";
        public string? DefaultSortColumn { get; set; } = "DefaultSortColumn";
        string ForeignKeyName { get; set; } = "";
        string ForeignKeyDataType { get; set; } = "";
        public string ModelNameWithSpaces { get; set; } = "";
        public bool UseBlazored { get; set; } = true;
        public bool UseRadzen { get; set; } = false;
        public string DataServiceNamespace { get; set; } = "TBC";
        public string RepositoryNamespace { get; set; } = "TBC";
        public string DTONamespaceName { get; set; } = "TBC";
        public GenericTableCodeBehind(IEnumerable<ClientDatabaseColumn> databaseColumns, string modelName, string modelNameCamelCase, string pluralTablename, string primaryKeyName, string primaryKeyDataType, string Namespace, string foreignKeyName, string foreignKeyDataType, bool useBlazored, bool useRadzen, string dataServiceNamespace, string repositoryNamespace, string dtoNamespaceName)
        {
            this.Namespace = Namespace;
            DatabaseColumns = databaseColumns;
            ModelName = modelName;
            ModelNameCamelCase = modelNameCamelCase;
            PluralTablename = pluralTablename;
            PrimaryKeyName = primaryKeyName;
            PrimaryKeyDataType = primaryKeyDataType;
            ForeignKeyName = foreignKeyName;
            ForeignKeyDataType = foreignKeyDataType;
            UseBlazored = useBlazored;
            UseRadzen = useRadzen;
            DataServiceNamespace = dataServiceNamespace;
            RepositoryNamespace = repositoryNamespace;
            DTONamespaceName = dtoNamespaceName;
            ModelNameWithSpaces = StringHelperService.AddSpacesToSentence(modelName);
            var result = databaseColumns.FirstOrDefault(c => c.Sort == true);
            if (result != null && result.PropertyName != null)
            {
                DefaultSortColumn = result.PropertyName;
            }
        }

    }
}
