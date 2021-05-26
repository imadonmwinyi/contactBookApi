using ContactBookAPI.Lib.Core.Services;
using ContactBookAPI.Lib.Data;
using ContactBookAPI.Lib.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookAPI.Lib.Core.Repositories
{
    public class PhoneNumberRepository : IPhoneNumberRepository
    {
        private readonly ContactBookContext _ctx;
        public PhoneNumberRepository(ContactBookContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<bool> CreateAsync(PhoneNumber model)
        {
            _ctx.PhoneNumbers.Add(model);
            var row = await _ctx.SaveChangesAsync();
            if (row < 1)
                return false;
            return true;
        }

        public async Task< IEnumerable<PhoneNumber>> ReadPhoneNumbers(string AppUserId)
        {
            
            var phoneNumbers = await _ctx.PhoneNumbers
                                    .Where(ph => ph.AppUserId == AppUserId)
                                    .ToListAsync();
            return phoneNumbers;
        }

        public async Task<IEnumerable<PhoneNumber>> UpdateAsync(IEnumerable<PhoneNumber> phones)
        {
            foreach (var phone in phones)
            {
                _ctx.Entry(phone).State = EntityState.Modified;
            }
            await _ctx.SaveChangesAsync();
            return phones;
        }
    }
}
