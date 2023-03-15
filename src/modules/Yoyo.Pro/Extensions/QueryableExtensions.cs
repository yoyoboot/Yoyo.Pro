// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;

namespace Abp.Linq.Extensions
{
    public static class YoyoProQueryableExtensions
    {
        /// <summary>
        /// Queryable 扩展
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">查询器</param>
        /// <param name="condition">表达式，满足表达式走then,不满足走else</param>
        /// <param name="thenPredicate">then过滤条件</param>
        /// <param name="elsePredicate">else过滤条件</param>
        /// <returns></returns>
        public static IQueryable<T> WhereIfThenElse<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> thenPredicate, Expression<Func<T, bool>> elsePredicate)
        {
            if (condition)
            {
                return query.Where(thenPredicate);
            }

            return query.Where(elsePredicate);
        }
    }
}
