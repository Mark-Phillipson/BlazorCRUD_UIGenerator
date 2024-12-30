using Blazored.Toast.Services;
using DynamicCRUD.Services;
using DynamicCRUD.T4Templates;
using Humanizer;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace DynamicCRUD.Pages;

public partial class BlazorCRUDGeneration : ComponentBase
{
    public string? Tablename { get => tablename; set { tablename = value; } }
    public string? ModelName { get; set; }
    public string SchemaName { get; set; } = "dbo";// Usually dbo
    public string? PluralName { get; set; }
    public string? DbContextName { get; set; } = "ApplicationDbContext";
    public string? AutoMapperCode { get; set; } = "";
    private ElementReference AutoMapperCodeElement { get; set; }
    public string? DependencyInjectionCode { get; set; } = "";
    private ElementReference DependencyInjectionCodeElement { get; set; }
    private bool UseBlazored { get; set; } = false;
    private bool UseRadzen { get; set; } = false;
    [Inject] public required IJSRuntime JSRuntime { get; set; }
    [Inject] public required IToastService ToastService { get; set; }
    [Inject] public required DatabaseMetaDataService DatabaseMetaDataService { get; set; }
    [Inject] public required IConfiguration Configuration { get; set; }
    IEnumerable<ClientDatabaseTable> databaseTables = new List<ClientDatabaseTable>();
    IEnumerable<ClientDatabaseColumn> Columns { get; set; } = new List<ClientDatabaseColumn>();
    private string? tablename;
    private string? infoMessage;
    private bool showConfirmDialog = false;
    private string? filesCreatedMessage;
    public string? Message { get; set; }
    public bool ShowInstructions { get; set; }
    string ConnectionString { get; set; } = null!;
    public string SearchString { get; private set; } = "";
    public string PopulateColumnsCaption { get; set; } = "Populate Columns";
    public string? RazorNamespaceName { get; set; } = "TBC";
    public string? DTONamespaceName { get; set; } = "TBC";
    public string? RepositoryNamespaceName { get; set; } = "TBC";
    public string? DataServiceNamespaceName { get; set; } = "TBC";
    public string? LocationRazor { get; set; } = "";
    public string? LocationDTO { get; set; } = "";
    public string? LocationRepository { get; set; } = "";
    public string? LocationDataService { get; set; } = "";
    public List<string> Projects { get; set; } = new List<string>();
    public string SelectedProject { get; set; } = "MarvinMeyers";
    protected override async Task OnInitializedAsync()
    {
        Projects = GetProjects();
        SetupDatabaseProperties(SelectedProject);
        UseBlazored = false;
        await base.OnInitializedAsync();
    }

    private List<string> GetProjects()
    {
        string projectsMappingJson = File.ReadAllText("projectMappings.json");
        var projectMappings = JsonSerializer.Deserialize<ProjectMapping>(projectsMappingJson);
        var projects = projectMappings?.Projects?.Select(s => s.DatabaseName).ToList();
        if (projects != null && projects.Count > 0)
        {
            return projects!.Where(p => p != null).Cast<string>().OrderBy(x => x).ToList();
        }
        return new List<string>();
    }

    private void SetupDatabaseProperties(string project)
    {
        SelectedProject = project;
        ConnectionString = Configuration.GetConnectionString(project) ?? "";
        var result = GetNameSpaceAndLocations(ConnectionString, project);
        if (!result)
        {
            Message = "Something Went Wrong getting namespaces and locations! Please check the connection string and projectMappings.json then try again!";
        }
        else
        {
            databaseTables = DatabaseMetaDataService.GetDatabaseList(ConnectionString).Where(w => w.Tablename != null && w.Tablename.ToLower().Contains(SearchString.ToLower()));
        }
    }

