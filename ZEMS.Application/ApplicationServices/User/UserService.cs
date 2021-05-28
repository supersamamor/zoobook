using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ZEMS.Application.Commands.User.ActivateUser;
using ZEMS.Application.Commands.User.DeactivateUser;
using ZEMS.Application.Commands.User.UpdateUser;
using ZEMS.Application.Exception;
using ZEMS.Application.Models.User;
using ZEMS.Application.Queries.User.GetUserItem;
using ZEMS.Application.Queries.User.GetUserList;
using ZEMS.Data;
using System.Threading.Tasks;
using X.PagedList;

namespace ZEMS.Application.ApplicationServices.User
{
    public class UserService: BaseApplicationService
    {
        public UserService(IMediator mediator, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContext) 
            : base(mediator, userManager, httpContext) {}

        public async Task<IPagedList<UserModel>> GetUserListAsync(string searchKey, string orderBy, string sortBy, int pageIndex,
            int pageSize)
        {
            if (!_claims.IsInRole(Roles.ADMIN))
            {
                throw new UnAuthorizedException();
            }
            var request = new GetUserListRequest
            {
                SearchKey = searchKey,
                OrderBy = orderBy,
                SortBy = sortBy,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return await _mediator.Send(request);   
        }

        public async Task<UserModel> GetUserItemAsync(int id)
        {
            if (!_claims.IsInRole(Roles.ADMIN))
            {
                throw new UnAuthorizedException();
            }
            var request = new GetUserItemRequest
            {
                Id = id
            };
            return await _mediator.Send(request);
        }

        public async Task<UserModel> UpdateUserAsync(UserModel user)
        {
            if (!_claims.IsInRole(Roles.ADMIN))
            {
                throw new UnAuthorizedException();
            }
            var request = new UpdateUserRequest
            {
                User = user,
                Username = _userName
            };
            return await _mediator.Send(request);           
        }

        public async Task<UserModel> ActivateUserAsync(int id)
        {
            if (!_claims.IsInRole(Roles.ADMIN))
            {
                throw new UnAuthorizedException();
            }
            var request = new ActivateUserRequest
            {
                Id = id,
                Username = _userName
            };
            return await _mediator.Send(request);
        }
        public async Task<UserModel> DeactivateUserAsyncAsync(int id)
        {
            if (!_claims.IsInRole(Roles.ADMIN))
            {
                throw new UnAuthorizedException();
            }
            var request = new DeactivateUserRequest
            {
                Id = id,
                Username = _userName
            };
            return await _mediator.Send(request);
        }     
    }
}
