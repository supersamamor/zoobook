using MediatR;
using ZEMS.Application.Models.User;

namespace ZEMS.Application.Commands.User.ActivateUser
{
    public class ActivateUserRequest : IRequest<UserModel>
    {
        public int Id { get; set; }
        public string Username { get; set; }        
    }
}
