using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ZEMS.Data.Repositories;
using ZEMS.Application.Models.User;
namespace ZEMS.Application.Queries.User.GetUserItem
{
    public class GetUserItemRequestHandler : IRequestHandler<GetUserItemRequest, UserModel>
    {
        private readonly UserRepository _repository;
        private readonly IMapper _mapper;    
        public GetUserItemRequestHandler(UserRepository repository, MapperConfiguration mapperConfig)
        {
            _repository = repository;
            _mapper = mapperConfig.CreateMapper();           
        }
        public async Task<UserModel> Handle(GetUserItemRequest request, CancellationToken cancellationToken)
        {
            var userCore = await _repository.GetItemAsync(request.Id);
            var userModel = _mapper.Map<Core.Models.ZEMSUser, UserModel>(userCore);
            userModel.Roles = await _repository.GetUserRoles(request.Id);
            return userModel;
        }
    }
}
