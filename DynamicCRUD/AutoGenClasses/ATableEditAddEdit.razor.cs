
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
using ARM_BlazorServer.DTOs;
using ARM_BlazorServer.Services;

namespace ARM_BlazorServer.Pages
{
    public partial class ATableEditAddEdit : ComponentBase
    {
        [Inject] IToastService? ToastService { get; set; }
        [CascadingParameter] BlazoredModalInstance? ModalInstance { get; set; }
        [Parameter] public string? Title { get; set; }
        [Inject] public ILogger<ATableEditAddEdit>? Logger { get; set; }
        [Inject] public IJSRuntime? JSRuntime { get; set; }
        [Parameter] public int? TableEditId { get; set; }
        public ATableEditDTO ATableEditDTO { get; set; } = new ATableEditDTO();//{ };
        [Inject] public IATableEditDataService? ATableEditDataService { get; set; }
        [Inject] public ApplicationState? ApplicationState { get; set; }
        [Parameter] public int ParentId { get; set; }
#pragma warning disable 414, 649
        bool TaskRunning = false;
#pragma warning restore 414, 649
        protected override async Task OnInitializedAsync()
        {
            if (ATableEditDataService == null)
            {
                return;
            }
            if (TableEditId > 0)
            {
                var result = await ATableEditDataService.GetATableEditById((int)TableEditId);
                if (result != null)
                {
                    ATableEditDTO = result;
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
                        await JSRuntime.InvokeVoidAsync("window.setFocus", "Table");
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
            if ((TableEditId == 0 || TableEditId == null) && ATableEditDataService != null)
            {
                ATableEditDTO? result = await ATableEditDataService.AddATableEdit(ATableEditDTO);
                if (result == null && Logger!= null)
                {
                    Logger.LogError("A Table Edit failed to add, please investigate Error Adding New A Table Edit");
                    ToastService?.ShowError("A Table Edit failed to add, please investigate Error Adding New A Table Edit");
                    return;
                }
                ToastService?.ShowSuccess("A Table Edit added successfully", "SUCCESS");
            }
            else
            {
                if (ATableEditDataService != null)
                {
                    await ATableEditDataService!.UpdateATableEdit(ATableEditDTO, "");
                    ToastService?.ShowSuccess("The A Table Edit updated successfully", "SUCCESS");
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