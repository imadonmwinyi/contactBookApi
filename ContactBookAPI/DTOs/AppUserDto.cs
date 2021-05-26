using System;
using System.Collections.Generic;

namespace ContactBookAPI.DTOs
{
    public class AppUserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhotoUrl { get; set; }
        public IEnumerable<PhoneNumberDto> PhoneNumbers { get; set; }
        public IEnumerable<SocialDto> Socials { get; set; }
    }
}
