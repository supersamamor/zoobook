using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZEMS.Data;
using X.PagedList;
using ZEMS.Application.Models.Employee;
using ZEMS.Application.Models;

namespace ZEMS.Application.Queries.Employee.GetEmployeeList
{
    public class GetEmployeeListRequestHandler : IRequestHandler<GetEmployeeListRequest, CustomPagedList<EmployeeModel>>
    {       
        
        private readonly ZEMSContext _context;
        private readonly IMapper _mapper;
        public GetEmployeeListRequestHandler(ZEMSContext context, MapperConfiguration mapperConfig)
        {
            _context = context;
            _mapper = mapperConfig.CreateMapper();
        }
        public async Task<CustomPagedList<EmployeeModel>> Handle(GetEmployeeListRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Employee.AsNoTracking();
            if (request.SearchKey != null)
            {
                var searchWords = request.SearchKey.ToLower().Split(' ');
                query = query.Where(i => 
									 i.FirstName.ToLower().Contains(searchWords[0])
                                  || i.MiddleName.ToLower().Contains(searchWords[0])
                                  || i.LastName.ToLower().Contains(searchWords[0])
                                  
								  );
                if (searchWords.Length > 1)
                {
                    for (int x = 1; x < searchWords.Length; x++)
                    {
                        var search = searchWords[x];
                        query = query.Where(i => 
									 i.FirstName.ToLower().Contains(search)
                                  || i.MiddleName.ToLower().Contains(search)
                                  || i.LastName.ToLower().Contains(search)
                                  
								  );
                    }
                }            
            }
            switch (request.SortBy)
            {
				case "FirstName":
                    if (request.OrderBy == "Asc")
                    {
                        query = query.OrderBy(l=>l.FirstName);
                    }
                    else
                    {
                        query = query.OrderByDescending(l => l.FirstName);
                    }
                    break;
               case "MiddleName":
                    if (request.OrderBy == "Asc")
                    {
                        query = query.OrderBy(l=>l.MiddleName);
                    }
                    else
                    {
                        query = query.OrderByDescending(l => l.MiddleName);
                    }
                    break;
               case "LastName":
                    if (request.OrderBy == "Asc")
                    {
                        query = query.OrderBy(l=>l.LastName);
                    }
                    else
                    {
                        query = query.OrderByDescending(l => l.LastName);
                    }
                    break;
                           
            }         
            var pagedEmployee = new CustomPagedList<Data.Models.Employee>(query, request.PageIndex, request.PageSize);
            var employeeList = _mapper.Map<IList<Data.Models.Employee>, IList<EmployeeModel>>(await pagedEmployee.Items.ToListAsync());
            return new CustomPagedList<EmployeeModel>(employeeList, pagedEmployee.PagedListMetaData);                     
        }
    }
}
