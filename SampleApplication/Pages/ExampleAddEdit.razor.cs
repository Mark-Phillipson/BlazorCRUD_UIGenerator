
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
using System.Security.Claims;
using SampleApplication.DTOs;
using SampleApplication.Services;

namespace SampleApplication.Pages
{
    public partial class ExampleAddEdit : ComponentBase
    {
        [Parameter] public EventCallback<bool> CloseModal { get; set; } 
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
              await CloseModal.InvokeAsync(true);
        }
        protected async Task HandleValidSubmit()
        {
            if (ApplicationState== null )
            {
                return;
            }
            TaskRunning = true;
            if ((Id == 0 || Id == null) && ExampleDataService != null)
            {
                ExampleDTO? result = await ExampleDataService.AddExample(ExampleDTO);
                if (result == null && Logger!= null)
                {
                    Logger.LogError("Example failed to add, please investigate Error Adding New Example");
                    ApplicationState.Message = "Example failed to add, please investigate Error Adding New Example";
                    ApplicationState.MessageType = "danger";
                    return;
                }
                ApplicationState.Message = "Example Added successfully";
                ApplicationState.MessageType = "success";
            }
            else
            {
                if (ExampleDataService != null)
                {
                    await ExampleDataService!.UpdateExample(ExampleDTO, "");
                    ApplicationState.Message="The Example updated successfully";
                    ApplicationState.MessageType = "success";
                }
            }
            await CloseModal.InvokeAsync(true);
            TaskRunning = false;
        }
    }
}