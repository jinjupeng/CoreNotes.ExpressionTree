using CoreNotes.ExpressionToSQL;
using CoreNotes.ExpressionToSQL.Entity;
using NUnit.Framework;

namespace CoreNotes.Test
{
    public class Tests
    {
        [Test]
        public void ReturnsSqlString_ShouldReturnTrue()
        {
            var expectedResult = "SELECT * FROM [Article] WHERE (Article.UserId = '1' )  ";
            var exp = DbUtilityFactory.GetDbUtility().GetSqlExpression<ArticleEntity>();
            
            exp.Where(a => a.UserId == 1);

            var result = $"SELECT * FROM [{ exp.TableName }] WHERE { exp.WhereStr }";

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void ReturnsSqlSortString_ShouldReturnTrue()
        {
            var expectedResult = "SELECT * FROM [Article] ORDER BY Article.ArticleDate DESC ";
            var exp = DbUtilityFactory.GetDbUtility().GetSqlExpression<ArticleEntity>();

            exp.OrderByDescending(a => a.ArticleDate);

            var result = $"SELECT * FROM [{ exp.TableName }] ORDER BY { exp.OrderByStr }";

            Assert.AreEqual(expectedResult, result);
        }
    }
}