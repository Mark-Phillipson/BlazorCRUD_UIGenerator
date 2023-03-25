using Ardalis.GuardClauses;
using BlazorAppEditTable.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Data;
using System.Security.Claims;

namespace BlazorAppEditTable.Pages
{
    public partial class DynamicTableTable : ComponentBase
    {
        [Parameter] public string? Sql { get; set; } = "";
        [Parameter] public string? TableName { get; set; } = "Examples";
        private bool Show { get; set; } = false;
        private bool sortAscending { get; set; } = true;
        [Inject] public IDynamicTableDataService? DynamicTableDataService { get; set; }
        [Inject] public NavigationManager? NavigationManager { get; set; }
        [Inject] public ILogger<DynamicTableTable>? Logger { get; set; }
        [Inject] public IToastService? ToastService { get; set; }
        [Inject] public ApplicationState? applicationState { get; set; }
        public string Title { get; set; } = "";
        private bool allowFilter = true;
        private int maxColumns = 15;
        private bool showExceptionMessage = false;
        public DataTable? DataTable { get; set; }
        public DataTable? FilteredDataTable { get; set; }
        ElementReference SearchInput;
#pragma warning disable 414, 649
        private bool _loadFailed = false;
        private string? searchTerm = null;
#pragma warning restore 414, 649
        private bool disableSave = true;
        public string? SearchTerm { get => searchTerm; set { searchTerm = value; ApplyFilter(); } }
        [Parameter] public string? ServerSearchTerm { get; set; }
        public string ExceptionMessage { get; set; } = string.Empty;
        public List<string>? PropertyInfo { get; set; }
        [CascadingParameter] public ClaimsPrincipal? User { get; set; }
        [Inject] public IJSRuntime? JSRuntime { get; set; }
        public IEnumerable<DynamicDatabaseColumn>? Columns { get; set; }
        public DynamicDatabaseTable? DatabaseTable { get; set; }
        public string? PrimaryKeyName { get; private set; }
        public string? Message { get; private set; }
        public string? SortColumn { get; private set; }
        bool readOnly = false;
        private int currentRow = 0;
        List<Dictionary<string, Microsoft.Extensions.Primitives.StringValues>> list = new List<Dictionary<string, Microsoft.Extensions.Primitives.StringValues>>();
        protected override void OnParametersSet()
        {
            LoadData();
        }
        protected override  void  OnInitialized()
        {
            LoadData();
        }
        private void LoadData()
        {
            if (Sql == null || !Sql.Contains($"{TableName}"))
            {
                Sql = $"SELECT * FROM [{TableName}]";
            }
            try
            {
                if (DynamicTableDataService != null && !string.IsNullOrWhiteSpace(Sql) && applicationState != null && !string.IsNullOrWhiteSpace(TableName))
                {
                    List<DataRow>? result = null;
                    DataTable = DynamicTableDataService!.GetAllDynamicTables(Sql);
                    if (!string.IsNullOrWhiteSpace(SortColumn) && Columns != null)
                    {
                        result = PerformSort();
                    }
                    if (result != null && result.Count() > 0)
                    {
                        DataTable = result.CopyToDataTable();
                    }
                    Columns = DynamicTableDataService.GetColumnNames(Sql);
                    applicationState.Columns = Columns;
                    applicationState.TableName = TableName;
                    if (Columns != null)
                    {
                        DynamicDatabaseColumn? col = Columns.Where(v => v.IsKey || v.PrimaryKeyOverride || v.IsIdentity || v.IsAutoIncrement).FirstOrDefault();
                        if (col != null)
                        {
                            PrimaryKeyName = col.ColumnName;
                            applicationState.PrimaryKeyName = col.ColumnName;
                            applicationState.PrimaryKeyDataType = col.DataType;
                            DataColumn? dataColumn = DataTable.Columns[PrimaryKeyName];
                            DataColumn[] keys = new DataColumn[1];
                            if (dataColumn != null)
                            {
                                keys[0] = dataColumn;
                                DataTable.PrimaryKey = keys;
                            }
                        }
                        else
                        {
                            ExceptionMessage = $"Unable to edit a list without a primary key from SQL: {Sql}";
                            Show = false;
                            return;
                        }
                        Message = "After editing please refresh, so the changes are shown.  Note clicking on the delete will delete record with no warning!";
                        Message = $"{Message}.  Loaded {DateTime.Now.ToLocalTime()}";

                    }
                }
            }
            catch (Exception e)
            {
                Logger?.LogError("Exception occurred in LoadData Method, Getting Records from the Service", e);
                _loadFailed = true;
                ExceptionMessage = e.Message;
            }
            FilteredDataTable = DataTable;
            Title = $"List Values {TableName} ({FilteredDataTable?.Rows?.Count})";

        }

