using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ZEMS.Data;
using ZEMS.Data.Repositories;

namespace ZEMS.Application.Commands.Employee.DeleteEmployee
{  
    public class DeleteEmployeeRequestHandler : AsyncRequestHandler<DeleteEmployeeRequest>
    {
        private readonly EmployeeRepository _repository;
        private readonly ZEMSContext _context;     
        public DeleteEmployeeRequestHandler(EmployeeRepository repository, ZEMSContext context) 
        {
            _repository = repository;
            _context = context;          
        }

        protected override async Task Handle(DeleteEmployeeRequest request, CancellationToken cancellationToken)
        {
            var employeeCore = await _repository.GetItemAsync(request.Id);
            _repository.Delete(employeeCore);
            await _context.SaveChangesAsync();
        }

        public async Task HandleAsync(DeleteEmployeeRequest request, CancellationToken cancellationToken)
        {
            await this.Handle(request, cancellationToken);
        }
    }
}
