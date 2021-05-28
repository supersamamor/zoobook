using MediatR;
using ZEMS.Application.Models;
using ZEMS.Application.Models.Employee;
using X.PagedList;

namespace ZEMS.Application.Queries.Employee.GetEmployeeList
{
    public class GetEmployeeListRequest : IRequest<CustomPagedList<EmployeeModel>>
    {
        public string SearchKey { get; set; }
        public string OrderBy { get; set; }
        public string SortBy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
