using ContactBookAPI.Lib.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactBookAPI.Lib.Core.Services
{
    public interface IPhoneNumberRepository
    {
        Task<IEnumerable<PhoneNumber>> ReadPhoneNumbers(string AppUserId);
        Task<bool> CreateAsync(PhoneNumber phoneNumber);
        Task<IEnumerable<PhoneNumber>> UpdateAsync(IEnumerable<PhoneNumber> number);
    }
}
