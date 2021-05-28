using MediatR;

namespace ZEMS.Application.Commands.Employee.DeleteEmployee
{
    public class DeleteEmployeeRequest : IRequest
    {
        public int Id { get; set; }
    }
}
