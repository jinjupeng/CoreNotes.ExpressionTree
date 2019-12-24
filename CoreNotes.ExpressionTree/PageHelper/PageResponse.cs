using System.Collections.Generic;

namespace CoreNotes.ExpressionTree.PageHelper
{
    /// <summary>
    /// 通用分页返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResponse<T>
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// 返回
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public long PageIndex { get; set; }

        /// <summary>
        /// 每页条数
        /// </summary>
        public long PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPages { get; set; }

        /// <summary>
        /// 返回筛选集合
        /// </summary>
        public Dictionary<string, List<string>> ResultFilter = new Dictionary<string, List<string>>();

    }
}