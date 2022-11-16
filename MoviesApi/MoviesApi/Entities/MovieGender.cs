namespace MoviesApi.Entities
{
    public class MovieGender
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
    }
}