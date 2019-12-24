using System;
using CoreNotes.ExpressionToSQL.Attributes;

namespace CoreNotes.ExpressionToSQL.Entity
{
    [TableName("Article")]
    public class ArticleEntity
    {
        [Primary]
        [Identity]
        //文章ID
        public int ArticleId { get; set; }
        //分类ID
        public int CategoryId { get; set; }
        //文章标题
        public string ArticleTitle { get; set; }
        //文章版权
        public string ArticleCopyright { get; set; }
        //文章创建时间
        public DateTime ArticleDate { get; set; }
        //文章摘要
        public string ArticleAbstract { get; set; }
        //文章内容
        public string ArticleContain { get; set; }
        //文章所属User
        public int UserId { get; set; }
    }
}
