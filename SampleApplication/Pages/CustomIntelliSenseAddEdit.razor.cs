
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
    public partial class CustomIntelliSenseAddEdit : ComponentBase
    {
        [Inject] IToastService? ToastService { get; set; }
        [CascadingParameter] BlazoredModalInstance? ModalInstance { get; set; }
        [Parameter] public string? Title { get; set; }
        [Inject] public ILogger<CustomIntelliSenseAddEdit>? Logger { get; set; }
        [Inject] public IJSRuntime? JSRuntime { get; set; }
        [Parameter] public int? Id { get; set; }
        public CustomIntelliSenseDTO CustomIntelliSenseDTO { get; set; } = new CustomIntelliSenseDTO();//{ };
        [Inject] public ICustomIntelliSenseDataService? CustomIntelliSenseDataService { get; set; }
        [Inject] public ApplicationState? ApplicationState { get; set; }
        [Parameter] public int ParentId { get; set; }
#pragma warning disable 414, 649
        bool TaskRunning = false;
#pragma warning restore 414, 649
        protected override async Task OnInitializedAsync()
        {
            if (CustomIntelliSenseDataService == null)
            {
                return;
            }
            if (Id != null && Id != 0)
            {
                var result = await CustomIntelliSenseDataService.GetCustomIntelliSenseById((int)Id);
                if (result != null)
                {
                    CustomIntelliSenseDTO = result;
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
                        await JSRuntime.InvokeVoidAsync("window.setFocus", "DisplayValue");
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
            if ((Id == 0 || Id == null) && CustomIntelliSenseDataService != null)
            {
                CustomIntelliSenseDTO? result = await CustomIntelliSenseDataService.AddCustomIntelliSense(CustomIntelliSenseDTO);
                if (result == null && Logger!= null)
                {
                    Logger.LogError("Custom Intelli Sense failed to add, please investigate Error Adding New Custom Intelli Sense");
                    ToastService?.ShowError("Custom Intelli Sense failed to add, please investigate Error Adding New Custom Intelli Sense");
                    return;
                }
                ToastService?.ShowSuccess("Custom Intelli Sense added successfully");
            }
            else
            {
                if (CustomIntelliSenseDataService != null)
                {
                    await CustomIntelliSenseDataService!.UpdateCustomIntelliSense(CustomIntelliSenseDTO, "");
                    ToastService?.ShowSuccess("The Custom Intelli Sense updated successfully");
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