
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
    public partial class CustomerTable : ComponentBase
    {
        [Inject] public ICustomerDataService? CustomerDataService { get; set; }
        [Inject] public NavigationManager? NavigationManager { get; set; }
        [Inject] public ILogger<CustomerTable>? Logger { get; set; }
        
        [Inject] public IToastService? ToastService { get; set; }
        [CascadingParameter] public IModalService? Modal { get; set; }
        public string Title { get; set; } = "Customer Items (Customers)";
        public string EditTitle { get; set; } = "Edit Customer Item (Customers)";
        [Parameter] public int ParentId { get; set; }
        public List<CustomerDTO>? CustomerDTO { get; set; }
        public List<CustomerDTO>? FilteredCustomerDTO { get; set; }
        protected CustomerAddEdit? CustomerAddEdit { get; set; }
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
        private int CustomerId  { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                if (CustomerDataService != null)
                {
                    var result = await CustomerDataService!.GetAllCustomersAsync();
                    //var result = await CustomerDataService.SearchCustomersAsync(ServerSearchTerm);
                    if (result != null)
                    {
                        CustomerDTO = result.ToList();
                        FilteredCustomerDTO = result.ToList();
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
            FilteredCustomerDTO = CustomerDTO;
            Title = $"Customer ({FilteredCustomerDTO?.Count})";

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
        private async Task AddNewCustomer()
        {
              var parameters = new ModalParameters();
              var formModal = Modal?.Show<CustomerAddEdit>("Add Customer", parameters);
              if (formModal != null)
              {
                  var result = await formModal.Result;
                  if (!result.Cancelled)
                  {
                      await LoadData();
                  }
              }
              CustomerId=0;
        }


        private void ApplyFilter()
        {
            if (FilteredCustomerDTO == null || CustomerDTO == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(SearchTerm))
            {
                FilteredCustomerDTO = CustomerDTO.OrderBy(v => v.CustomerName).ToList();
                Title = $"All Customer ({FilteredCustomerDTO.Count})";
            }
            else
            {
                var temporary = SearchTerm.ToLower().Trim();
                FilteredCustomerDTO = CustomerDTO
                    .Where(v => 
                    (v.CustomerName!= null  && v.CustomerName.ToLower().Contains(temporary))
                     || (v.ContactName!= null  &&  v.ContactName.ToLower().Contains(temporary))
                     || (v.Address!= null  &&  v.Address.ToLower().Contains(temporary))
                     || (v.City!= null  &&  v.City.ToLower().Contains(temporary))
                     || (v.PostalCode!= null  &&  v.PostalCode.ToLower().Contains(temporary))
                     || (v.Email!= null  &&  v.Email.ToLower().Contains(temporary))
                     || (v.Website!= null  &&  v.Website.ToLower().Contains(temporary))
                     || (v.Country!= null  &&  v.Country.ToLower().Contains(temporary))
                    )
                    .ToList();
                Title = $"Filtered Customers ({FilteredCustomerDTO.Count})";
            }
        }
        protected void SortCustomer(string sortColumn)
        {
            Guard.Against.Null(sortColumn, nameof(sortColumn));
                        if (FilteredCustomerDTO == null)
            {
                return;
            }
            if (sortColumn == "CustomerName")
            {
                FilteredCustomerDTO = FilteredCustomerDTO.OrderBy(v => v.CustomerName).ToList();
            }
            else if (sortColumn == "CustomerName Desc")
            {
                FilteredCustomerDTO = FilteredCustomerDTO.OrderByDescending(v => v.CustomerName).ToList();
            }
            if (sortColumn == "Created")
            {
                FilteredCustomerDTO = FilteredCustomerDTO.OrderBy(v => v.Created).ToList();
            }
            else if (sortColumn == "Created Desc")
            {
                FilteredCustomerDTO = FilteredCustomerDTO.OrderByDescending(v => v.Created).ToList();
            }
        }
        private async Task DeleteCustomer(int id)
        {
            //TODO Optionally remove child records here or warn about their existence
              var parameters = new ModalParameters();
              if (CustomerDataService != null)
              {
                  var customer = await CustomerDataService.GetCustomerById(id);
                  parameters.Add("Title", "Please Confirm, Delete Customer");
                  parameters.Add("Message", $"CustomerName: {customer?.CustomerName}");
                  parameters.Add("ButtonColour", "danger");
                  parameters.Add("Icon", "fa fa-trash");
                  var formModal = Modal?.Show<BlazoredModalConfirmDialog>($"Delete Customer ({customer?.CustomerName})?", parameters);
                  if (formModal != null)
                  {
                      var result = await formModal.Result;
                      if (!result.Cancelled)
                      {
                          await CustomerDataService.DeleteCustomer(id);
                          ToastService?.ShowSuccess("Customer deleted successfully");
                          await LoadData();
                      }
                  }
             }
             CustomerId = id;
        }
                  
        private async void EditCustomer(int id)
        {
            var parameters = new ModalParameters();
            parameters.Add("id", id);
            var formModal = Modal?.Show<CustomerAddEdit>("Edit Customer", parameters);
            if (formModal != null)
            {
                var result = await formModal.Result;
                if (!result.Cancelled)
                {
                    await LoadData();
                }
            }
            CustomerId = id;
        }
            
    }
}