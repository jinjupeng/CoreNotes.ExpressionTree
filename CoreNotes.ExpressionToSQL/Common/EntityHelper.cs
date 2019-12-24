using System;
using System.Collections.Generic;
using System.Linq;
using CoreNotes.ExpressionToSQL.Attributes;

namespace CoreNotes.ExpressionToSQL.Common
{
    public class EntityHelper
    {
        /// <summary>
        /// 通过反射获取DTO的字段，主要提供给在需要从Entity获取数据后，填充给DTO并返回的作用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> GetDtoFields<T>()
        {
            // 通过反射获取PropertyInfo[]对象，然后取出Name属性填入新表
            var fields = typeof(T).GetProperties();
            return fields.Select(i => i.Name).ToList();
        }


        /// <summary>
        /// 获取Entity实体中的字段，主要提供在Select、Update、Insert、Join等字段的获取，以及动态返回泛型TEntity时为反射构建Entity的对象时使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isFullName">true：字段名前面包含表名</param>
        /// <returns></returns>
        public static List<string> GetFields<T>(bool isFullName)
        {
            // 通过反射获取PropertyInfo[]，当isFullName为true时，使用GetTableName<T>方法获取实体表名，并将表名和字段名用"."连接
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
        /// 获取实体代表的数据库表的表名，用于构建sql时提供表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTableName<T>()
        {
            // 根据自定义特性反射出Entity代表的表名
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
        /// 获取实体代表的数据表的主键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetPrimaryKey<T>()
        {
            // 根据自定义特性反射出Entity代表的表的主键
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