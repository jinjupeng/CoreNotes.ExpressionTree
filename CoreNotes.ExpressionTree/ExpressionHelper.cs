using System;
using System.Linq.Expressions;

namespace CoreNotes.ExpressionTree
{
    public class ExpressionHelper: ExpressionVisitor
    {
        // 将Expression<Func<T, bool>>解析成sql语句
        public static string ExpressionToSql<T>(Expression<Func<T, bool>> func)
        {
            MyVisitor myVisitor = new MyVisitor();
            Console.WriteLine();
            return myVisitor.GetSqlString();
        }
    }
}