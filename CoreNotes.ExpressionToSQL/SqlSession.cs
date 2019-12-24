using System.Collections.Generic;
using CoreNotes.ExpressionToSQL.Common;

namespace CoreNotes.ExpressionToSQL
{
    /// <summary>
    /// 基本思想就是通过EntityHelper，ObjectHelper和ExpressionHelper获取拼接Select语句的全部元素，拼接出完整Select语句
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class SqlSession<T>
    {
        public SqlSession()
        {
            Fields = EntityHelper.GetFields<T>(false);
            Field = EntityHelper.GetFiledString<T>();
            TableName = EntityHelper.GetTableName<T>();
            PrimaryKey = EntityHelper.GetPrimaryKey<T>();
        }
        /// <summary>
        /// 字段，用逗号隔开
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 字段列表
        /// </summary>
        public List<string> Fields { get; set; }
        /// <summary>
        /// 条件表达式
        /// </summary>
        public string WhereStr { get; set; } = "";
        /// <summary>
        /// 是否聚合
        /// </summary>
        public bool IsDistinct { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string OrderByStr { get; set; }
        /// <summary>
        /// 连表字符串
        /// </summary>
        public string JoinStr { get; set; }
        /// <summary>
        /// 完整sql
        /// </summary>
        public string SqlExpression
        {
            get
            {
                var sql = "SELECT $distinct " + Field + " FROM " + TableName + " $join$where$orderby";

                sql = sql.Replace("$distinct", IsDistinct ? "DISTINCT" : "");
                sql = sql.Replace("$join", string.IsNullOrEmpty(JoinStr) ? "" : JoinStr);
                sql = sql.Replace("$where", string.IsNullOrEmpty(WhereStr) ? "" : "WHERE " + WhereStr);
                sql = sql.Replace("$orderby", string.IsNullOrEmpty(OrderByStr) ? "" : "ORDER BY " + OrderByStr);
                return sql;
            }
        }


    }
}