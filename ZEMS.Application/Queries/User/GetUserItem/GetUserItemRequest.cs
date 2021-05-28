using MediatR;
using ZEMS.Application.Models.User;

namespace ZEMS.Application.Queries.User.GetUserItem
{
    public class GetUserItemRequest : IRequest<UserModel>
    {
        public int Id { get; set; }
    }
}
