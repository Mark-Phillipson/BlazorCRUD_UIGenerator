
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
        [Inject] public required IExampleDataService ExampleDataService { get; set; }
        [Inject] public required  NavigationManager NavigationManager { get; set; }
        [Inject] public ILogger<ExampleTable>? Logger { get; set; }
        
        [Inject] public required IToastService ToastService { get; set; }
        [CascadingParameter] public IModalService? Modal { get; set; }
        public string Title { get; set; } = "Example Items (Examples)";
        public string EditTitle { get; set; } = "Edit Example Item (Examples)";
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
        private int pageNumber = 1;
        private int pageSize = 20;
        private int totalRows = 0;
    
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
                    totalRows = await ExampleDataService.GetTotalCount();
                    var result = await ExampleDataService!.GetAllExamplesAsync
                    
                    (pageNumber,pageSize);
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
                Logger?.LogError("e, Exception occurred in LoadData Method, Getting Records from the Service");
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
                FilteredExampleDTO = ExampleDTO.OrderBy(v => v.Name).ToList();
                Title = $"All Example ({FilteredExampleDTO.Count})";
            }
            else
            {
                var temporary = SearchTerm.ToLower().Trim();
                FilteredExampleDTO = ExampleDTO
                    .Where(v => 
                    (v.Name!= null  && v.Name.ToLower().Contains(temporary))
                     || (v.Description!= null  &&  v.Description.ToLower().Contains(temporary))
                     || (v.CreatedBy!= null  &&  v.CreatedBy.ToLower().Contains(temporary))
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
            if (sortColumn == "Name")
            {
                FilteredExampleDTO = FilteredExampleDTO.OrderBy(v => v.Name).ToList();
            }
            else if (sortColumn == "Name Desc")
            {
                FilteredExampleDTO = FilteredExampleDTO.OrderByDescending(v => v.Name).ToList();
            }
            if (sortColumn == "DateCreated")
            {
                FilteredExampleDTO = FilteredExampleDTO.OrderBy(v => v.DateCreated).ToList();
            }
            else if (sortColumn == "DateCreated Desc")
            {
                FilteredExampleDTO = FilteredExampleDTO.OrderByDescending(v => v.DateCreated).ToList();
            }
            if (sortColumn == "Price")
            {
                FilteredExampleDTO = FilteredExampleDTO.OrderBy(v => v.Price).ToList();
            }
            else if (sortColumn == "Price Desc")
            {
                FilteredExampleDTO = FilteredExampleDTO.OrderByDescending(v => v.Price).ToList();
            }
            if (sortColumn == "Quantity")
            {
                FilteredExampleDTO = FilteredExampleDTO.OrderBy(v => v.Quantity).ToList();
            }
            else if (sortColumn == "Quantity Desc")
            {
                FilteredExampleDTO = FilteredExampleDTO.OrderByDescending(v => v.Quantity).ToList();
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
                  parameters.Add("Message", $"Name: {example?.Name}");
                  parameters.Add("ButtonColour", "danger");
                  parameters.Add("Icon", "fa fa-trash");
                  var formModal = Modal?.Show<BlazoredModalConfirmDialog>($"Delete Example ({example?.Name})?", parameters);
                  if (formModal != null)
                  {
                      var result = await formModal.Result;
                      if (!result.Cancelled)
                      {
                          await ExampleDataService.DeleteExample(Id);
                          ToastService?.ShowSuccess("Example deleted successfully");
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
            
        private async Task OnValueChangedPageSize(int value)
        {
            pageSize = value;
            pageNumber = 1;
            await LoadData();
        }
        private async Task PageDown(bool goBeginning)
        {
            if (goBeginning || pageNumber <= 0)
            {
                pageNumber = 1;
            }
            else
            {
                pageNumber--;
            }
            await LoadData();
        }
        private async Task PageUp(bool goEnd)
        {
            int maximumPages = (int)Math.Ceiling((decimal)totalRows / pageSize);
            if (goEnd || pageNumber >= maximumPages)
            {
                pageNumber = maximumPages;
            }
            else
            {
                pageNumber++;
            }
            await LoadData();
        }

    }
}