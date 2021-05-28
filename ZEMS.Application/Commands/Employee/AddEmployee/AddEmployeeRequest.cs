using MediatR;
using ZEMS.Application.Models.Employee;

namespace ZEMS.Application.Commands.Employee.AddEmployee
{
    public class AddEmployeeRequest : IRequest<EmployeeModel>
    {
        public EmployeeModel Employee { get; set; }
        public string Username { get; set; }        
    }
}
