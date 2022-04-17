namespace DynamicCRUD.Services
{
    public class ClientDatabaseColumn
    {
        public string? ColumnName { get; set; }
        public string? PropertyName { get; set; }
        public string? DataType { get; set; }
        public int ColumnSize { get; set; }
        public bool Required { get; set; } = false;
        public bool IsAutoIncrement { get; set; } = false;
        public bool IsIdentity { get; set; } = false;
        public bool IsKey { get; set; } = false;
        public bool Filter { get; set; } = false;
        public bool PrimaryKeyOverride { get; set; }
        public bool Sort { get; set; } = false;
        public string? Label { get; set; }
        public bool ForeignKey { get; set; }= false;
    }
}