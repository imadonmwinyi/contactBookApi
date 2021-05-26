using Microsoft.AspNetCore.Http;

namespace ContactBookAPI.DTOs
{
    public class PhotoUpdateDto
    {
        public IFormFile Photo { get; set; }
    }
}
