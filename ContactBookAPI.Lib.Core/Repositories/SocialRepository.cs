using ContactBookAPI.Lib.Core.Services;
using ContactBookAPI.Lib.Data;
using ContactBookAPI.Lib.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookAPI.Lib.Core.Repositories
{
    public class SocialRepository : ISocialRepository
    {
        private readonly ContactBookContext _ctx;
        public SocialRepository(ContactBookContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<bool> CreateAsync(Social model)
        {
            _ctx.Socials.Add(model);
            var row = await _ctx.SaveChangesAsync();
            if (row < 1)
                return false;
            return true;
        }

        public async Task<IEnumerable<Social>> ReadSocialsAsync(string AppUserId)
        {
            var phoneNumbers = await _ctx.Socials
                                   .Where(sc => sc.AppUserId == AppUserId)
                                   .ToListAsync();
            return phoneNumbers;
        }

        public async Task<List<Social>> UpdateAsync(List<Social> socials)
        {
            //var socials = await _ctx.Socials.Where(sc => sc.AppUserId == social.AppUserId).ToListAsync();
            foreach (var soc in socials)
            {
                _ctx.Entry(soc).State = EntityState.Modified;
            }
            await _ctx.SaveChangesAsync();
            return socials;
        }
    }
}
