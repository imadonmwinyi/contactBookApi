using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace ContactBookAPI.Lib.Model
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoUrl { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public IEnumerable<Social> Socials { get; set; }
        public IEnumerable<PhoneNumber> PhoneNumbers { get; set; }
        

    }

}
