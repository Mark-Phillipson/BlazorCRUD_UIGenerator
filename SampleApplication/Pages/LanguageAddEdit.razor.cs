
using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Extensions.Logging;
using SampleApplication.DTOs;
using SampleApplication.Services;

using System.Threading.Tasks;

namespace SampleApplication.Pages;

public partial class LanguageAddEdit : ComponentBase
{
        [Inject] public required IToastService ToastService { get; set; }
        [CascadingParameter] BlazoredModalInstance? ModalInstance { get; set; }
        [Parameter] public string? Title { get; set; }
        [Inject] public required ILogger<LanguageAddEdit> Logger { get; set; }
        [Inject] public required IJSRuntime JSRuntime { get; set; }
        [Parameter] public int? Id { get; set; }
        [Inject] public required NavigationManager NavigationManager { get; set; }
        public LanguageDTO LanguageDTO { get; set; } = new LanguageDTO();//{ };
        [Inject] public required ILanguageDataService LanguageDataService { get; set; }
        [Parameter] public int ParentId { get; set; }

    ElementReference? FirstInput;
    private bool isSubmitting;
    protected override async Task OnInitializedAsync()
    {
            if (Id != null && Id != 0)
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
                await Task.Delay(100);
                if (FirstInput != null)
                {
                    await FirstInput.Value.FocusAsync();
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
        isSubmitting = true;
        if ((Id == 0 || Id == null))
        {
            LanguageDTO? result = await LanguageDataService.AddLanguage(LanguageDTO);
            if (result == null)
            {
                Logger.LogError("Error adding Language");
            }
            else
            {
                LanguageDTO = result;
                NavigationManager.NavigateTo("/Languages");
            }
        }
        else
        {
            var updateResult = await LanguageDataService.UpdateLanguage(LanguageDTO,"TBC");
            if (updateResult==null)
            {
                Logger.LogError("Error updating Language");
            }
            else
            {
                NavigationManager.NavigateTo("/Languages");
            }
        }
        isSubmitting = false;
    }
}