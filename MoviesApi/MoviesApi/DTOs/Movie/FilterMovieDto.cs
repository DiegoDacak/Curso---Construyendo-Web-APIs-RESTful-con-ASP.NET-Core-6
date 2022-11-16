using MoviesApi.DTOs.Pagination;

namespace MoviesApi.DTOs.Movie
{
    public class FilterMovieDto
    {
        private int Page { get; set; }
        public int RegisterQuantityPerPage { get; set; }

        public PaginationDto Pagination => new PaginationDto() {Page = Page, RegisterQuantityPerPage = RegisterQuantityPerPage};

        public string Title { get; set; }
        public int GenderId { get; set; }
        public bool AtCinema { get; set; }
        public bool FutureRelease { get; set; }
        public string Order { get; set; }
        public bool Ascending { get; set; }
    }
}