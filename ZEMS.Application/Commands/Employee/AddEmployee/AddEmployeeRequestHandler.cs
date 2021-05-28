using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ZEMS.Data;
using ZEMS.Data.Repositories;
using ZEMS.Application.Models.Employee;

namespace ZEMS.Application.Commands.Employee.AddEmployee
{  
    public class AddEmployeeRequestHandler : IRequestHandler<AddEmployeeRequest, EmployeeModel>
    {
        private readonly EmployeeRepository _repository;
        private readonly ZEMSContext _context;
        private readonly IMapper _mapper;
        public AddEmployeeRequestHandler(EmployeeRepository repository, ZEMSContext context, MapperConfiguration mapperConfig)
        {
            _repository = repository;
            _context = context;
            _mapper = mapperConfig.CreateMapper();
        }

        public async Task<EmployeeModel> Handle(AddEmployeeRequest request, CancellationToken cancellationToken)
        {
            var employeeCore = _mapper.Map<EmployeeModel, Core.Models.Employee>(request.Employee);
            employeeCore.SetCreatedInformation(request.Username);
            var employeeData = await _repository.SaveAsync(employeeCore);
            await _context.SaveChangesAsync();
            return _mapper.Map<Data.Models.Employee, EmployeeModel>(employeeData);
        }  
    }
}
