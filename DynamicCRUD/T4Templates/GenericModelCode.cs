
using DynamicCRUD.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCRUD.T4Templates
{
    public partial class GenericModel
    {
        private readonly IEnumerable<ClientDatabaseColumn> DatabaseColumns;
        string TableName { get; set; }
        string ModelName;
        string OriginalTableName;
        string Namespace = "";
        public GenericModel(IEnumerable<ClientDatabaseColumn> databaseColumns, string tableName, string Namespace, string modelName)
        {
            this.Namespace = Namespace;
            DatabaseColumns = databaseColumns;
            TableName = tableName;
            OriginalTableName = tableName;
            ModelName = modelName.Replace("_", "");
        }

    }
}
