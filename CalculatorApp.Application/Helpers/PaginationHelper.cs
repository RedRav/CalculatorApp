using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorApp.Application.Helpers
{
    public static class PaginationHelper
    {
        public static IQueryable<T> OffsetPagination<T>(IQueryable<T> query, int offset, int pageSize)
        {
            return query.Skip(offset).Take(pageSize);
        }

        public static IQueryable<T> CursorPagination<T>(
            IQueryable<T> query,
            Expression<Func<T, DateTime>> timestampSelector,
            DateTime? afterTimestamp,
            int pageSize)
        {
            if (afterTimestamp.HasValue)
            {
                query = query.Where(Expression.Lambda<Func<T, bool>>(
                    Expression.LessThan(timestampSelector.Body, Expression.Constant(afterTimestamp.Value)),
                    timestampSelector.Parameters[0]));
            }

            return query
                .OrderByDescending(timestampSelector)
                .Take(pageSize);
        }
    }
}
