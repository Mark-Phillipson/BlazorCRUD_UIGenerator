
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
using SampleApplication.DTOs;
using SampleApplication.Services;

namespace SampleApplication.Pages
{
    public partial class CustomerAddEdit : ComponentBase
    {
        [Inject] IToastService? ToastService { get; set; }
        [CascadingParameter] BlazoredModalInstance? ModalInstance { get; set; }
        [Parameter] public string? Title { get; set; }
        [Inject] public ILogger<CustomerAddEdit>? Logger { get; set; }
        [Inject] public IJSRuntime? JSRuntime { get; set; }
        [Parameter] public int? id { get; set; }
        public CustomerDTO CustomerDTO { get; set; } = new CustomerDTO();//{ };
        [Inject] public ICustomerDataService? CustomerDataService { get; set; }
        [Inject] public ApplicationState? ApplicationState { get; set; }
        [Parameter] public int ParentId { get; set; }
#pragma warning disable 414, 649
        bool TaskRunning = false;
#pragma warning restore 414, 649
        protected override async Task OnInitializedAsync()
        {
            if (CustomerDataService == null)
            {
                return;
            }
            if (id != null && id != 0)
            {
                var result = await CustomerDataService.GetCustomerById((int)id);
                if (result != null)
                {
                    CustomerDTO = result;
                }
            }
            else
            {
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    if (JSRuntime != null)
                    {
                        await JSRuntime.InvokeVoidAsync("window.setFocus", "CustomerName");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }
        public async Task CloseAsync()
        {
              if (ModalInstance != null)
                  await ModalInstance.CancelAsync();
        }
        protected async Task HandleValidSubmit()
        {
            TaskRunning = true;
            if ((id == 0 || id == null) && CustomerDataService != null)
            {
                CustomerDTO? result = await CustomerDataService.AddCustomer(CustomerDTO);
                if (result == null && Logger!= null)
                {
                    Logger.LogError("Customer failed to add, please investigate Error Adding New Customer");
                    ToastService?.ShowError("Customer failed to add, please investigate Error Adding New Customer");
                    return;
                }
                ToastService?.ShowSuccess("Customer added successfully");
            }
            else
            {
                if (CustomerDataService != null)
                {
                    await CustomerDataService!.UpdateCustomer(CustomerDTO, "");
                    ToastService?.ShowSuccess("The Customer updated successfully");
                }
            }
            if (ModalInstance != null)
            {
                await ModalInstance.CloseAsync(ModalResult.Ok(true));
            }
            TaskRunning = false;
        }
    }
}