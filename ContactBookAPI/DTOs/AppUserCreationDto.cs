using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookAPI.DTOs
{
    public class AppUserCreationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public List<PhoneNumberDto> PhoneNumbers { get; set; }
        public List<SocialDto> Socials { get; set; }

    }
}
