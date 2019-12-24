using System;

namespace CoreNotes.ExpressionToSQL.Attributes
{
    /// <summary>
    /// 递增键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class IdentityAttribute : Attribute
    {
    }
}