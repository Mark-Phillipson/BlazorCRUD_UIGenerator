
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
    public partial class LanguageTable : ComponentBase
    {
        [Inject] public required ILanguageDataService LanguageDataService { get; set; }
        [Inject] public required NavigationManager NavigationManager { get; set; }
        [Inject] public ILogger<LanguageTable>? Logger { get; set; }

        [Inject] public required IToastService ToastService { get; set; }
        [CascadingParameter] public IModalService? Modal { get; set; }
        public string Title { get; set; } = "Language Items (Languages)";
        public string EditTitle { get; set; } = "Edit Language Item (Languages)";
        [Parameter] public int ParentId { get; set; }
        public List<LanguageDTO>? LanguageDTO { get; set; }
        public List<LanguageDTO>? FilteredLanguageDTO { get; set; }
        protected LanguageAddEdit? LanguageAddEdit { get; set; }
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
        private int pageNumber = 1;
        private int pageSize = 5;
        private int totalRows = 0;

        private int LanguageId { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                if (LanguageDataService != null)
                {
                    totalRows = await LanguageDataService.GetTotalCount();
                    var result = await LanguageDataService!.GetAllLanguagesAsync(pageNumber, pageSize);
                    //var result = await LanguageDataService.SearchLanguagesAsync(ServerSearchTerm);
                    if (result != null)
                    {
                        LanguageDTO = result.ToList();
                        FilteredLanguageDTO = result.ToList();
                        StateHasChanged();
                    }
                }
            }
            catch (Exception e)
            {
                Logger?.LogError("e, Exception occurred in LoadData Method, Getting Records from the Service");
                _loadFailed = true;
                ExceptionMessage = e.Message;
            }
            FilteredLanguageDTO = LanguageDTO;
            Title = $"Language ({FilteredLanguageDTO?.Count})";

        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    if (JSRuntime != null)
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
        private async Task AddNewLanguage()
        {
            var parameters = new ModalParameters();
            var formModal = Modal?.Show<LanguageAddEdit>("Add Language", parameters);
            if (formModal != null)
            {
                var result = await formModal.Result;
                if (!result.Cancelled)
                {
                    await LoadData();
                }
            }
            LanguageId = 0;
        }


        private void ApplyFilter()
        {
            if (FilteredLanguageDTO == null || LanguageDTO == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(SearchTerm))
            {
                FilteredLanguageDTO = LanguageDTO.OrderBy(v => v.LanguageName).ToList();
                Title = $"All Language ({FilteredLanguageDTO.Count})";
            }
            else
            {
                var temporary = SearchTerm.ToLower().Trim();
                FilteredLanguageDTO = LanguageDTO
                    .Where(v =>
                    (v.LanguageName != null && v.LanguageName.ToLower().Contains(temporary))
                     || (v.Colour != null && v.Colour.ToLower().Contains(temporary))
                    )
                    .ToList();
                Title = $"Filtered Languages ({FilteredLanguageDTO.Count})";
            }
        }
        protected void SortLanguage(string sortColumn)
        {
            Guard.Against.Null(sortColumn, nameof(sortColumn));
            if (FilteredLanguageDTO == null)
            {
                return;
            }
            if (sortColumn == "LanguageName")
            {
                FilteredLanguageDTO = FilteredLanguageDTO.OrderBy(v => v.LanguageName).ToList();
            }
            else if (sortColumn == "LanguageName Desc")
            {
                FilteredLanguageDTO = FilteredLanguageDTO.OrderByDescending(v => v.LanguageName).ToList();
            }
        }
        private async Task DeleteLanguage(int Id)
        {
            //TODO Optionally remove child records here or warn about their existence
            var parameters = new ModalParameters();
            if (LanguageDataService != null)
            {
                var language = await LanguageDataService.GetLanguageById(Id);
                parameters.Add("Title", "Please Confirm, Delete Language");
                parameters.Add("Message", $"LanguageName: {language?.LanguageName}");
                parameters.Add("ButtonColour", "danger");
                parameters.Add("Icon", "fa fa-trash");
                var formModal = Modal?.Show<BlazoredModalConfirmDialog>($"Delete Language ({language?.LanguageName})?", parameters);
                if (formModal != null)
                {
                    var result = await formModal.Result;
                    if (!result.Cancelled)
                    {
                        await LanguageDataService.DeleteLanguage(Id);
                        ToastService?.ShowSuccess("Language deleted successfully");
                        await LoadData();
                    }
                }
            }
            LanguageId = Id;
        }

        private async void EditLanguage(int Id)
        {
            var parameters = new ModalParameters();
            parameters.Add("Id", Id);
            var formModal = Modal?.Show<LanguageAddEdit>("Edit Language", parameters);
            if (formModal != null)
            {
                var result = await formModal.Result;
                if (!result.Cancelled)
                {
                    await LoadData();
                }
            }
            LanguageId = Id;
        }

        private async Task OnValueChangedPageSize(int value)
        {
            pageSize = value;
            pageNumber = 1;
            await LoadData();
        }
        private async Task PageDown(bool goBeginning)
        {
            if (goBeginning || pageNumber <= 0)
            {
                pageNumber = 1;
            }
            else
            {
                pageNumber--;
            }
            await LoadData();
        }
        private async Task PageUp(bool goEnd)
        {
            int maximumPages = (int)(totalRows / pageSize + 0.9);
            if (goEnd || pageNumber >= maximumPages)
            {
                pageNumber = maximumPages;
            }
            else
            {
                pageNumber++;
            }
            await LoadData();
        }

    }
}