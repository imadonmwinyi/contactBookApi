using ContactBookAPI.Lib.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactBookAPI.Lib.Core.Services
{
    public interface ISocialRepository
    {
        Task<IEnumerable<Social>> ReadSocialsAsync(string AppUserId);
        Task<bool> CreateAsync(Social phoneNumber);
        Task<List<Social>> UpdateAsync(List<Social> social);
    }
}
