using MediatR;
using ZEMS.Application.Models.User;

namespace ZEMS.Application.Commands.User.UpdateUser
{
    public class UpdateUserRequest : IRequest<UserModel>
    {
        public UserModel User { get; set; }
        public string Username { get; set; }        
    }
}
