
namespace ContactBookAPI.Lib.Model
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
