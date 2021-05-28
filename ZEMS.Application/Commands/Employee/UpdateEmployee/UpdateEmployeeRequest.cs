using MediatR;
using ZEMS.Application.Models.Employee;

namespace ZEMS.Application.Commands.Employee.UpdateEmployee
{
    public class UpdateEmployeeRequest : IRequest
    {
        public EmployeeModel Employee { get; set; }
        public string Username { get; set; }        
    }
}
