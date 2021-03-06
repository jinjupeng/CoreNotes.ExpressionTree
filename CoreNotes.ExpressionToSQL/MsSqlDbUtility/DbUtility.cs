﻿using System;
using System.Configuration;
using System.Data.SqlClient;

namespace CoreNotes.ExpressionToSQL.MsSqlDbUtility
{
    public partial class DbUtility : IDbUtility
    {
        private SqlConnection _conn;
        private SqlDataAdapter _da;
        private readonly string _connectionString;

        private DbUtility()
        {
            _connectionString = ConfigurationManager.AppSettings["MsSql"];
        }
        private static DbUtility _dbUtility;

        public static DbUtility GetInstance()
        {
            return _dbUtility ??= new DbUtility();
        }

        /// <summary>
        /// 为通过反射生成的实例赋值
        /// </summary>
        /// <typeparam name="T">实例的类型</typeparam>
        /// <param name="obj">实例</param>
        /// <param name="value">值</param>
        /// <param name="key">成员名称</param>
        private void SetValue<T>(ref T obj, Object value, String key) where T : class
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

        /// <summary>
        /// 获得SQLSession实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public SqlSession<T> GetSqlExpression<T>() where T : class
        {
            var temp = new SqlSession<T>();
            _conn = new SqlConnection(_connectionString);
            return temp;
        }
    }
}