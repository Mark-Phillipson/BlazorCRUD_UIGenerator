
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
using Ardalis.GuardClauses;
using SampleApplication.Shared;
using SampleApplication.Services;
using SampleApplication.DTOs;

namespace SampleApplication.Pages
{
    public partial class GeneralLookupTable : ComponentBase
    {
        [Inject] public IGeneralLookupDataService? GeneralLookupDataService { get; set; }
        [Inject] public NavigationManager? NavigationManager { get; set; }
        [Inject] public ILogger<GeneralLookupTable>? Logger { get; set; }
        //[Inject] public IToastService? ToastService { get; set; }
        [Inject] public ApplicationState? ApplicationState { get; set; }
        [CascadingParameter] public IModalService? Modal { get; set; }
        public string Title { get; set; } = "GeneralLookup Items (GeneralLookups)";
        public string EditTitle { get; set; } = "Edit GeneralLookup Item (GeneralLookups)";
        [Parameter] public int ParentId { get; set; }
        public List<GeneralLookupDTO>? GeneralLookupDTO { get; set; }
        public List<GeneralLookupDTO>? FilteredGeneralLookupDTO { get; set; }
        protected GeneralLookupAddEdit? GeneralLookupAddEdit { get; set; }
        ElementReference SearchInput;
#pragma warning disable 414, 649
        private bool _loadFailed = false;
        private string? searchTerm = null;
#pragma warning restore 414, 649
        public string? SearchTerm { get => searchTerm; set { searchTerm = value; ApplyFilter(); } }
        [Parameter] public string? ServerSearchTerm { get; set; }
        public string ExceptionMessage { get; set; } = String.Empty;
        public List<string>? PropertyInfo { get; set; }
        [CascadingParameter] public ClaimsPrincipal? User { get; set; }
        [Inject] public IJSRuntime? JSRuntime { get; set; }
        public bool ShowEdit { get; set; } = false;
        private bool ShowDeleteConfirm { get; set; }
        private int GeneralLookupId  { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                if (GeneralLookupDataService != null)
                {
                    var result = await GeneralLookupDataService!.GetAllGeneralLookupsAsync();
                    //var result = await GeneralLookupDataService.SearchGeneralLookupsAsync(ServerSearchTerm);
                    if (result != null)
                    {
                        GeneralLookupDTO = result.ToList();
                    }
                }

            }
            catch (Exception e)
            {
                Logger?.LogError("Exception occurred in LoadData Method, Getting Records from the Service", e);
                _loadFailed = true;
                ExceptionMessage = e.Message;
            }
            FilteredGeneralLookupDTO = GeneralLookupDTO;
            Title = $"General Lookup ({FilteredGeneralLookupDTO?.Count})";

        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    if (JSRuntime!= null )
                    {
                        await JSRuntime.InvokeVoidAsync("window.setFocus", "SearchInput");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }
        private void AddNewGeneralLookup()
        {
//            var parameters = new ModalParameters();
//            var formModal = Modal?.Show<GeneralLookupAddEdit>("Add General Lookup", parameters);
//            if (formModal != null)
//            {
//                var result = await formModal.Result;
//                if (!result.Cancelled)
//                {
//                    await LoadData();
//                }
//            }
            GeneralLookupId=0;
            EditTitle = "Add General Lookup";
            ShowEdit = true;
        }

