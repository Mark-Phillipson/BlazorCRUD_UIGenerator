
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
        [Inject] public ILanguageDataService? LanguageDataService { get; set; }
        [Inject] public NavigationManager? NavigationManager { get; set; }
        [Inject] public ILogger<LanguageTable>? Logger { get; set; }
        
        [Inject] public IToastService? ToastService { get; set; }
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
        private int LanguageId  { get; set; }
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
                    var result = await LanguageDataService!.GetAllLanguagesAsync();
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
              LanguageId=0;
        }


        private void ApplyFilter()
        {
            if (FilteredLanguageDTO == null || LanguageDTO == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(SearchTerm))
            {
                FilteredLanguageDTO = LanguageDTO.OrderBy(v => v.Language).ToList();
                Title = $"All Language ({FilteredLanguageDTO.Count})";
            }
            else
            {
                var temporary = SearchTerm.ToLower().Trim();
                FilteredLanguageDTO = LanguageDTO
                    .Where(v => 
                    (v.Language!= null  && v.Language.ToLower().Contains(temporary))
                     || (v.Colour!= null  &&  v.Colour.ToLower().Contains(temporary))
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
            if (sortColumn == "Language")
            {
                FilteredLanguageDTO = FilteredLanguageDTO.OrderBy(v => v.Language).ToList();
            }
            else if (sortColumn == "Language Desc")
            {
                FilteredLanguageDTO = FilteredLanguageDTO.OrderByDescending(v => v.Language).ToList();
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
                  parameters.Add("Message", $"Language: {language?.Language}");
                  parameters.Add("ButtonColour", "danger");
                  parameters.Add("Icon", "fa fa-trash");
                  var formModal = Modal?.Show<BlazoredModalConfirmDialog>($"Delete Language ({language?.Language})?", parameters);
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
            
    }
}