using MediatR;
using ZEMS.Application.Models.User;

namespace ZEMS.Application.Commands.User.DeactivateUser
{
    public class DeactivateUserRequest : IRequest<UserModel>
    {
        public int Id { get; set; }
        public string Username { get; set; }        
    }
}
