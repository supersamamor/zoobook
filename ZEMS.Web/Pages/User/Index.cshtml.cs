using Correlate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using X.PagedList;
using ZEMS.Web.Models;
using ZEMS.Web.Extensions;
using ZEMS.Application.ApplicationServices.User;
using ZEMS.Application.Models.User;
using ZEMS.Application.Models.Role;
using System.Collections.Generic;
using ZEMS.Application.ApplicationServices.Role;
using ZEMS.Application;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace ZEMS.Web.Pages.User
{
    public class IndexModel : BasePageModel
    {
        private readonly UserService _service;
        private readonly ILogger _logger;
        private readonly ICorrelationContextAccessor _correlationContext;
        private readonly RoleService _roleService;
        public IndexModel(UserService service, IOptions<ZEMSWebConfig> appSetting, 
            ILogger<IndexModel> logger, ICorrelationContextAccessor correlationContext, RoleService roleService) : base(appSetting.Value.PageSize)
        {
            _service = service;
            _logger = logger;
            _correlationContext = correlationContext;
            _roleService = roleService;
        }

        public IPagedList<UserModel> UserList { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchKey { get; set; }   
        [BindProperty]
        public UserModel ZEMSUser { get; set; }
        public IList<SelectListItem> Roles { get; set; }
        public IActionResult OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnGetInitializeListAsync()
        {
            try
            {
                await GetUserListAsync();
            }
            catch (Exception ex)
            {
                TempData[PromptContainerMessageTempDataName.Error] = _logger.CustomErrorLogger(ex, _correlationContext, nameof(OnGetInitializeListAsync));
            }
            return Partial("_List", this);
        }

        public async Task<IActionResult> OnGetShowEdit(int id)
        {
            await GetRecordAsync(id);           
            return Partial("_Edit", this);
        }

        public async Task<IActionResult> OnGetShowActivate(int id)
        {
            await GetRecordAsync(id);
            return Partial("_Activate", this);
        }

        public async Task<IActionResult> OnGetShowDeactivate(int id)
        {
            await GetRecordAsync(id);
            return Partial("_Deactivate", this);
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            try
            {
                this.ValidateModelState();
                await UpdateUserAsync();
                TempData[PromptContainerMessageTempDataName.Success] = Resource.PromptMessageUpdateSuccess;
            }
            catch (Exception ex)
            {
                TempData[PromptContainerMessageTempDataName.Error] = _logger.CustomErrorLogger(ex, _correlationContext, nameof(OnPostUpdateAsync), ZEMSUser);
            }
            await GetRoleDropdowns();
            return Partial("_Edit", this);
        }

        public async Task<IActionResult> OnPostActivateAsync()
        {

            try
            {
                this.ValidateModelState();
                await ActivateUserAsync();       
                TempData[PromptContainerMessageTempDataName.Success] = Resource.PromptMessageActivateSuccess;
            }
            catch (Exception ex)
            {
                TempData[PromptContainerMessageTempDataName.Error] = _logger.CustomErrorLogger(ex, _correlationContext, nameof(OnPostActivateAsync), User);
            }
            await GetRoleDropdowns();
            return Partial("_Activate", this);
        }

        public async Task<IActionResult> OnPostDeactivateAsync()
        {

            try
            {
                this.ValidateModelState();
                await DeactivateUserAsync();
                TempData[PromptContainerMessageTempDataName.Success] = Resource.PromptMessageDeactivateSuccess;
            }
            catch (Exception ex)
            {
                TempData[PromptContainerMessageTempDataName.Error] = _logger.CustomErrorLogger(ex, _correlationContext, nameof(OnPostActivateAsync), User);
            }
            await GetRoleDropdowns();
            return Partial("_Deactivate", this);
        }

        public async Task<IActionResult> OnGetShowView(int id)
        {
            await GetRecordAsync(id);
            return Partial("_View", this);
        }

        private async Task ActivateUserAsync()
        {
            ZEMSUser = await _service.ActivateUserAsync(ZEMSUser.Id);
        }

        private async Task DeactivateUserAsync()
        {
            ZEMSUser = await _service.DeactivateUserAsyncAsync(ZEMSUser.Id);
        }

        private async Task GetUserListAsync() 
        {
            var paginatedUser = await _service.GetUserListAsync(SearchKey, OrderBy, SortBy, PageNumber, PageSize);
            UserList = paginatedUser;
        }

        private async Task UpdateUserAsync()
        {
            ZEMSUser = await _service.UpdateUserAsync(ZEMSUser);
        }

        private async Task GetRoleDropdowns()
        {          
            var roleList = await _roleService.GetRoleListAsync();
            Roles = roleList.Select(r => new SelectListItem { Text = r.NormalizedName, Value = r.Name }).ToListAsync().Result;
        }
        private async Task GetUserItemAsync(int id)
        {
            ZEMSUser = await _service.GetUserItemAsync(id);
        }

        private async Task GetRecordAsync(int id)
        {
            try
            {
                await GetUserItemAsync(id);
            }
            catch (Exception ex)
            {
                TempData[PromptContainerMessageTempDataName.Error] = _logger.CustomErrorLogger(ex, _correlationContext, nameof(GetRecordAsync), User);
            }
            await GetRoleDropdowns();
        }
    }
}
