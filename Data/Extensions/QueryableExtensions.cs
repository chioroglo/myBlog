using Common.Dto.Paging;
using Common.Dto.Paging.CursorPaging;
using Common.Dto.Paging.OffsetPaging;
using Common.Exceptions;
using Domain.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Reflection;

namespace DAL.Extensions
{
    public static class QueryableExtensions
    {
        public async static Task<CursorPagedResult<TEntity>> CreateCursorPagedResultAsync<TEntity>(this IQueryable<TEntity> query,CursorPagedRequest pagedRequest, CancellationToken cancellationToken)
            where TEntity : BaseEntity
        {
            if (pagedRequest.PageSize <= 0)
            {
                throw new ValidationException($"Negative pagination offsets introduced! PageSize: {pagedRequest.PageSize}");
            }

            query = query.ApplyFilters(pagedRequest.RequestFilters);

            var total = await query.CountAsync(cancellationToken);

            query = query.Sort(nameof(BaseEntity.Id), pagedRequest.GetNewer ? "ASC" : "DESC", cancellationToken);

            query = PaginateCursor(query, pagedRequest.PageSize, pagedRequest.PivotElementId,pagedRequest.GetNewer);

            var listResult = await query.ToListAsync(cancellationToken);

            return new CursorPagedResult<TEntity>()
            {
                Items = listResult,
                PageSize = pagedRequest.PageSize,
                Total = total,
                HeadElementId = listResult.FirstOrDefault()?.Id ?? 0 ,
                TailElementId = listResult.LastOrDefault()?.Id ?? 0
            };

        }
        
        private static IQueryable<T> PaginateCursor<T>(this IQueryable<T> query,int pageSize, int? pivotElementId, bool getNewer) where T : BaseEntity
        {
            if (pivotElementId != null)
            {
                if (getNewer)
                {
                    query = query.Where(x => x.Id > pivotElementId);
                }
                else

                {
                    query = query.Where(x => x.Id < pivotElementId);
                }
            }

            var entities =  query.Take(pageSize);

            return entities;
        }

        private static IQueryable<T> Sort<T>(this IQueryable<T> query, string columnNameForSorting, string sortingDirection, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(columnNameForSorting))
            {
                query = query.OrderBy(columnNameForSorting + " " + sortingDirection, cancellationToken);
            }

            return query;
        }

        private static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, RequestFilters requestFilters)
            {
            var predicate = new StringBuilder();

            for (int i = 0; i < requestFilters.Filters.Count; i++)
            {
                if (i > 0)
                {
                    predicate.Append($" {requestFilters.LogicalOperator} ");
                }


                bool isString = typeof(T).GetProperty(requestFilters.Filters[i].Path)?.PropertyType == typeof(string);


                if (isString)
                {
                    predicate.Append(requestFilters.Filters[i].Path + $".{nameof(string.Contains)}(@{i})");
                }
                else
                {
                    predicate.Append(requestFilters.Filters[i].Path + $" == (@{i})");
                }
                
            }

            if (requestFilters.Filters.Any())
            {
                var propertyValues = requestFilters.Filters.Select(filter => filter.Value).ToArray();

                query = query.Where(predicate.ToString(), propertyValues);
            }
            
            return query;
        }
    }
}
