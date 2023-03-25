namespace BlazorAppEditTable.Services;

public class DynamicDatabaseColumn
{
    public string ColumnName { get; set; } = "";
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
    public int Order { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string? Property { get; set; }
    public string? HelpText { get; set; }
    public bool TextEditor { get; set; }
    public string? Group { get; set; }
    public string? GroupLabel { get; set; }
    public bool Exclude { get; set; }
    public bool BatchExclude { get; set; }
    public bool UploadExclude { get; set; }
    public string? LookUp { get; set; }
    public string? LookUpLabel { get; set; }
    public int? LookUpOrder { get; set; } = null;
    public List<string> LookUpList { get; set; } = new List<string>();
    public string? LookUpType { get; set; }
    public string? LookUpSql { get; set; }
    public string? LookUpTable { get; set; }
    public bool HasValueList { get; set; } = false;
    public bool HasFieldPattern { get; set; } = false;
    public List<string> Styles { get; set; } = new List<string>() { "STD" };
    public bool DisplayInDataTable { get; set; } = true;
    public bool Searchable { get; set; } = false;
    public bool Hide { get; set; }
    public string? DefaultStringValue { get; set; }
    public bool? ClosedList { get; set; } = false;
}