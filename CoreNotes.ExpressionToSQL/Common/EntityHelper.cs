using System;
using System.Collections.Generic;
using System.Linq;
using CoreNotes.ExpressionToSQL.Attributes;

namespace CoreNotes.ExpressionToSQL.Common
{
    public class EntityHelper
    {
        /// <summary>
        /// 获取DTO字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> GetDtoFields<T>()
        {
            var fields = typeof(T).GetProperties();
            return fields.Select(i => i.Name).ToList();
        }


        /// <summary>
        /// 获取Entity实体中的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isFullName">true：字段名前面包含表名</param>
        /// <returns></returns>
        public static List<string> GetFields<T>(bool isFullName)
        {
            var fields = typeof(T).GetProperties();
            var result = new List<string>();
            if (isFullName)
            {
                var tableName = EntityHelper.GetTableName<T>();
                result.AddRange(fields.Select(i => tableName + "." + i.Name));
                return result;
            }
            result.AddRange(fields.Select(i => i.Name));
            return result;
        }

        /// <summary>
        /// 获取实体中的字段，包括表名，使用","连接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetFiledString<T>()
        {
            var list = GetFields<T>(true).ToArray();
            var result = string.Join(",", list);
            return result;
        }


        /// <summary>
        /// 获取实体代表的表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTableName<T>()
        {
            var tableName = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true);
            return ((TableNameAttribute)tableName[0]).TableName;
        }

        public static string GetTableName(Type entityType)
        {
            try
            {
                var tableName = entityType.GetCustomAttributes(typeof(TableNameAttribute), true);
                return ((TableNameAttribute)tableName[0]).TableName;
            }
            catch
            {
                throw new Exception("没有配置TableName特性！");
            }

        }

        /// <summary>
        /// 获取实体主键名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetPrimaryKey<T>()
        {
            var primary = typeof(T).GetCustomAttributes(typeof(PrimaryAttribute), true);
            var pri = typeof(T).GetProperties();
            foreach (var i in pri)
            {
                var pris = i.GetCustomAttributes(typeof(PrimaryAttribute), true);
                if (pris.Any())
                {
                    return i.Name;
                }
            }
            return "";
        }
    }
}