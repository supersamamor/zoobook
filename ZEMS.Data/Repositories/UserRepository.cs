using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZEMS.Data.Repositories
{
    public class UserRepository
    {
        private readonly ZEMSContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        public UserRepository(ZEMSContext context, MapperConfiguration mapperConfig, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _mapper = mapperConfig.CreateMapper();
            _userManager = userManager;
        }    

        public async Task<Data.Models.ZEMSUser> SaveAsync(Core.Models.ZEMSUser userCore) {
            var user = _mapper.Map<Core.Models.ZEMSUser, Models.ZEMSUser>(userCore);
            if (user.Id == 0)
            {
                await _context.ZEMSUser.AddAsync(user);
            }
            else {
                _context.Entry(user).State = EntityState.Modified;
                _context.Entry(user.Identity).State = EntityState.Modified;
            }   
            return user;
        }

        public void Delete(Core.Models.ZEMSUser userCore)
        {
            var user = _mapper.Map<Core.Models.ZEMSUser, Models.ZEMSUser>(userCore);
            _context.ZEMSUser.Remove(user);
        }

        public async Task<Core.Models.ZEMSUser> GetItemAsync(int id)
        {        
            return _mapper.Map<Models.ZEMSUser, Core.Models.ZEMSUser>
                (await _context.ZEMSUser.Include(l=>l.Identity).Where(l => l.Id == id).AsNoTracking().FirstOrDefaultAsync());          
        }

        public async Task<IList<string>> GetUserRoles(int id)
        {
            var identity = (await _context.ZEMSUser.Include(l => l.Identity).Where(l => l.Id == id).AsNoTracking().FirstOrDefaultAsync())?.Identity;
            return await _userManager.GetRolesAsync(identity);      
        }
    }     
}
