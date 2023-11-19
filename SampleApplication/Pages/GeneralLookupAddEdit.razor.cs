
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
        [Parameter] public EventCallback<bool> CloseModal { get; set; } 
        [Parameter] public string? Title { get; set; }
        [Inject] public ILogger<GeneralLookupAddEdit>? Logger { get; set; }
        [CascadingParameter] BlazoredModalInstance? ModalInstance { get; set; }
        [Inject] public IJSRuntime? JSRuntime { get; set; }
        [Parameter] public int? Id { get; set; }
        public GeneralLookupDTO GeneralLookupDTO { get; set; } = new GeneralLookupDTO();//{ };
        [Inject] public IGeneralLookupDataService? GeneralLookupDataService { get; set; }
        //[Inject] public IToastService? ToastService { get; set; }
        [Inject] public ApplicationState? ApplicationState { get; set; }
        [Parameter] public int ParentId { get; set; }
#pragma warning disable 414, 649
        bool TaskRunning = false;
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
        public async Task CloseAsync()
        {
//            if (ModalInstance != null)
//                ModalInstance.CancelAsync();
              await CloseModal.InvokeAsync(true);
        }

        protected async Task HandleValidSubmit()
        {
            if (ApplicationState== null )
            {
                return;
            }
            TaskRunning = true;
            if ((Id == 0 || Id == null) && GeneralLookupDataService != null)
            {
                GeneralLookupDTO? result = await GeneralLookupDataService.AddGeneralLookup(GeneralLookupDTO);
                if (result == null && Logger!= null)
                {
                    Logger.LogError("General Lookup failed to add, please investigate Error Adding New General Lookup");
                    ApplicationState.Message = "General Lookup failed to add, please investigate Error Adding New General Lookup";
                    ApplicationState.MessageType = "danger";
                    return;
                }
                //ToastService?.ShowSuccess("General Lookup added successfully");
                ApplicationState.Message = "General Lookup Added successfully";
                ApplicationState.MessageType = "success";

            }
            else
            {
                if (GeneralLookupDataService != null)
                {
                    await GeneralLookupDataService!.UpdateGeneralLookup(GeneralLookupDTO, "");
                    //ToastService?.ShowSuccess("The General Lookup updated successfully");
                    ApplicationState.Message="The A Menu updated successfully";
                    ApplicationState.MessageType = "success";
                }
            }
            //if (ModalInstance != null)
            //{
            //    await ModalInstance.CloseAsync(ModalResult.Ok(true));
            //}
            await CloseModal.InvokeAsync(true);
            TaskRunning = false;
        }
    }
}