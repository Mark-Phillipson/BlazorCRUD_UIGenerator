using Blazored.Toast.Services;
using DynamicCRUD.Services;
// using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using DynamicCRUD.T4Templates;
using Humanizer;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using System.Reflection;

namespace DynamicCRUD.Pages;

public partial class BlazorCRUDGeneration : ComponentBase
{
    public string? Tablename { get => tablename; set { tablename = value; } }
    public string? ModelName { get; set; }
    public string SchemaName { get; set; } = "dbo";// Usually dbo
    public string? PluralName { get; set; }
    public string? DbContextName { get; set; } = "MyDbContext";
    public string? AutoMapperCode { get; set; } = "";
    private ElementReference AutoMapperCodeElement { get; set; }
    public string? DependencyInjectionCode { get; set; } = "";
    private ElementReference DependencyInjectionCodeElement { get; set; }
    private bool UseBlazored { get; set; } = true;
    [Inject] public IJSRuntime? JSRuntime { get; set; }
    [Inject] IToastService? ToastService { get; set; }
    [Inject] DatabaseMetaDataService? DatabaseMetaDataService { get; set; }
    [Inject] IConfiguration? Configuration { get; set; }
    IEnumerable<ClientDatabaseTable> databaseTables = new List<ClientDatabaseTable>();
    IEnumerable<ClientDatabaseColumn> Columns { get; set; } = new List<ClientDatabaseColumn>();
    private string? tablename;
    public string? Message { get; set; }
    public bool ShowInstructions { get; set; }
    string ConnectionString { get; set; } = null!;
    public string SearchString { get; private set; } = "";
    public string PopulateColumnsCaption { get; set; } = "Populate Columns P";
    public string NamespaceName { get; set; } = "SampleApplication";

