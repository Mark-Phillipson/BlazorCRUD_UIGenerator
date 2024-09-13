
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

        public GenericTableCodeBehind(IEnumerable<ClientDatabaseColumn> databaseColumns, string modelName, string modelNameCamelCase, string pluralTablename, string primaryKeyName, string primaryKeyDataType, string Namespace, string foreignKeyName, string foreignKeyDataType, bool useBlazored, bool useRadzen)
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
            ModelNameWithSpaces = StringHelperService.AddSpacesToSentence(modelName);
            var result = databaseColumns.FirstOrDefault(c => c.Sort == true);
            if (result != null && result.PropertyName != null)
            {
                DefaultSortColumn = result.PropertyName;
            }
        }

    }
}
