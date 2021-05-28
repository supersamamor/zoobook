using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZEMS.Web.Filters;
namespace ZEMS.Web.Controller
{
    [ApiController]
    [Produces("application/json")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [AllowAnonymous]
    [AuthorizeApiKey]
    public class BaseController : ControllerBase
    {      
    }
}