        private List<DataRow>? PerformSort()
        {
            List<DataRow>? result = null;
            if (Columns == null || DataTable == null && SortColumn == null)
            {
                return null;
            }
            var column = Columns.FirstOrDefault(c => c.ColumnName == SortColumn);

            if (DataTable != null && SortColumn != null && column != null && column.DataType != null && column.DataType.ToLower().Contains("char"))
            {
                if (sortAscending)
                {
                    result = DataTable.AsEnumerable()
                    .OrderBy(v => v.Field<string>(SortColumn))
                    .ToList();
                }
                else
                {
                    result = DataTable.AsEnumerable()
                    .OrderByDescending(v => v.Field<string>(SortColumn))
                    .ToList();
                }
            }
            else if (SortColumn != null && DataTable != null && column != null && column.DataType != null && column.DataType.ToLower().Contains("int"))
            {
                if (sortAscending)
                {
                    result = DataTable.AsEnumerable()
                    .OrderBy(v => v.Field<int>(SortColumn))
                    .ToList();
                }
                else
                {
                    result = DataTable.AsEnumerable()
                    .OrderByDescending(v => v.Field<int>(SortColumn))
                    .ToList();
                }
            }
            return result;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    if (JSRuntime != null)
                    {
                        await JSRuntime.InvokeVoidAsync("window.setFocus", "SearchInput");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }
        protected async Task AddNewDynamicTableAsync()
        {
            Message = "";
            if (applicationState == null)
            {
                return;
            }
            bool focusSet = false;
            string? focusColumn = null;
            if (DataTable != null && Columns != null)
            {
                DataRow newRow = DataTable.NewRow();
                foreach (var column in Columns)
                {
                    if (column.DataType != null && column.DataType.ToLower().Contains("int") && column.ColumnName != applicationState.PrimaryKeyName)
                    {
                        newRow[column.ColumnName] = 0;
                    }
                    else if (column.DataType != null && column.DataType.ToLower().Contains("varchar"))
                    {
                        string defaultValue = column.DefaultStringValue ?? "";
                        newRow[column.ColumnName] = defaultValue;

                    }
                    else if (column.DataType != null && column.DataType.ToLower().Contains("bit"))
                    {
                        newRow[column.ColumnName] = false;
                    }
                    else if (column.DataType != null && column.DataType.ToLower().Contains("date"))
                    {
                        newRow[column.ColumnName] = DateTime.Now;
                    }
                    if (column.ColumnName != PrimaryKeyName && !focusSet)
                    {
                        focusColumn = column.ColumnName;
                        focusSet = true;
                    }
                }
                DataTable.Rows.Add(newRow);
                FilteredDataTable = DataTable;
                if (JSRuntime != null && focusColumn != null)
                {
                    await JSRuntime.InvokeVoidAsync("window.setFocus", $"{DataTable.Rows.Count}_{focusColumn}");
                    disableSave = false;
                }
            }
        }

        private void ApplyFilter()
        {
            if (FilteredDataTable == null || DataTable == null || Columns == null)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                LoadData();
                Title = $"All List Values {TableName} ({FilteredDataTable?.Rows?.Count})";
            }
            else
            {
                var temporary = SearchTerm.ToLower().Trim();
                var column = DataTable.Columns[1];
                {
                    if (!string.IsNullOrWhiteSpace(column.ColumnName))
                    {
                        var result = FilteredDataTable.AsEnumerable()
                                .Where(v => v.Field<string>(column.ColumnName) != null && v.Field<string>(column.ColumnName)!.ToLower().Contains(temporary));

                        if (result.Count() > 0)
                        {
                            FilteredDataTable = result.CopyToDataTable();
                        }
                        else
                        {
                            FilteredDataTable.Clear();
                            Title = $"All List Values {TableName} ({FilteredDataTable?.Rows?.Count})";
                            return;
                        }
                    }
                }
                Title = $"Filtered List Items {TableName} ({FilteredDataTable.Rows.Count})";
            }
        }
        protected void SortDynamicTable(string sortColumn)
        {
            Guard.Against.Null(sortColumn, nameof(sortColumn));
            if (FilteredDataTable == null)
            {
                return;
            }
        }
        void DeleteDynamicTable(object id)
        {
            if (DynamicTableDataService != null && DataTable != null && id != null)
            {
                DataRow? dataRow = DataTable.Rows.Find(id);
                if (dataRow != null && applicationState != null && PrimaryKeyName != null)
                {
                    //dataRow.Delete();//this actually only marks it as deleted
                    var result = DynamicTableDataService.DeleteDynamicTable(dataRow[PrimaryKeyName].ToString(), applicationState);
                    DataTable.Rows.Remove(dataRow);
                    disableSave = false;
                }
                LoadData();
            }
        }
        private async Task SaveAllAsync()
        {
            disableSave = true;
            Message = " Saving records ...";
            StateHasChanged();
            if (DataTable == null || Columns == null || DynamicTableDataService == null || applicationState == null)
            {
                return;
            }
            var result = false;
            foreach (DataRow row in DataTable.Rows)
            {
                if (row.RowState == DataRowState.Modified)
                {
                    result = DynamicTableDataService.UpdateDynamicTable(row, applicationState);
                }
                else if (row.RowState == DataRowState.Deleted)
                {
                    result = true;
                }
                else if (row.RowState == DataRowState.Added)
                {
                    result = DynamicTableDataService.AddDynamicTable(row, applicationState);
                }
                else if (row.RowState == DataRowState.Unchanged)
                {
                    result = true;
                }
                if (result == false)
                {
                    Message = "There was a problem saving the data!";
                }
            }
            if (result)
            {
                Message = "Data has been saved successfully!";
                if (JSRuntime != null)
                {
                    await JSRuntime.InvokeVoidAsync("window.setFocus", "CloseButton");
                    disableSave = true;
                }
            }
        }
        private async Task CallChangeAsync(string elementId)
        {
            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("CallChange", elementId);
            }
        }
        private void Close()
        {
            Show = false;
        }
        private void ToggleShow()
        {
            Show = !Show;
            if (Show) LoadData();
        }
        public void DataTableBind(ChangeEventArgs __e, string? key, int row, string columnName)
        {
            if (DataTable != null)
            {
                DataRow dataRow = DataTable.Rows[row - 1];
                dataRow.BeginEdit();

                try
                {
                    dataRow[columnName] = __e.Value;
                    dataRow.EndEdit();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    Message = exception.Message;
                    dataRow[columnName] = DBNull.Value;
                }

                disableSave = false;
                Message = "";
            }
        }
        private async Task CopyDynamicTableAsync(object id)
        {
            bool focusSet = false;
            string? focusColumn = null;
            if (DataTable != null && Columns != null)
            {
                DataRow? copyRow = DataTable.Rows.Find(id);
                if (copyRow == null) return;
                DataRow newRow = DataTable.NewRow();
                foreach (var column in Columns)
                {
                    if (column.ColumnName != PrimaryKeyName)
                    {
                        newRow[column.ColumnName] = copyRow[column.ColumnName];
                    }
                    if (column.ColumnName != PrimaryKeyName && !focusSet)
                    {
                        focusColumn = column.ColumnName;
                        focusSet = true;
                    }
                }
                DataTable.Rows.Add(newRow);
                FilteredDataTable = DataTable;
                if (JSRuntime != null && focusColumn != null)
                {
                    await JSRuntime.InvokeVoidAsync("window.setFocus", $"{DataTable.Rows.Count}_{focusColumn}");
                    disableSave = false;
                }
            }
        }
        private void Sort(string columnName)
        {
            if (FilteredDataTable != null)
            {
                SortColumn = $"{columnName}";
            }
            sortAscending = !sortAscending;
            LoadData();
        }
        private void ToggleExceptionMessage()
        {
            showExceptionMessage = !showExceptionMessage;
        }
    }
}