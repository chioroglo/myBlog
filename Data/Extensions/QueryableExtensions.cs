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
        public async static Task<OffsetPagedResult<TEntity>> CreateOffsetPagedResultAsync<TEntity>(this IQueryable<TEntity> query, OffsetPagedRequest pagedRequest, CancellationToken cancellationToken)
            where TEntity : BaseEntity
        {
            if (pagedRequest.PageIndex <= 0 || pagedRequest.PageSize <= 0)
            {
                throw new ValidationException($"Negative pagination offsets introduced!  Page No.{pagedRequest.PageIndex} PageSize: {pagedRequest.PageSize}");
            }
            query = query.ApplyFilters(pagedRequest.RequestFilters);

            var total = await query.CountAsync(cancellationToken);

            query = query.Sort(pagedRequest.ColumnNameForSorting, pagedRequest.SortingDirection, cancellationToken);

            query = query.PaginateOffset(pagedRequest.PageIndex,pagedRequest.PageSize);


            var listResult = await query.ToListAsync(cancellationToken);

            return new OffsetPagedResult<TEntity>()
            {
                Items = listResult,
                PageSize = pagedRequest.PageSize,
                PageIndex = pagedRequest.PageIndex,
                Total = total
            };
        }

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

        private static IQueryable<T> PaginateOffset<T>(this IQueryable<T> query, int pageIndex, int pageSize) where T : BaseEntity
        {
            var entities = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return entities;
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

                var path = requestFilters.Filters[i].Path + (isString ? "" : $".{nameof(ToString)}()");

                predicate.Append(path + $".{nameof(string.Contains)}(@{i})");
            }

            if (requestFilters.Filters.Any())
            {
                var propertyValues = requestFilters.Filters.Select(filter => filter.Value).ToArray();

                query = query.Where(predicate.ToString(), propertyValues);
            }

            Console.WriteLine(predicate.ToString());
            return query;
        }
    }
}
