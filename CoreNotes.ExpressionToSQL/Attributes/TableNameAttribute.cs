using System;

namespace CoreNotes.ExpressionToSQL.Attributes
{
    /// <summary>
    /// 表名
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class TableNameAttribute : Attribute
    {
        public string TableName { get; set; }

        public TableNameAttribute(string name)
        {
            TableName = name;
        }
    }
}