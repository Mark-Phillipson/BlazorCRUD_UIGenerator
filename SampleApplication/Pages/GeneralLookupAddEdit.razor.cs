
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
    public partial class GeneralLookupAddEdit : ComponentBase
    {
        [CascadingParameter] BlazoredModalInstance? ModalInstance { get; set; }
        [Inject] public IJSRuntime? JSRuntime { get; set; }
        [Parameter] public int? Id { get; set; }
        public GeneralLookupDTO GeneralLookupDTO { get; set; } = new GeneralLookupDTO();//{ };
        [Inject] public IGeneralLookupDataService? GeneralLookupDataService { get; set; }
        [Inject] public IToastService? ToastService { get; set; }
#pragma warning disable 414, 649
        string TaskRunning = "";
#pragma warning restore 414, 649
        protected override async Task OnInitializedAsync()
        {
            if (GeneralLookupDataService == null)
            {
                return;
            }
            if (Id > 0)
            {
                var result = await GeneralLookupDataService.GetGeneralLookupById((int)Id);
                if (result != null)
                {
                    GeneralLookupDTO = result;
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
                        await JSRuntime.InvokeVoidAsync("window.setFocus", "ItemValue");
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
            if ((Id == 0 || Id == null) && GeneralLookupDataService != null)
            {
                GeneralLookupDTO? result = await GeneralLookupDataService.AddGeneralLookup(GeneralLookupDTO);
                if (result == null)
                {
                    ToastService?.ShowError("General Lookup failed to add, please investigate", "Error Adding New General Lookup");
                    return;
                }
                ToastService?.ShowSuccess("General Lookup added successfully", "SUCCESS");
            }
            else
            {
                if (GeneralLookupDataService != null)
                {
                    await GeneralLookupDataService!.UpdateGeneralLookup(GeneralLookupDTO, "");
                    ToastService?.ShowSuccess("The General Lookup updated successfully", "SUCCESS");
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