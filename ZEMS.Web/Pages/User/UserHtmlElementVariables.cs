using ZEMS.Application;
using ZEMS.Web.Extensions;
using System.Collections.Generic;

namespace ZEMS.Web.Pages.User
{
    public static class UserHtmlElementVariables
    {
        public const string UserForm = "formUserForm";
        public const string UserListingsContainer = "containerUserListings";
        public static PromptContainer UserListingsPromptContainer = new PromptContainer(name: "promptUserListings", effects: "Blink");
        public static PromptContainer UserFormPromptContainer = new PromptContainer(name: "promptUserForm", effects: "Blink");

        public static readonly FormModal UserModal = new FormModal(name: "UserModal", width: 700, isDraggable: true);
        public static readonly PageHandler InitializeListHandler = new PageHandler(name: "InitializeList");
        public static readonly PageHandler ShowEditHandler = new PageHandler(name: "ShowEdit", description: Resource.LabelEditUser, handlerParameters: new List<string> { "id" });
        public static readonly PageHandler ShowViewHandler = new PageHandler(name: "ShowView", description: Resource.LabelDetailsUser, handlerParameters: new List<string> { "id" });
        public static readonly PageHandler UpdateHandler = new PageHandler(name: "Update", withPromptConfirmation: true);
        public static readonly PageHandler ActivateHandler = new PageHandler(name: "Activate", withPromptConfirmation: true);
        public static readonly PageHandler ShowActivateHandler = new PageHandler(name: "ShowActivate", description: Resource.LabelActivateUser, handlerParameters: new List<string> { "id" });
        public static readonly PageHandler DeactivateHandler = new PageHandler(name: "Deactivate", withPromptConfirmation: true);
        public static readonly PageHandler ShowDeactivateHandler = new PageHandler(name: "ShowDeactivate", description: Resource.LabelDeactivateUser, handlerParameters: new List<string> { "id" });
    }
}
