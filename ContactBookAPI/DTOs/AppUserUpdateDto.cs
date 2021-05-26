using System.Collections.Generic;

namespace ContactBookAPI.DTOs
{
    public class AppUserUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Password { get; set; }
        public IEnumerable<PhoneNumberUpdateDto> PhoneNumbers { get; set; }
        public IEnumerable<SocialUpdateDto> Socials { get; set; }
    }
}
