using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Services.HttpContextExtensions
{
    public static class HttpContextExtensions
    {
        public static async Task SetPaginationParameters<T>(this HttpContext httpContext, IQueryable<T> queryable,
            int registerQuantityPerPage, CancellationToken token)
        {
            double quantity = await queryable.CountAsync(token);
            var pageQuantity = Math.Ceiling(quantity / registerQuantityPerPage);
            httpContext.Response.Headers.Add("total-pages", pageQuantity.ToString(CultureInfo.InvariantCulture));
        }
    }
}