    protected override async Task OnInitializedAsync()
    {
        if (DatabaseMetaDataService != null && Configuration != null)
        {
            ConnectionString = Configuration.GetConnectionString("DefaultConnection") ?? "";
            databaseTables = DatabaseMetaDataService.GetDatabaseList(ConnectionString).Where(w => w.Tablename != null && w.Tablename.ToLower().Contains(SearchString.ToLower()));
        }
        UseBlazored = true;
        await base.OnInitializedAsync();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                if (JSRuntime != null)
                {
                    await JSRuntime.InvokeVoidAsync("window.setFocus", "SearchString");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
    private void GenerateClasses()
    {
        Message = null; ShowInstructions = false;
        ArgumentNullException.ThrowIfNull(ModelName);
        if (string.IsNullOrWhiteSpace(Tablename) || DatabaseMetaDataService == null || ConnectionString == null || PluralName == null || ModelName == null)
        {
            Message = "Please select a table and fill in all the details, then try again!";
            return;
        }
        if (!Columns.Any(c => (c.IsKey || c.PrimaryKeyOverride) && (c.DataType == "bigint" || c.DataType == "int" || c.DataType == "nvarchar")))
        {
            Message = "Please indicate the primary key and try again! Note it has to be an int or nvarchar";
            return;
        }
        if (ModelName != null && ModelName.ToLower().EndsWith("s"))
        {
            Message = "Please make sure the model name is not plural!";
        }
        if (!Columns.Any(c => c.Sort == true))
        {
            Message = "Please select at least one column to sort by! ";
            return;
        }
        AutoMapperCode = $"CreateMap<{ModelName}, {ModelName}DTO>();\r\n" +
            $"CreateMap<{ModelName}DTO, {ModelName}>();";
        DependencyInjectionCode = $"builder.Services.AddScoped<I{ModelName}Repository, {ModelName}Repository>();\r\n " +
            $"builder.Services.AddScoped<I{ModelName}DataService, {ModelName}DataService>();";
        string primaryKeyName = ""; string primaryKeyDatatype = "";
        GetPrimaryKeyDetails(ref primaryKeyName, ref primaryKeyDatatype);
        string filterColumns = "";
        GetFilterColumns(ref filterColumns);
        string? foreignKeyName = ""; string? foreignKeyDataType = "";
        var columnForeignKey = Columns.FirstOrDefault(f => f.ForeignKey == true);
        if (columnForeignKey != null)
        {
            foreignKeyName = columnForeignKey.ColumnName;
            foreignKeyDataType = columnForeignKey.DataType;
            if (foreignKeyDataType == "nvarchar")
            {
                foreignKeyDataType = "string";
            }
        }
        var namespaceString = $"{NamespaceName}";
        var tablename = Tablename.Replace($"{SchemaName}.", "").Replace("/", "");
        string locationBlazor, locationModels, locationRepository;
        PrepareLocations(out locationBlazor, out locationModels, out locationRepository);
        string content = "";
        if (false) //Usuually this already exists After reverse engineering the database
        {
            // GenericModel genericModel = new(Columns, tablename, namespaceString, ModelName);
            // content = genericModel.TransformText();
            // tablename = tablename.Replace("_", "");
            // File.WriteAllText($"{locationModels}AutoGenClasses\\{ModelName}.cs", content);
        }

        GenericDTO genericDTO = new(Columns, ModelName!, namespaceString);
        content = genericDTO.TransformText();
        File.WriteAllText($"{locationModels}AutoGenClasses\\{ModelName}DTO.cs", content);

        var camelTablename = StringHelperService.GetCamelCase(ModelName!);
        namespaceString = $"{NamespaceName}";

        GenericIRepository genericIRepository = new(Columns, ModelName!, camelTablename, PluralName, primaryKeyName, primaryKeyDatatype, namespaceString, foreignKeyName, foreignKeyDataType);
        content = genericIRepository.TransformText();
        File.WriteAllText($"{locationRepository}AutoGenClasses\\I{ModelName}Repository.cs", content);

        GenericRepository genericRepository = new(Columns, ModelName!, camelTablename, PluralName, primaryKeyName, primaryKeyDatatype, namespaceString, foreignKeyName ?? "", foreignKeyDataType ?? "", DbContextName ?? "BostonAcademicDbContext");
        content = genericRepository.TransformText();
        File.WriteAllText($"{locationRepository}AutoGenClasses\\{ModelName}Repository.cs", content);

        namespaceString = $"{NamespaceName}";

        GenericIDataService genericIDataService = new(Columns, ModelName!, camelTablename, PluralName, primaryKeyName, primaryKeyDatatype, namespaceString, foreignKeyName ?? "", foreignKeyDataType ?? "");
        content = genericIDataService.TransformText();
        File.WriteAllText($"{locationBlazor}AutoGenClasses\\I{ModelName}DataService.cs", content);

        GenericDataService genericDataService = new(Columns, ModelName!, camelTablename, PluralName, primaryKeyName, primaryKeyDatatype, namespaceString, foreignKeyName ?? "", foreignKeyDataType ?? "");
        content = genericDataService.TransformText();
        File.WriteAllText($"{locationBlazor}AutoGenClasses\\{ModelName}DataService.cs", content);

        GenericTable genericTable = new(Columns, ModelName!, camelTablename, PluralName, primaryKeyName, primaryKeyDatatype, namespaceString, filterColumns, foreignKeyName ?? "", foreignKeyDataType ?? "", UseBlazored);
        content = genericTable.TransformText();
        File.WriteAllText($"{locationBlazor}AutoGenClasses\\{ModelName}Table.razor", content);

        GenericTableCodeBehind genericTableCodeBehind = new(Columns, ModelName!, camelTablename, PluralName, primaryKeyName, primaryKeyDatatype, namespaceString, foreignKeyName ?? "", foreignKeyDataType ?? "", UseBlazored);
        content = genericTableCodeBehind.TransformText();
        File.WriteAllText($"{locationBlazor}AutoGenClasses\\{ModelName}Table.razor.cs", content);

        GenericAddEdit genericAddEdit = new(Columns, ModelName!, camelTablename, PluralName, primaryKeyName, primaryKeyDatatype, namespaceString, filterColumns, foreignKeyName ?? "", foreignKeyDataType ?? "", UseBlazored);
        content = genericAddEdit.TransformText();
        File.WriteAllText($"{locationBlazor}AutoGenClasses\\{ModelName}AddEdit.razor", content);

        GenericAddEditCodeBehind genericAddEditCodeBehind = new(Columns, ModelName!, camelTablename, PluralName, primaryKeyName, primaryKeyDatatype, namespaceString, foreignKeyName ?? "", foreignKeyDataType ?? "", UseBlazored);
        content = genericAddEditCodeBehind.TransformText();
        File.WriteAllText($"{locationBlazor}AutoGenClasses\\{ModelName}AddEdit.razor.cs", content);


        ShowInstructions = true;
    }

    private static void PrepareLocations(out string locationBlazor, out string locationModels, out string locationRepository)
    {
        var location = Assembly.GetExecutingAssembly().Location;
        locationBlazor = location.Substring(0, location.IndexOf("DynamicCRUD") + 12);
        locationModels = locationBlazor;
        locationRepository = locationBlazor;
    }

    private void GetFilterColumns(ref string filterColumns)
    {
        var result = "";
        foreach (var column in Columns)
        {
            if (column.Filter)
            {
                if (result?.Length > 0)
                {
                    result = $"{result}/{column.Label}";
                }
                else
                {
                    result = column.Label;
                }
            }
        }
        filterColumns = result!;
    }

    private void GetPrimaryKeyDetails(ref string primaryKeyName, ref string primaryKeyDatatype)
    {
        var column = Columns.FirstOrDefault(f => f.IsIdentity == true && f.IsAutoIncrement == true || f.IsKey == true || f.PrimaryKeyOverride == true);
        if (column != null && column.ColumnName != null)
        {
            primaryKeyName = StringHelperService.RemoveUnsupportedCharacters(column.ColumnName).Replace("ID", "Id");
            if (column?.DataType != null)
            {
                primaryKeyDatatype = column.DataType;
                if (primaryKeyDatatype == "nvarchar") primaryKeyDatatype = "string";
            }
        }
    }

    string GetNamespaceName(string connectionString)
    {
        if (connectionString.ToLower().Contains("arm_core"))
        {
            return "ARM_BlazorServer";
        }
        return "SampleApplication";
    }
    string GetDbContextName(string connectionString)
    {
        if (connectionString.ToLower().Contains("arm_core"))
        {
            return "ARMDbContext";
        }
        return "MyDbContext";
    }
    private void PopulateColumns()
    {
        PopulateColumnsCaption = "Please Wait";
        if (DatabaseMetaDataService == null || ConnectionString == null || Tablename == null)
        {
            return;
        }
        if (Tablename.Replace($"{SchemaName}.", "").Contains('.'))
        {
            Message = "System does not support table names with dots in the name!";
            return;
        }
        NamespaceName = GetNamespaceName(ConnectionString);
        DbContextName = GetDbContextName(ConnectionString);
        PluralName = $"{tablename}";
        PluralName = PluralName.Replace($"{SchemaName}.", "").Replace("_", "");
        PluralName = StringHelperService.RemoveUnsupportedCharacters(PluralName);
        PluralName = PluralName.Pluralize();
        ModelName = $"{tablename}";
        ModelName = ModelName.Replace($"{SchemaName}.", "").Replace("_", "");
        ModelName = ModelName.Singularize();
        if (ModelName.ToLower().EndsWith("s"))
        {
            Message = "Please check the model is Not Plural";
        }
        ModelName = StringHelperService.RemoveUnsupportedCharacters(ModelName);
        Columns = DatabaseMetaDataService.GetColumnNames(ConnectionString, Tablename, SchemaName, ModelName);
        PopulateColumnsCaption = "Populate Columns";
    }
    private async Task CallChangeAsync(string elementId)
    {
        if (JSRuntime != null)
        {
            await JSRuntime.InvokeVoidAsync("CallChange", elementId);
        }
    }
    public async Task CopyAsync(string value)
    {
        if (JSRuntime == null)
        {
            return;
        }
        await JSRuntime.InvokeVoidAsync(
"clipboardCopy.copyText", value);
        var message = $"copy commandline Copied Successfully: '{value}'";
        ToastService!.ShowSuccess(message);
    }
    private async Task FocusAutoMapperCode()
    {
        await AutoMapperCodeElement.FocusAsync();
    }
    private async Task FocusDependencyInjectionCode()
    {
        await DependencyInjectionCodeElement.FocusAsync();
    }
    private void ReverseEngineerTable()
    {
        //Turns out this is not viable as it will only create a new DB context not add to an existing one
        //--context ARM.Data.Data.ARMDbContext does not work when this is supplied
        //ReverseEngineerDatabaseCommand = $"dotnet ef dbcontext scaffold \"{ConnectionString}\" Microsoft.EntityFrameworkCore.SqlServer --context-dir Data --output-dir AutoGenModels --table {Tablename}";
        //Cannot run while the web application is running
        //DatabaseMetaDataService.ExecuteCommand(command);

    }
}