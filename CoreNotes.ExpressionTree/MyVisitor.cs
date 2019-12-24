using System.Linq.Expressions;
using System.Text;

namespace CoreNotes.ExpressionTree
{
    /// <summary>
    /// 实现一个简单的由表达式树生成sql语句的功能
    /// </summary>
    public class MyVisitor : ExpressionVisitor
    {
        private string _tableName;
        private readonly StringBuilder _sbSql = new StringBuilder();

        /*
         * ConstantExpression：常量表达式
         * ParameterExpression：参数表达式
         * UnaryExpression：一元运算符表达式
         * BinaryExpression：二元运算符表达式
         * TypeBinaryExpression：is运算符表达式
         * ConditionalExpression：条件表达式
         * MemberExpression：访问字段或属性表达式
         * MethodCallExpression：调用成员函数表达式
         * Expression<TDelegate>：委托表达式
         */
        protected override Expression VisitBinary(BinaryExpression node)
        {
            base.Visit(node.Left);
            _sbSql.Append(ExpressionTypeToSql(node.NodeType));
            base.Visit(node.Right);
            return node;
        }
        public string GetSqlString()
        {
            return "select * from " + _tableName + " where " + _sbSql.ToString();
        }
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type == typeof(int))
            {
                _sbSql.Append(node.Value);
            }
            else
            {
                _sbSql.Append("'" + node.Value + "'");
            }
            return base.VisitConstant(node);
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_tableName == null)
            {
                _tableName = "[" + node.Type.Name + "]";
            }
            return base.VisitParameter(node);
        }
        protected override Expression VisitMember(MemberExpression node)
        {
            _sbSql.Append("[" + node.Member.Name + "]");
            return base.VisitMember(node);
        }
        /// <summary>
        /// 表达式树类型
        /// </summary>
        /// <param name="expressionType"></param>
        /// <returns></returns>
        public string ExpressionTypeToSql(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.Add:
                    return " + ";
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return " and ";
                case ExpressionType.Equal:
                    return " = ";
                case ExpressionType.NotEqual:
                    return " != ";
                case ExpressionType.GreaterThan:
                    return " > ";
                case ExpressionType.GreaterThanOrEqual:
                    return " >= ";
                case ExpressionType.LessThan:
                    return " < ";
                case ExpressionType.LessThanOrEqual:
                    return " <= ";
                case ExpressionType.Multiply:
                    return " * ";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return " or ";
                default:
                    return "";
            }
        }
    }
}