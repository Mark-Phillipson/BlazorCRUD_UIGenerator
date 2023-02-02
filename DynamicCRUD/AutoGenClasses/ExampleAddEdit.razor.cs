
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
    public partial class ExampleAddEdit : ComponentBase
    {
        [Inject] IToastService? ToastService { get; set; }
        [CascadingParameter] BlazoredModalInstance? ModalInstance { get; set; }
        [Parameter] public string? Title { get; set; }
        [Inject] public ILogger<ExampleAddEdit>? Logger { get; set; }
        [Inject] public IJSRuntime? JSRuntime { get; set; }
        [Parameter] public int? Id { get; set; }
        public ExampleDTO ExampleDTO { get; set; } = new ExampleDTO();//{ };
        [Inject] public IExampleDataService? ExampleDataService { get; set; }
        [Inject] public ApplicationState? ApplicationState { get; set; }
        [Parameter] public int ParentId { get; set; }
#pragma warning disable 414, 649
        bool TaskRunning = false;
#pragma warning restore 414, 649
        protected override async Task OnInitializedAsync()
        {
            if (ExampleDataService == null)
            {
                return;
            }
            if (Id > 0)
            {
                var result = await ExampleDataService.GetExampleById((int)Id);
                if (result != null)
                {
                    ExampleDTO = result;
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
                        await JSRuntime.InvokeVoidAsync("window.setFocus", "Text");
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
            if ((Id == 0 || Id == null) && ExampleDataService != null)
            {
                ExampleDTO? result = await ExampleDataService.AddExample(ExampleDTO);
                if (result == null && Logger!= null)
                {
                    Logger.LogError("Example failed to add, please investigate Error Adding New Example");
                    ToastService?.ShowError("Example failed to add, please investigate Error Adding New Example");
                    return;
                }
                ToastService?.ShowSuccess("Example added successfully", "SUCCESS");
            }
            else
            {
                if (ExampleDataService != null)
                {
                    await ExampleDataService!.UpdateExample(ExampleDTO, "");
                    ToastService?.ShowSuccess("The Example updated successfully", "SUCCESS");
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