        private void ApplyFilter()
        {
            if (FilteredGeneralLookupDTO == null || GeneralLookupDTO == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(SearchTerm))
            {
                FilteredGeneralLookupDTO = GeneralLookupDTO.OrderBy(v => v.ItemValue).ToList();
                Title = $"All General Lookup ({FilteredGeneralLookupDTO.Count})";
            }
            else
            {
                var temporary = SearchTerm.ToLower().Trim();
                FilteredGeneralLookupDTO = GeneralLookupDTO
                    .Where(v => 
                    (v.ItemValue!= null  && v.ItemValue.ToLower().Contains(temporary))
                     || (v.Category!= null  &&  v.Category.ToLower().Contains(temporary))
                     || (v.DisplayValue!= null  &&  v.DisplayValue.ToLower().Contains(temporary))
                    )
                    .ToList();
                Title = $"Filtered General Lookups ({FilteredGeneralLookupDTO.Count})";
            }
        }
        protected void SortGeneralLookup(string sortColumn)
        {
            Guard.Against.Null(sortColumn, nameof(sortColumn));
                        if (FilteredGeneralLookupDTO == null)
            {
                return;
            }
            if (sortColumn == "ItemValue")
            {
                FilteredGeneralLookupDTO = FilteredGeneralLookupDTO.OrderBy(v => v.ItemValue).ToList();
            }
            else if (sortColumn == "ItemValue Desc")
            {
                FilteredGeneralLookupDTO = FilteredGeneralLookupDTO.OrderByDescending(v => v.ItemValue).ToList();
            }
            if (sortColumn == "Category")
            {
                FilteredGeneralLookupDTO = FilteredGeneralLookupDTO.OrderBy(v => v.Category).ToList();
            }
            else if (sortColumn == "Category Desc")
            {
                FilteredGeneralLookupDTO = FilteredGeneralLookupDTO.OrderByDescending(v => v.Category).ToList();
            }
            if (sortColumn == "SortOrder")
            {
                FilteredGeneralLookupDTO = FilteredGeneralLookupDTO.OrderBy(v => v.SortOrder).ToList();
            }
            else if (sortColumn == "SortOrder Desc")
            {
                FilteredGeneralLookupDTO = FilteredGeneralLookupDTO.OrderByDescending(v => v.SortOrder).ToList();
            }
            if (sortColumn == "DisplayValue")
            {
                FilteredGeneralLookupDTO = FilteredGeneralLookupDTO.OrderBy(v => v.DisplayValue).ToList();
            }
            else if (sortColumn == "DisplayValue Desc")
            {
                FilteredGeneralLookupDTO = FilteredGeneralLookupDTO.OrderByDescending(v => v.DisplayValue).ToList();
            }
        }
        private void DeleteGeneralLookup(int Id)
        {
            //Optionally remove child records here or warn about their existence
            //var ? = await ?DataService.GetAllGeneralLookup(Id);
            //if (? != null)
            //{
            //	ToastService.ShowWarning($"It is not possible to delete a generalLookup that is linked to one or more companies! You would have to delete the companys first. {?.Count()}");
            //	return;
            //}
//            var parameters = new ModalParameters();
//            if (GeneralLookupDataService != null)
//            {
//                var generalLookup = await GeneralLookupDataService.GetGeneralLookupById(Id);
//                parameters.Add("Title", "Please Confirm, Delete General Lookup");
//                parameters.Add("Message", $"ItemValue: {generalLookup?.ItemValue}");
//                parameters.Add("ButtonColour", "danger");
//                parameters.Add("Icon", "fa fa-trash");
//                var formModal = Modal?.Show<BlazoredModalConfirmDialog>($"Delete  General Lookup ({generalLookup?.ItemValue})?", parameters);
//                if (formModal != null)
//                {
//                    var result = await formModal.Result;
//                    if (!result.Cancelled)
//                    {
//                        await GeneralLookupDataService.DeleteGeneralLookup(Id);
//                        ToastService?.ShowSuccess(" General Lookup deleted successfully");
//                        await LoadData();
//                    }
//                }
               GeneralLookupId = Id;
               ShowDeleteConfirm=true;
        }
        private void  EditGeneralLookup(int Id)
        {
//            var parameters = new ModalParameters();
            //parameters.Add("Id", Id);
            //var formModal = Modal?.Show<GeneralLookupAddEdit>("Edit General Lookup", parameters);
            //if (formModal != null)
            //{
                //var result = await formModal.Result;
                //if (!result.Cancelled)
                //{
                //    await LoadData();
                //}
            //}
            GeneralLookupId = Id;
            EditTitle = "Edit General Lookup";
            ShowEdit = true;
        }
        private void ToggleModal()
        {
            ShowEdit = !ShowEdit;
        }
        private void ToggleShowDeleteConfirm()
        {
            ShowDeleteConfirm = !ShowDeleteConfirm;
        }
        public async Task CloseModalAsync(bool close)
        {
            if (close)
            {
                ShowEdit = false;
                await LoadData();
            }
        }
        private async void CloseConfirmDeletion(bool confirmation)
        {
            ShowDeleteConfirm = false;
            if (GeneralLookupDataService == null) return;
            if (confirmation)
            {
                await GeneralLookupDataService.DeleteGeneralLookup(GeneralLookupId);
                if (ApplicationState != null)
                {
                    ApplicationState.Message = $"{GeneralLookupId} General Lookup item has been deleted successfully";
                    ApplicationState.MessageType = "success";
                }
                await LoadData();
                StateHasChanged();
            }
        }

    }
}