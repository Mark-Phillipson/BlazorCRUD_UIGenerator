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

namespace DynamicCRUD.Shared
{
    public partial class BlazoredModalConfirmDialog : ComponentBase
    {
        [Parameter]
        public string Title { get; set; } = "Please Confirm";
        [Parameter]
        public string? Message { get; set; }

        [Parameter]
        public string ButtonColour { get; set; } = "danger";
        [Parameter]
        public string? Icon { get; set; } = "fas fa-question";
        [CascadingParameter]
        BlazoredModalInstance? ModalInstance { get; set; }

        ElementReference CancelButton;
        void OnCancel()
        {
            ModalInstance?.CancelAsync();
        }

        void OnConfirm()
        {
            ModalInstance?.CloseAsync(ModalResult.Ok(true));
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CancelButton.FocusAsync();
            }
        }
    }
}