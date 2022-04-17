
using DynamicCRUD.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicCRUD.T4Templates
{
    public partial class GenericDTO
    {
        private readonly IEnumerable<ClientDatabaseColumn> DatabaseColumns;
        string ModelName;

        string Namespace = "";
        public GenericDTO(IEnumerable<ClientDatabaseColumn> databaseColumns, string modelName, string Namespace = "ARM.Data.AutoGenClasses")
        {
            this.Namespace = Namespace;
            DatabaseColumns = databaseColumns;
            ModelName = modelName;

        }

    }
}
