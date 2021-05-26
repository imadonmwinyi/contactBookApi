namespace ContactBookAPI.Lib.Model
{
    public class Social
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}