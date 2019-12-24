using System;
using System.Collections.Generic;
using System.Linq;
using CoreNotes.ExpressionToSQL.Attributes;

namespace CoreNotes.ExpressionToSQL.Common
{
    internal static class ObjectHelper
    {
        /// <summary>
        /// 获取Entity实例的字段名和值（用于更新和插入数据）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetKeyValue(object obj)
        {
            var data = new Dictionary<string, object>();
            foreach (var i in obj.GetType().GetProperties())
            {
                if (IsContainsAttribute(i.GetCustomAttributes(true))) continue;
                var value = obj.GetType().GetProperty(i.Name).GetValue(obj, null);
                data.Add(i.Name, value);
            }
            return data;
        }

        /// <summary>
        /// 实体是否包含属性
        /// </summary>
        /// <param name="attrs"></param>
        /// <returns></returns>
        private static bool IsContainsAttribute(IEnumerable<object> attrs)
        {
            var enumerable = attrs.ToList();
            return enumerable.OfType<TableNameAttribute>().Any() && enumerable.OfType<PrimaryAttribute>().Any();
        }

        /// <summary>
        /// 是否包含自增键，在插入表时可以跳过自增键的设置
        /// </summary>
        /// <param name="attrs"></param>
        /// <returns></returns>
        private static bool IsContainsIdentityAttribute(IEnumerable<object> attrs)
        {
            return attrs.OfType<IdentityAttribute>().Any();
        }

        /// <summary>
        /// 为通过反射生成的实例赋值
        /// </summary>
        /// <typeparam name="T">实例的类型</typeparam>
        /// <param name="obj">实例</param>
        /// <param name="value">值</param>
        /// <param name="key">成员名称</param>
        public static void SetValue<T>(ref T obj, Object value, String key) where T : class
        {
            var property = obj.GetType().GetProperty(key);
            var type = property.PropertyType.Name;
            if (value is DBNull)
            {
                property.SetValue(obj, null, null);
                return;
            }
            switch (type)
            {
                case "Int32":
                    property.SetValue(obj, int.Parse(value.ToString()), null);
                    break;
                case "String":
                    property.SetValue(obj, value.ToString(), null);
                    break;
                case "DateTime":
                    property.SetValue(obj, (DateTime)value, null);
                    break;
                default:
                    property.SetValue(obj, value, null);
                    break;
            }
        }
    }
}