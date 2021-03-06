﻿using System.ComponentModel;

namespace CoreNotes.ExpressionToSQL.Enumerations
{
    public enum EnumNodeType
    {
        [Description("二元运算符")]
        BinaryOperator = 1,
        [Description("单目运算符")]
        UnaryOperator = 2,
        [Description("常量表达式")]
        Constant = 3,
        [Description("成员（变量）")]
        MemberAccess = 4,
        [Description("函数")]
        Call = 5,
        [Description("未知")]
        Unknown = -99,
        [Description("不支持")]
        NotSupported = -98
    }

}