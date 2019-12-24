using CoreNotes.ExpressionToSQL;
using CoreNotes.ExpressionToSQL.Entity;
using NUnit.Framework;

namespace CoreNotes.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var userID = 1;
            var exp = DbUtilityFactory.GetDbUtility().GetSqlExpression<ArticleEntity>();
            exp.Where(a => a.UserId == userID);
            exp.OrderByDescending(a => a.ArticleDate);
            // var data = DbUtilityFactory.GetDbUtility().Paged<ArticleEntity>(a => a.ArticleDate, 1, 6);
            Assert.Pass();
        }
    }
}