using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ZEMS.Data.Repositories;
using ZEMS.Data;
using ZEMS.Application.Models.User;

namespace ZEMS.Application.Commands.User.DeactivateUser
{  
    public class DeactivateUserRequestHandler : IRequestHandler<DeactivateUserRequest, UserModel>
    {
        private readonly UserRepository _repository;
        private readonly ZEMSContext _context;
        private readonly IMapper _mapper;
        public DeactivateUserRequestHandler(UserRepository repository, ZEMSContext context,
            MapperConfiguration mapperConfig) 
        {
            _repository = repository;
            _context = context;
            _mapper = mapperConfig.CreateMapper();           
        }
        public async Task<UserModel> Handle(DeactivateUserRequest request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.Database.BeginTransaction()) 
            {
                var userCore = await _repository.GetItemAsync(request.Id);           
                userCore.SetUpdatedInformation(request.Username);     
                userCore.Identity.DeactivateUser();
                var user = await _repository.SaveAsync(userCore);            
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return _mapper.Map<Data.Models.ZEMSUser, UserModel>(user);
            }         
        }      
    }
}
