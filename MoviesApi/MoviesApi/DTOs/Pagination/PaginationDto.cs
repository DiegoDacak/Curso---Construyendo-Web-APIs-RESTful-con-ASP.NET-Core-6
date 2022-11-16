namespace MoviesApi.DTOs.Pagination
{
    public class PaginationDto
    {
        public int Page { get; set; } = 1;
        private int _registerQuantityPerPage = 10;
        private const int MaxRegisterQuantityPerPage = 50;

        public int RegisterQuantityPerPage
        {
            get => _registerQuantityPerPage;
            set => _registerQuantityPerPage = (value > MaxRegisterQuantityPerPage) ? MaxRegisterQuantityPerPage : value;
        }
    }
}