
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
    public partial class LanguageAddEdit : ComponentBase
    {
        [CascadingParameter] BlazoredModalInstance? ModalInstance { get; set; }
        [Inject] public IJSRuntime? JSRuntime { get; set; }
        [Parameter] public int? Id { get; set; }
        public LanguageDTO LanguageDTO { get; set; } = new LanguageDTO();//{ };
        [Inject] public ILanguageDataService? LanguageDataService { get; set; }
        [Inject] public IToastService? ToastService { get; set; }
#pragma warning disable 414, 649
        string TaskRunning = "";
#pragma warning restore 414, 649
        protected override async Task OnInitializedAsync()
        {
            if (LanguageDataService == null)
            {
                return;
            }
            if (Id > 0)
            {
                var result = await LanguageDataService.GetLanguageById((int)Id);
                if (result != null)
                {
                    LanguageDTO = result;
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
                        await JSRuntime.InvokeVoidAsync("window.setFocus", "Language");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }
        public void Close()
        {
            if (ModalInstance != null)
                ModalInstance.CancelAsync();
        }

        protected async Task HandleValidSubmit()
        {
            TaskRunning = "disabled";
            if ((Id == 0 || Id == null) && LanguageDataService != null)
            {
                LanguageDTO? result = await LanguageDataService.AddLanguage(LanguageDTO);
                if (result == null)
                {
                    ToastService?.ShowError("Language failed to add, please investigate", "Error Adding New Language");
                    return;
                }
                ToastService?.ShowSuccess("Language added successfully", "SUCCESS");
            }
            else
            {
                if (LanguageDataService != null)
                {
                    await LanguageDataService!.UpdateLanguage(LanguageDTO, "");
                    ToastService?.ShowSuccess("The Language updated successfully", "SUCCESS");
                }
            }
            if (ModalInstance != null)
            {
                await ModalInstance.CloseAsync(ModalResult.Ok(true));
            }
            TaskRunning = "";
        }
    }
}