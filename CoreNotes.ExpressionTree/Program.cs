using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CoreNotes.ExpressionTree.Helper;
using CoreNotes.ExpressionTree.Model;
using CoreNotes.ExpressionTree.PageHelper;

namespace CoreNotes.ExpressionTree
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Expression<Func<Person, bool>> expression = p => p.Name == "张三" && p.Name != "李四";
            MyVisitor myVisitor = new MyVisitor();
            myVisitor.Visit(expression);
            Console.WriteLine(myVisitor.GetSqlString());
            */
            Expression<Func<Person, bool>> expression = p => p.Name == "张三" && p.Name != "李四";
            Console.WriteLine(LambdaToSqlHelper.GetSqlFromExpression(expression));
            /*
            var data = new List<ScoreClass>
            {
                new ScoreClass {CourseName = "数学", StudentName = "学生A", Score = 60},
                new ScoreClass {CourseName = "数学", StudentName = "学生B", Score = 65},
                new ScoreClass {CourseName = "数学", StudentName = "学生C", Score = 70},
                new ScoreClass {CourseName = "数学", StudentName = "学生D", Score = 75},
                new ScoreClass {CourseName = "数学", StudentName = "学生E", Score = 80},
                new ScoreClass {CourseName = "数学", StudentName = "学生F", Score = 81},
                new ScoreClass {CourseName = "数学", StudentName = "学生G", Score = 82},
                new ScoreClass {CourseName = "数学", StudentName = "学生H", Score = 83},
                new ScoreClass {CourseName = "数学", StudentName = "学生I", Score = 84}
            };
            // 按照Score降序排序取第一个（5条数据）
            var page = new PageRequest()
            {
                Direction = "desc",
                PageIndex = 1,
                PageSize = 5,
                SortBy = "Score"
            };
            var result = Pagination.DataPagination(data.AsQueryable(), page);

            Console.WriteLine($"分页结果：\n{string.Join("\n", result.Items.Select(e => $"{e.StudentName} {e.CourseName} {e.Score}"))}");
            */
        }
    }
}
