
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
    public partial class CategoryAddEdit : ComponentBase
    {
        [Parameter] public EventCallback<bool> CloseModal { get; set; }
        [Parameter]  public  string? Title { get; set; }
        [Inject] public ILogger<CategoryAddEdit>? Logger { get; set; }
        [CascadingParameter] BlazoredModalInstance? ModalInstance { get; set; }
        [Inject] public IJSRuntime? JSRuntime { get; set; }
        [Parameter] public int? Id { get; set; }
        public CategoryDTO CategoryDTO { get; set; } = new CategoryDTO();//{ };
        [Inject] public ICategoryDataService? CategoryDataService { get; set; }
        //[Inject] public IToastService? ToastService { get; set; }
        [Inject] public ApplicationState? ApplicationState { get; set; }
        [Parameter] public int ParentId { get; set; }
#pragma warning disable 414, 649
        bool TaskRunning = false;
#pragma warning restore 414, 649
        protected override async Task OnInitializedAsync()
        {
            if (CategoryDataService == null)
            {
                return;
            }
            if (Id > 0)
            {
                var result = await CategoryDataService.GetCategoryById((int)Id);
                if (result != null)
                {
                    CategoryDTO = result;
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
                        await JSRuntime.InvokeVoidAsync("window.setFocus", "Category");
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
            if ((Id == 0 || Id == null) && CategoryDataService != null)
            {
                CategoryDTO? result = await CategoryDataService.AddCategory(CategoryDTO);
                if (result == null && Logger!=null)
{
                    Logger.LogError("Category failed to add, please investigate Error Adding New Category");
                    ApplicationState.Message = "Category failed to add, please investigate Error Adding New Category";
                    ApplicationState.MessageType = "danger";
                    return;
                }
                //ToastService?.ShowSuccess("Category added successfully", "SUCCESS");
                ApplicationState.Message = "Category Added successfully";
                ApplicationState.MessageType = "success";

            }
            else
            {
                if (CategoryDataService != null)
                {
                    await CategoryDataService!.UpdateCategory(CategoryDTO, "");
                    //ToastService?.ShowSuccess("The Category updated successfully", "SUCCESS");
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