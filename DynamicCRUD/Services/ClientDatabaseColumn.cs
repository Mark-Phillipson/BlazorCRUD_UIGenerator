namespace DynamicCRUD.Services
{
    public class ClientDatabaseColumn
    {
        public string? ColumnName { get; set; }
        public string? PropertyName { get; set; }
        public string? DataType { get; set; }
        public int ColumnSize { get; set; }
        public bool Required { get; set; }
        public bool IsAutoIncrement { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsKey { get; set; }
        public bool Filter { get; set; }
        public bool PrimaryKeyOverride { get; set; }
        public bool Sort { get; set; }
        public string? Label { get; set; }
    }
}