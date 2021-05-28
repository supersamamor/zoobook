using MediatR;
using ZEMS.Application.Models.Employee;

namespace ZEMS.Application.Queries.Employee.GetEmployeeItem
{
    public class GetEmployeeItemRequest : IRequest<EmployeeModel>
    {
        public int Id { get; set; }
    }
}
