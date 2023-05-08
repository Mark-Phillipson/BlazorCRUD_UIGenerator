
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast;
using Blazored.Toast.Services;
using System.Security.Claims;
using Ardalis.GuardClauses;
using ARM_BlazorServer.Shared;
using ARM_BlazorServer.Services;
using ARM_BlazorServer.DTOs;

namespace ARM_BlazorServer.Pages
{
    public partial class ATableEditTable : ComponentBase
    {
        [Inject] public IATableEditDataService? ATableEditDataService { get; set; }
        [Inject] public NavigationManager? NavigationManager { get; set; }
        [Inject] public ILogger<ATableEditTable>? Logger { get; set; }
        
        [Inject] public IToastService? ToastService { get; set; }
        [CascadingParameter] public IModalService? Modal { get; set; }
        public string Title { get; set; } = "ATableEdit Items (ATableEdits)";
        public string EditTitle { get; set; } = "Edit ATableEdit Item (ATableEdits)";
        [Parameter] public int ParentId { get; set; }
        public List<ATableEditDTO>? ATableEditDTO { get; set; }
        public List<ATableEditDTO>? FilteredATableEditDTO { get; set; }
        protected ATableEditAddEdit? ATableEditAddEdit { get; set; }
        ElementReference SearchInput;
#pragma warning disable 414, 649
        private bool _loadFailed = false;
        private string? searchTerm = null;
#pragma warning restore 414, 649
        public string? SearchTerm { get => searchTerm; set { searchTerm = value; ApplyFilter(); } }
        [Parameter] public string? ServerSearchTerm { get; set; }
        public string ExceptionMessage { get; set; } = String.Empty;
        public List<string>? PropertyInfo { get; set; }
        [CascadingParameter] public ClaimsPrincipal? User { get; set; }
        [Inject] public IJSRuntime? JSRuntime { get; set; }
        public bool ShowEdit { get; set; } = false;
        private bool ShowDeleteConfirm { get; set; }
        private int ATableEditId  { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                if (ATableEditDataService != null)
                {
                    var result = await ATableEditDataService!.GetAllATableEditsAsync();
                    //var result = await ATableEditDataService.SearchATableEditsAsync(ServerSearchTerm);
                    if (result != null)
                    {
                        ATableEditDTO = result.ToList();
                        FilteredATableEditDTO = result.ToList();
                        StateHasChanged();
                    }
                }
            }
            catch (Exception e)
            {
                Logger?.LogError("Exception occurred in LoadData Method, Getting Records from the Service", e);
                _loadFailed = true;
                ExceptionMessage = e.Message;
            }
            FilteredATableEditDTO = ATableEditDTO;
            Title = $"A Table Edit ({FilteredATableEditDTO?.Count})";

        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    if (JSRuntime!= null )
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
        private async Task AddNewATableEdit()
        {
              var parameters = new ModalParameters();
              var formModal = Modal?.Show<ATableEditAddEdit>("Add A Table Edit", parameters);
              if (formModal != null)
              {
                  var result = await formModal.Result;
                  if (!result.Cancelled)
                  {
                      await LoadData();
                  }
              }
              ATableEditId=0;
        }


        private void ApplyFilter()
        {
            if (FilteredATableEditDTO == null || ATableEditDTO == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(SearchTerm))
            {
                FilteredATableEditDTO = ATableEditDTO.OrderBy(v => v.Table).ToList();
                Title = $"All A Table Edit ({FilteredATableEditDTO.Count})";
            }
            else
            {
                var temporary = SearchTerm.ToLower().Trim();
                FilteredATableEditDTO = ATableEditDTO
                    .Where(v => 
                    (v.Table!= null  && v.Table.ToLower().Contains(temporary))
                    )
                    .ToList();
                Title = $"Filtered A Table Edits ({FilteredATableEditDTO.Count})";
            }
        }
        protected void SortATableEdit(string sortColumn)
        {
            Guard.Against.Null(sortColumn, nameof(sortColumn));
                        if (FilteredATableEditDTO == null)
            {
                return;
            }
            if (sortColumn == "Table")
            {
                FilteredATableEditDTO = FilteredATableEditDTO.OrderBy(v => v.Table).ToList();
            }
            else if (sortColumn == "Table Desc")
            {
                FilteredATableEditDTO = FilteredATableEditDTO.OrderByDescending(v => v.Table).ToList();
            }
        }
        private async Task DeleteATableEdit(int TableEditId)
        {
            //TODO Optionally remove child records here or warn about their existence
              var parameters = new ModalParameters();
              if (ATableEditDataService != null)
              {
                  var aTableEdit = await ATableEditDataService.GetATableEditById(TableEditId);
                  parameters.Add("Title", "Please Confirm, Delete A Table Edit");
                  parameters.Add("Message", $"Table: {aTableEdit?.Table}");
                  parameters.Add("ButtonColour", "danger");
                  parameters.Add("Icon", "fa fa-trash");
                  var formModal = Modal?.Show<BlazoredModalConfirmDialog>($"Delete A Table Edit ({aTableEdit?.Table})?", parameters);
                  if (formModal != null)
                  {
                      var result = await formModal.Result;
                      if (!result.Cancelled)
                      {
                          await ATableEditDataService.DeleteATableEdit(TableEditId);
                          ToastService?.ShowSuccess("A Table Edit deleted successfully", "SUCCESS");
                          await LoadData();
                      }
                  }
             }
             ATableEditId = TableEditId;
        }
                  
        private async void EditATableEdit(int TableEditId)
        {
            var parameters = new ModalParameters();
            parameters.Add("TableEditId", TableEditId);
            var formModal = Modal?.Show<ATableEditAddEdit>("Edit A Table Edit", parameters);
            if (formModal != null)
            {
                var result = await formModal.Result;
                if (!result.Cancelled)
                {
                    await LoadData();
                }
            }
            ATableEditId = TableEditId;
        }
            
    }
}