    private bool GetNameSpaceAndLocations(string connectionString, string? projectName = null)
    {
        // Extract the initial catalog from the connection string
        var initialCatalogue = "";
        var connectionStringParts = connectionString.Split(";");
        foreach (var part in connectionStringParts)
        {
            if (part.ToLower().Contains("initial catalog") || part.ToLower().Contains("database"))
            {
                initialCatalogue = part.Split("=")[1];
                break;
            }
        }
        if (string.IsNullOrEmpty(initialCatalogue))
        {
            return false;
        }

        // Read and deserialize the projectsMapping.json file
        var projectsMappingJson = "";
        try
        {
            projectsMappingJson = File.ReadAllText("projectMappings.json");
            Console.WriteLine("JSON Content: " + projectsMappingJson);
        }
        catch (System.Exception exception)
        {
            System.Console.WriteLine(exception.Message);
            return false;
        }
        var projectMappings = new ProjectMapping();
        try
        {
            projectMappings = JsonSerializer.Deserialize<ProjectMapping>(projectsMappingJson);
            if (projectMappings == null)
            {
                Console.WriteLine("Deserialization returned null.");
                return false;
            }
            else
            {
                Console.WriteLine("Deserialization successful.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Deserialization error: " + ex.Message);
            return false;
        }

        // Find the project that matches the initial catalog
        Project? project = null;
        try
        {
            if (projectName != null)
            {
                project = projectMappings?.Projects?.FirstOrDefault(p => p.DatabaseName!.Equals(projectName, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                project = projectMappings?.Projects?.FirstOrDefault(p => p.DatabaseName!.Equals(initialCatalogue, StringComparison.OrdinalIgnoreCase));
            }

        }
        catch (System.Exception exception)
        {
            System.Console.WriteLine(exception.Message);
            return false;
        }
        if (project == null)
        {
            return false;
        }

        // Map the values to the appropriate properties
        RazorNamespaceName = project.Namespaces?.RazorNamespace;
        DTONamespaceName = project.Namespaces?.DtoNamespace;
        DataServiceNamespaceName = project.Namespaces?.DataServiceNamespace;
        RepositoryNamespaceName = project.Namespaces?.RepositoryNamespace;

        LocationRazor = project.Folders?.RazorFolder;
        LocationDTO = project.Folders?.DtoFolder;
        LocationDataService = project.Folders?.DataServiceFolder;
        LocationRepository = project.Folders?.RepositoryFolder;

        return true;
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
        var valid = ValidateInputRequirements();
        if (valid == false)
        {
            return;
        }
        AutoMapperCode = $"CreateMap<{ModelName}, {ModelName}DTO>();" + Environment.NewLine +
            $"CreateMap<{ModelName}DTO, {ModelName}>();";
        DependencyInjectionCode = $"builder.Services.AddScoped<I{ModelName}Repository, {ModelName}Repository>(); " + Environment.NewLine +
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
        var tablename = Tablename!.Replace($"{SchemaName}.", "").Replace("/", "");
        string content = "";
        filesCreatedMessage = "";
        GenericDTO genericDTO = new(Columns, ModelName!, DTONamespaceName ?? "DTO_Namespace");
        content = genericDTO.TransformText();
        File.WriteAllText($"{LocationDTO}\\{ModelName}DTO.cs", content);
        filesCreatedMessage = $" {ModelName}DTO.cs";
        var camelTablename = StringHelperService.GetCamelCase(ModelName!);

        GenericIRepository genericIRepository = new(Columns, ModelName!, camelTablename, PluralName!, primaryKeyName, primaryKeyDatatype, RepositoryNamespaceName ?? "Repository_Namespace", foreignKeyName, foreignKeyDataType);
        content = genericIRepository.TransformText();
        File.WriteAllText($"{LocationRepository}\\I{ModelName}Repository.cs", content);
        filesCreatedMessage = $"{filesCreatedMessage}{Environment.NewLine} I{ModelName}Repository.cs";

        GenericRepository genericRepository = new(Columns, ModelName!, camelTablename, PluralName!, primaryKeyName, primaryKeyDatatype, RepositoryNamespaceName ?? "Repository_Namespace", foreignKeyName ?? "", foreignKeyDataType ?? "", DbContextName ?? "ApplicationDbContext");
        content = genericRepository.TransformText();
        File.WriteAllText($"{LocationRepository}\\{ModelName}Repository.cs", content);
        filesCreatedMessage = $"{filesCreatedMessage}{Environment.NewLine} {ModelName}Repository.cs";

        GenericIDataService genericIDataService = new(Columns, ModelName!, camelTablename, PluralName!, primaryKeyName, primaryKeyDatatype, DataServiceNamespaceName ?? "DataService_Namespace", foreignKeyName ?? "", foreignKeyDataType ?? "", DTONamespaceName ?? "DTO_Namespace");
        content = genericIDataService.TransformText();
        File.WriteAllText($"{LocationDataService}\\I{ModelName}DataService.cs", content);
        filesCreatedMessage = $"{filesCreatedMessage}{Environment.NewLine} I{ModelName}DataService.cs";

        GenericDataService genericDataService = new(Columns, ModelName!, camelTablename, PluralName!, primaryKeyName, primaryKeyDatatype, DataServiceNamespaceName ?? "DataService_Namespace", foreignKeyName ?? "", foreignKeyDataType ?? "", DTONamespaceName ?? "DTO_Namespace");
        content = genericDataService.TransformText();
        File.WriteAllText($"{LocationDataService}\\{ModelName}DataService.cs", content);
        filesCreatedMessage = $"{filesCreatedMessage}{Environment.NewLine} {ModelName}DataService.cs";

        GenericTable genericTable = new(Columns, ModelName!, camelTablename, PluralName!, primaryKeyName, primaryKeyDatatype, RazorNamespaceName ?? "Razor_Namespace", filterColumns, foreignKeyName ?? "", foreignKeyDataType ?? "", UseBlazored, UseRadzen, DataServiceNamespaceName ?? "DataService_Namespace", RepositoryNamespaceName ?? "Repository_Namespace");
        content = genericTable.TransformText();
        File.WriteAllText($"{LocationRazor}\\{ModelName}Table.razor", content);
        filesCreatedMessage = $"{filesCreatedMessage}{Environment.NewLine} {ModelName}Table.razor";

        GenericTableCodeBehind genericTableCodeBehind = new(Columns, ModelName!, camelTablename, PluralName!, primaryKeyName, primaryKeyDatatype, RazorNamespaceName ?? "Razor_Namespace", foreignKeyName ?? "", foreignKeyDataType ?? "", UseBlazored, UseRadzen, DataServiceNamespaceName ?? "DataService_Namespace", RepositoryNamespaceName ?? "Repository_Namespace", DTONamespaceName ?? "DTO_Namespace");
        content = genericTableCodeBehind.TransformText();
        File.WriteAllText($"{LocationRazor}\\{ModelName}Table.razor.cs", content);
        filesCreatedMessage = $"{filesCreatedMessage}{Environment.NewLine} {ModelName}Table.razor.cs";

        GenericAddEdit genericAddEdit = new(Columns, ModelName!, camelTablename, PluralName!, primaryKeyName, primaryKeyDatatype, RazorNamespaceName ?? "Razor_Namespace", filterColumns, foreignKeyName ?? "", foreignKeyDataType ?? "", UseBlazored, UseRadzen, ModelName ?? "");
        content = genericAddEdit.TransformText();
        File.WriteAllText($"{LocationRazor}\\{ModelName}AddEdit.razor", content);
        filesCreatedMessage = $"{filesCreatedMessage}{Environment.NewLine} {ModelName}AddEdit.razor";

        GenericAddEditCodeBehind genericAddEditCodeBehind = new(Columns, ModelName!, camelTablename, PluralName!, primaryKeyName, primaryKeyDatatype, RazorNamespaceName ?? "Razor_Namespace", foreignKeyName ?? "", foreignKeyDataType ?? "", UseBlazored, UseRadzen, DTONamespaceName ?? "DTO_Namespace", DataServiceNamespaceName ?? "DataService_Namespace");
        content = genericAddEditCodeBehind.TransformText();
        File.WriteAllText($"{LocationRazor}\\{ModelName}AddEdit.razor.cs", content);
        filesCreatedMessage = $"{filesCreatedMessage}{Environment.NewLine} {ModelName}AddEdit.razor.cs";


        ShowInstructions = true;
    }

    private bool ValidateInputRequirements()
    {
        if (string.IsNullOrWhiteSpace(Tablename) || DatabaseMetaDataService == null || ConnectionString == null || PluralName == null || ModelName == null)
        {
            Message = "Please select a table and fill in all the details, then try again!";
            return false;
        }
        if (!Columns.Any(c => (c.IsKey || c.PrimaryKeyOverride) && (c.DataType == "bigint" || c.DataType == "int" || c.DataType == "nvarchar")))
        {
            Message = "Please indicate the primary key and try again! Note it has to be an int or nvarchar";
            return false;
        }
        if (ModelName != null && ModelName.ToLower().EndsWith("s"))
        {
            Message = "Please make sure the model name is not plural!";
        }
        if (!Columns.Any(c => c.Sort == true))
        {
            Message = "Please select at least one column to sort by! ";
            return false;
        }
        if (!Directory.Exists(LocationRazor))
        {
            Message = $"The location {LocationRazor} does not exist!";
            return false;
        }
        if (!Directory.Exists(LocationDTO))
        {
            Message = $"The location {LocationDTO} does not exist!";
            return false;
        }
        if (!Directory.Exists(LocationRepository))
        {
            Message = $"The location {LocationRepository} does not exist!";
            return false;
        }
        if (!Directory.Exists(LocationDataService))
        {
            Message = $"The location {LocationDataService} does not exist!";
            return false;
        }
        return true;
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
        else if (connectionString.ToLower().Contains("bostonacademic"))
        {
            return "BostonAcademic.Client";
        }
        else if (connectionString.ToLower().Contains("voicelauncher"))
        {
            return "RazorClassLibrary";
        }
        else if (connectionString.ToLower().Contains("templatedatabase"))
        {
            return "BlazorApp.Template";
        }
        else if (connectionString.ToLower().Contains("packtex"))
        {
            return "PacktexBlazorServer";
        }
        return "SampleApplication";
    }
    string GetDbContextName(string connectionString)
    {
        if (connectionString.ToLower().Contains("arm_core"))
        {
            return "ARMDbContext";
        }
        else if (connectionString.ToLower().Contains("bostonAcademic"))
        {
            return "BostonAcademicDbContext";
        }
        else if (connectionString.ToLower().Contains("voiceLauncher"))
        {
            return "ApplicationDbContext";
        }
        else if (connectionString.ToLower().Contains("templateDatabase"))
        {
            return "ApplicationDbContext";
        }
        else if (connectionString.ToLower().Contains("packtex"))
        {
            return "PacktexContext";
        }
        return "ApplicationDbContext";
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
        await JSRuntime.InvokeVoidAsync("copyToClipboard", value);
        infoMessage = $"Copied to Clipboard: '{value}'";
    }
    private async Task FocusAutoMapperCode()
    {
        await AutoMapperCodeElement.FocusAsync();
    }
    private async Task FocusDependencyInjectionCode()
    {
        await DependencyInjectionCodeElement.FocusAsync();
    }
    private void OpenConfigurationFile(string file)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), file);
        if (File.Exists(path))
        {
            try
            {
                var visualStudioPath = @"C:\Users\MPhil\AppData\Local\Programs\Microsoft VS Code\Code.exe"; // Update this path to your Visual Studio executable
                if (File.Exists(visualStudioPath))
                {
                    // Run a terminal command to open the file in Visual Studio Code
                    var processStartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c code \"{path}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using (var process = Process.Start(processStartInfo))
                    {
                        process!.WaitForExit();
                    }
                    Message = "File opened in Visual Studio Code.";
                }
            }
            catch (System.Exception exception)
            {
                Message = exception.Message;
            }
        }
        else
        {
            Message = "File not found!";
        }
    }
    private void SynchronizeUseOptions()
    {
        if (UseBlazored)
        {
            UseRadzen = false;
            StateHasChanged();
        }
        if (UseRadzen)
        {
            UseBlazored = false;
            StateHasChanged();
        }

    }
    private void ShowConfirmDialog()
    {
        showConfirmDialog = true;

    }
    private void OnConfirmDialogResponse(bool confirmed)
    {
        if (confirmed)
        {
            GenerateClasses();
        }
        showConfirmDialog = false;
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