using ZEMS.Application.Models.Employee;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using MediatR;
using ZEMS.Application.Commands.Employee.UpdateEmployee;
using ZEMS.Application.Commands.Employee.AddEmployee;
using ZEMS.Application.Commands.Employee.DeleteEmployee;
using ZEMS.Application.Queries.Employee.GetEmployeeList;
using ZEMS.Application.Queries.Employee.GetEmployeeItem;
using ZEMS.Application.Models;
using ZEMS.Data;
using ZEMS.Application.Exception;

namespace ZEMS.Application.ApplicationServices.Employee
{
    public class EmployeeService  : BaseApplicationService
    {
        public EmployeeService(IMediator mediator, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContext) 
            : base(mediator, userManager, httpContext)
        {            
        }

        public async Task<CustomPagedList<EmployeeModel>> GetEmployeeListAsync(string searchKey, string orderBy, string sortBy, int pageIndex, int pageSize)
        {          
            if (!_claims.IsInRole(Roles.ADMIN))
            {
                throw new UnAuthorizedException();
            }
            var request = new GetEmployeeListRequest
            {
                SearchKey = searchKey,
                OrderBy = orderBy,
                SortBy = sortBy,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return await _mediator.Send(request);          
        }

        public async Task<EmployeeModel> GetEmployeeItemAsync(int id)
        {
            if (!_claims.IsInRole(Roles.ADMIN))
            {
                throw new UnAuthorizedException();
            }
            var request = new GetEmployeeItemRequest
            {
                Id = id
            };
            return await _mediator.Send(request);
        }

        public async Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employee)
        {
            if (!_claims.IsInRole(Roles.ADMIN))
            {
                throw new UnAuthorizedException();
            }
            var request = new UpdateEmployeeRequest
            {
                Employee = employee,
                Username = _userName
            };
            await _mediator.Send(request);

            var updatedEmployeeRequest = new GetEmployeeItemRequest
            {
                Id = employee.Id
            };
            return await _mediator.Send(updatedEmployeeRequest);
        }

        public async Task<EmployeeModel> SaveEmployeeAsync(EmployeeModel employee)
        {
            if (!_claims.IsInRole(Roles.ADMIN))
            {
                throw new UnAuthorizedException();
            }
            var request = new AddEmployeeRequest
            {
                Employee = employee,
                Username = _userName
            };
            return await _mediator.Send(request);         
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            if (!_claims.IsInRole(Roles.ADMIN))
            {
                throw new UnAuthorizedException();
            }
            var request = new DeleteEmployeeRequest
            {
                Id = id
            };
            await _mediator.Send(request);
        }
    }
}
