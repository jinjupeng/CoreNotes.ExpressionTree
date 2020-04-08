# 自定义解析表达式树

## 认识表达式树的主要部分

**Body**：表达式主体，常用的还有：

- ConstantExpression：常量表达式
- ParameterExpression：参数表达式
- UnaryExpression：一元运算符表达式
- BinaryExpression：二元运算符表达式
- TypeBinaryExpression：is运算符表达式
- ConditionalExpression：条件表达式
- MemberExpression：访问字段或属性表达式
- MethodCallExpression：调用成员函数表达式
- Expression<TDelegate>：委托表达式

**NodeType**：节点类型，例子中是 Lambda ，常用还有的+，-，*，/，>，=，<，&&，|| 等都有，不过并不是符号而是对应的英文，详情查看 ExpressionType 枚举
***Parameters**：表达式的参数，a 和 b

```c#
Func<int,int,int> func = (a, b) => a + b;
Expression<Func<int,int,int>> expression = (a, b) => a + b;

Console.WriteLine(expression.Body);
Console.WriteLine(expression.NodeType);
Console.WriteLine(expression.Parameters[0]);
Console.WriteLine(expression.Parameters[1]);
```

输出：

```
(a + b)
Lambda
a
b
```

Body 的类型是 Expression，例子中的是二元表达式，所以要转换成 BinaryExpression 类来查看信息

```c#
BinaryExpression binaryExpression = (BinaryExpression)expression.Body;
Console.WriteLine(binaryExpression.Left);
Console.WriteLine(binaryExpression.Right);
Console.WriteLine(binaryExpression.NodeType);
```

输出是

```
a
b
Add
```



## 链接

[C# 表达式树讲解（一）(重点)](https://www.cnblogs.com/snailblog/p/11521043.html)

[SqlHelper简单实现（通过Expression和反射）1.引言](https://www.cnblogs.com/kakura/p/6108828.html)

[C#表达式树](https://www.cnblogs.com/zhouyg2017/p/11989790.html)

(https://www.cnblogs.com/zhouyg2017/archive/2019/12/06/11989790.html)

[C# Expression 树转化为SQL语句（一）](https://www.cnblogs.com/linxingxunyan/p/6245396.html)

[ExpressionToSQL](https://www.cnblogs.com/zengpeng/p/11319190.html)

[lambda表达式转换sql](https://www.cnblogs.com/maiaimei/p/7147049.html)