using System.Linq;
using MoviesApi.DTOs.Pagination;

namespace MoviesApi.Services.Pagination
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDto paginationDto)
        {
            return queryable
                .Skip((paginationDto.Page - 1) * paginationDto.RegisterQuantityPerPage)
                .Take(paginationDto.RegisterQuantityPerPage);
        }
    }
}