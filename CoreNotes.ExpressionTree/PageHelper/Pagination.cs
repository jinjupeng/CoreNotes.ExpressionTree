using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace CoreNotes.ExpressionTree.PageHelper
{
    public class Pagination
    {
        /// <summary>
        /// 对数据集分页
        /// </summary>
        /// <typeparam name="T">数据集对象</typeparam>
        /// <param name="source">数据集</param>
        /// <param name="page">分页信息</param>
        /// <returns></returns>
        public static PageResponse<T> DataPagination<T>(IQueryable<T> source, PageRequest page) where T : class, new()
        {
            var resultData = new PageResponse<T>();
            bool isAsc = page.Direction.ToLower() == "asc" ? true : false;
            string[] order = page.SortBy.Split('&');
            MethodCallExpression resultExp = null;
            foreach (string item in order)
            {
                string orderPart = item;
                orderPart = Regex.Replace(orderPart, @"\s+", "");
                string[] orderArray = orderPart.Split(' ');
                string orderField = orderArray[0];
                bool sort = isAsc;
                if (orderArray.Length == 2)
                {
                    isAsc = orderArray[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(T), "t");
                var property = typeof(T).GetProperty(orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(T), property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            }
            var tempData = source.Provider.CreateQuery<T>(resultExp);
            resultData.PageIndex = page.PageIndex;
            resultData.PageSize = page.PageSize;
            resultData.TotalCount = tempData.Count();

            resultData.TotalPages = resultData.TotalCount / resultData.PageSize;
            if (resultData.TotalCount % resultData.PageSize > 0)
            {
                resultData.TotalPages += 1;
            }

            resultData.Items = tempData.Skip(page.PageSize * (page.PageIndex - 1)).Take(page.PageSize).ToList();
            return resultData;
        }
    }
}