using ContactBookAPI.Lib.Core.Services;
using ContactBookAPI.Lib.Data;
using ContactBookAPI.Lib.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookAPI.Lib.Core.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ContactBookContext _ctx;
        public ContactRepository(ContactBookContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<AppUser>> GetContact(PagingParameter paging)
        {
           var result = await _ctx.AppUsers.OrderBy(user=>user.FirstName)
                         .Include(user=>user.PhoneNumbers)
                         .Include(user=>user.Socials)
                         .Skip((paging.PageNumber - 1) * paging.PageSize)
                         .Take(paging.PageSize)
                         .ToListAsync();
            return result;
        }
    }
}
