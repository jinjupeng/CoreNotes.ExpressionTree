using System.Linq;
using System.Text;

namespace CoreNotes.ExpressionTree.PageHelper
{
    public class PageRequest
    {
        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// 排序方式（desc、asc）
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// 获取排序sql语句
        /// </summary>
        /// <returns></returns>
        public string GetOrderBySql()
        {
            if (string.IsNullOrWhiteSpace(SortBy))
            {
                return "";
            }
            var resultSql = new StringBuilder(" ORDER BY ");
            string dir = Direction;
            if (string.IsNullOrWhiteSpace(dir))
            {
                dir = "ASC";
            }
            if (SortBy.Contains("&"))
            {
                resultSql.Append("").Append(string.Join(",", SortBy.Split('&').Select(e => $" {e} {dir}").ToArray()));
            }
            else
            {
                resultSql.Append(SortBy).Append("").Append(dir);//默认处理方式
            }
            return resultSql.ToString();
        }
    }
}