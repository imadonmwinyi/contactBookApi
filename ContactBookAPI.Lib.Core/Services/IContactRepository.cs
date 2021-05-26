using ContactBookAPI.Lib.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactBookAPI.Lib.Core.Services
{
    public interface IContactRepository
    {
        Task<List<AppUser>> GetContact(PagingParameter paging);
    }
}
