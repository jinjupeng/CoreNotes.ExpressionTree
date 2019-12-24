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
        /// 通过反射获取Entity的实例的字段值和表名，跳过自增键并填入Dictionary<string, string>中
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetKeyValue(object obj)
        {
            var data = new Dictionary<string, object>();
            foreach (var i in obj.GetType().GetProperties())
            {
                // 是否包含主键属性
                // if (IsContainsAttribute(i.GetCustomAttributes(true))) continue;

                // 如果包含自增列属性，跳过（或忽略）该列（跳过本次循环，进入下个循环）
                if (IsContainsIdentityAttribute(i.GetCustomAttributes(true))) continue;
                var value = obj.GetType().GetProperty(i.Name).GetValue(obj, null);
                data.Add(i.Name, value);
            }
            return data;
        }

        /// <summary>
        /// 对于一个实体来说，主键属性一定要有，而自增属性可以没有
        /// </summary>
        /// <param name="attrs"></param>
        /// <returns></returns>
        private static bool IsContainsAttribute(IEnumerable<object> attrs)
        {
            return attrs.OfType<PrimaryAttribute>().Any();
        }

        /// <summary>
        /// 是否包含自增键，在插入表时可以跳过自增键的设置，也就是自增列跳过
        /// </summary>
        /// <param name="attrs"></param>
        /// <returns></returns>
        private static bool IsContainsIdentityAttribute(IEnumerable<object> attrs)
        {
            return attrs.OfType<IdentityAttribute>().Any();
        }

        /// <summary>
        /// 通过反射为生成的实例赋值，此处只是列举了常用的数据类型：int，string和DataTime
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