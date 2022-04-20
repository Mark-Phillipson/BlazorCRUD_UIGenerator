
using DynamicCRUD.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCRUD.T4Templates
{
    public partial class GenericAddEditCodeBehind
    {
        private readonly IEnumerable<ClientDatabaseColumn> DatabaseColumns;
        string PrimaryKeyDataType { get; set; }
        string ModelName { get; set; }
        string PluralTablename { get; set; }
        string Namespace { get; set; } = "";
        string ModelNameCamelCase { get; }
        string PrimaryKeyName { get; set; } = "";
        public string? DefaultSortColumn { get; set; }="DefaultSortColumn";
		public string? ModelNameWithSpaces { get; set; }="ModelNameWithSpaces";
		public string? PrimaryKeyNameCamelCase { get; set; }="";
		public string ForeignKeyName { get; set; }="";
		public string ForeignKeyDataType { get; set; }="";

        public GenericAddEditCodeBehind(IEnumerable<ClientDatabaseColumn> databaseColumns, string modelName, string modelNameCamelCase, string pluralTablename, string primaryKeyName, string primaryKeyDataType, string Namespace,string foreignKeyName,string foreignKeyDataType)
        {
            this.Namespace = Namespace;
            DatabaseColumns = databaseColumns;
            ModelName = modelName;
            ModelNameCamelCase = modelNameCamelCase;
            PluralTablename = pluralTablename;
            PrimaryKeyName = primaryKeyName;
            PrimaryKeyDataType = primaryKeyDataType;
			ModelNameWithSpaces=StringHelperService.AddSpacesToSentence(modelName);
			PrimaryKeyNameCamelCase=StringHelperService.GetCamelCase(primaryKeyName);
			ForeignKeyName=foreignKeyName;
			ForeignKeyDataType=foreignKeyDataType;
            var result = databaseColumns.FirstOrDefault(c => c.Sort == true);
            if (result != null && result.PropertyName != null)
            {
                DefaultSortColumn = result.PropertyName;
            }
        }

    }
}
