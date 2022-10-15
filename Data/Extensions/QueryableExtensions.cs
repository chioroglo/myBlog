using Common.Exceptions;
using Common.Models.Pagination;
using Domain.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;

namespace DAL.Extensions
{
    public static class QueryableExtensions
    {
        public async static Task<PaginatedResult<TEntity>> CreatePaginatedResultAsync<TEntity>(this IQueryable<TEntity> query, PagedRequest pagedRequest, CancellationToken cancellationToken) where TEntity : BaseEntity
        {
            if (pagedRequest.PageIndex <= 0 || pagedRequest.PageSize <= 0)
            {
                throw new ValidationException($"Negative pagination offsets introduced!  Page No.{pagedRequest.PageIndex} PageSize: {pagedRequest.PageSize}");
            }
            query = query.ApplyFilters(pagedRequest);

            var total = await query.CountAsync(cancellationToken);

            query = query.Paginate(pagedRequest);

            query = query.Sort(pagedRequest, cancellationToken);

            var listResult = await query.ToListAsync(cancellationToken);

            return new PaginatedResult<TEntity>()
            {
                Items = listResult,
                PageSize = pagedRequest.PageSize,
                PageIndex = pagedRequest.PageIndex,
                Total = total
            };
        }

        private static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, PagedRequest pagedRequest)
        {
            var predicate = new StringBuilder();

            var requestFilters = pagedRequest.RequestFilters;

            for (int i = 0; i < requestFilters.Filters.Count; i++)
            {
                if (i > 0)
                {
                    predicate.Append($" {requestFilters.LogicalOperator} ");
                }
                predicate.Append(requestFilters.Filters[i].Path + $".{nameof(string.Contains)}(@{i})");
            }

            if (requestFilters.Filters.Any())
            {
                var propertyValues = requestFilters.Filters.Select(filter => filter.Value).ToArray();

                query = query.Where(predicate.ToString(), propertyValues);
            }

            Console.WriteLine(predicate.ToString());
            return query;
        }

        private static IQueryable<T> Paginate<T>(this IQueryable<T> query, PagedRequest pagedRequest)
        {
            var entities = query.Skip((pagedRequest.PageIndex - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
            return entities;
        }

        private static IQueryable<T> Sort<T>(this IQueryable<T> query, PagedRequest pagedRequest, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(pagedRequest.ColumnNameForSorting))
            {
                query = query.OrderBy(pagedRequest.ColumnNameForSorting + " " + pagedRequest.SortingDirection, cancellationToken);
            }

            return query;
        }
    }
}
