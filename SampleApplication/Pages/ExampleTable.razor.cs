
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
using SampleApplication.Shared;
using SampleApplication.Services;
using SampleApplication.DTOs;

namespace SampleApplication.Pages
{
    public partial class ExampleTable : ComponentBase
    {
        [Inject] public IExampleDataService? ExampleDataService { get; set; }
        [Inject] public NavigationManager? NavigationManager { get; set; }
        [Inject] public ILogger<ExampleTable>? Logger { get; set; }
        
        [Inject] public IToastService? ToastService { get; set; }
        [CascadingParameter] public IModalService? Modal { get; set; }
        public string Title { get; set; } = "Example Items (Examples)";
        public string EditTitle { get; set; } = "Edit Example Item (Examples)";
		
        [Parameter] public int NumberValue { get; set; }
        [Parameter] public int ParentId { get; set; }
        public List<ExampleDTO>? ExampleDTO { get; set; }
        public List<ExampleDTO>? FilteredExampleDTO { get; set; }
        protected ExampleAddEdit? ExampleAddEdit { get; set; }
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
        private int ExampleId  { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                if (ExampleDataService != null)
                {
                    var result = await ExampleDataService!.GetAllExamplesAsync(NumberValue);
                    //var result = await ExampleDataService.SearchExamplesAsync(ServerSearchTerm);
                    if (result != null)
                    {
                        ExampleDTO = result.ToList();
                        FilteredExampleDTO = result.ToList();
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
            FilteredExampleDTO = ExampleDTO;
            Title = $"Example ({FilteredExampleDTO?.Count})";

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
        private async Task AddNewExample()
        {
              var parameters = new ModalParameters();
			
              parameters.Add(nameof(NumberValue), NumberValue);
              var formModal = Modal?.Show<ExampleAddEdit>("Add Example", parameters);
              if (formModal != null)
              {
                  var result = await formModal.Result;
                  if (!result.Cancelled)
                  {
                      await LoadData();
                  }
              }
              ExampleId=0;
        }


        private void ApplyFilter()
        {
            if (FilteredExampleDTO == null || ExampleDTO == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(SearchTerm))
            {
                FilteredExampleDTO = ExampleDTO.OrderBy(v => v.NumberValue).ToList();
                Title = $"All Example ({FilteredExampleDTO.Count})";
            }
            else
            {
                var temporary = SearchTerm.ToLower().Trim();
                FilteredExampleDTO = ExampleDTO
                    .Where(v => 
                    (v.Text!= null  && v.Text.ToLower().Contains(temporary))
                    )
                    .ToList();
                Title = $"Filtered Examples ({FilteredExampleDTO.Count})";
            }
        }
        protected void SortExample(string sortColumn)
        {
            Guard.Against.Null(sortColumn, nameof(sortColumn));
                        if (FilteredExampleDTO == null)
            {
                return;
            }
            if (sortColumn == "NumberValue")
            {
                FilteredExampleDTO = FilteredExampleDTO.OrderBy(v => v.NumberValue).ToList();
            }
            else if (sortColumn == "NumberValue Desc")
            {
                FilteredExampleDTO = FilteredExampleDTO.OrderByDescending(v => v.NumberValue).ToList();
            }
            if (sortColumn == "Text")
            {
                FilteredExampleDTO = FilteredExampleDTO.OrderBy(v => v.Text).ToList();
            }
            else if (sortColumn == "Text Desc")
            {
                FilteredExampleDTO = FilteredExampleDTO.OrderByDescending(v => v.Text).ToList();
            }
        }
        private async Task DeleteExample(int Id)
        {
            //TODO Optionally remove child records here or warn about their existence
              var parameters = new ModalParameters();
              if (ExampleDataService != null)
              {
                  var example = await ExampleDataService.GetExampleById(Id);
                  parameters.Add("Title", "Please Confirm, Delete Example");
                  parameters.Add("Message", $"NumberValue: {example?.NumberValue}");
                  parameters.Add("ButtonColour", "danger");
                  parameters.Add("Icon", "fa fa-trash");
                  var formModal = Modal?.Show<BlazoredModalConfirmDialog>($"Delete Example ({example?.NumberValue})?", parameters);
                  if (formModal != null)
                  {
                      var result = await formModal.Result;
                      if (!result.Cancelled)
                      {
                          await ExampleDataService.DeleteExample(Id);
                          ToastService?.ShowSuccess("Example deleted successfully", "SUCCESS");
                          await LoadData();
                      }
                  }
             }
             ExampleId = Id;
        }
                  
        private async void EditExample(int Id)
        {
            var parameters = new ModalParameters();
            parameters.Add("Id", Id);
            var formModal = Modal?.Show<ExampleAddEdit>("Edit Example", parameters);
            if (formModal != null)
            {
                var result = await formModal.Result;
                if (!result.Cancelled)
                {
                    await LoadData();
                }
            }
            ExampleId = Id;
        }
            
    }
}