namespace MoviesApp.Dto.Dto
{
    public class MovieReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }
        public int DurationMinutes { get; set; }
        public string GenreName { get; set; }
        public string? DirectorName { get; set; }  
        public List<string> ActorNames { get; set; } = new();
    }
}
