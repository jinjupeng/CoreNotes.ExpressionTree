﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using CoreNotes.ExpressionToSQL.Common;
using MySql.Data.MySqlClient;

namespace CoreNotes.ExpressionToSQL.MySqlDbUtility
{
    public partial class DbUtility
    {
        /// <summary>
        /// 获取列表，适用于单表查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public List<T> GetList<T>(SqlSession<T> exp) where T : class
        {
            var dataTable = GetDataBySql<T>(exp.SqlExpression);
            var result = new List<T>();
            foreach (DataRow i in dataTable.Rows)
            {
                T obj = Activator.CreateInstance<T>();
                foreach (var k in exp.Fields)
                {
                    SetValue(ref obj, i[k], k);
                }
                result.Add(obj);
            }
            return result;
        }

        /// <summary>
        /// 获取列表，适用于联表查询
        /// </summary>
        /// <typeparam name="Target">DTO类型</typeparam>
        /// <typeparam name="T">exp代表的Entity类型</typeparam>
        /// <param name="exp">SQLSession实例</param>
        /// <returns>DTO列表</returns>
        public List<Target> GetList<Target, T>(SqlSession<T> exp) where T : class where Target : class
        {
            var dataTable = GetDataBySql<T>(exp.SqlExpression);
            var result = new List<Target>();

            foreach (DataRow i in dataTable.Rows)
            {
                var obj = Activator.CreateInstance<Target>();
                foreach (var k in EntityHelper.GetDtoFields<Target>())
                {
                    SetValue(ref obj, i[k], k);
                }
                result.Add(obj);
            }
            return result;
        }

        public List<T> Paged<T>(Expression<Func<T, object>> By, int pageIndex, int pageSize = 1) where T : class
        {
            var result = new List<T>();
            var sql = $@"SELECT * FROM {EntityHelper.GetTableName<T>()} ORDER BY {ExpressionHelper.GetSqlByExpression(By.Body)} LIMIT {pageIndex},{pageSize}";
            conn = new MySqlConnection(connectionString);
            conn.Open();
            var dataTable = GetDataBySql<T>(sql);
            conn.Close();
            foreach (DataRow i in dataTable.Rows)
            {
                var obj = Activator.CreateInstance<T>();
                foreach (var k in EntityHelper.GetFields<T>(false))
                {
                    SetValue(ref obj, i[k], k);
                }
                result.Add(obj);
            }
            return result;
        }
        /// <summary>
        /// 按照主键获取单条记录
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="func">筛选条件</param>
        /// <returns>实体的实例</returns>
        public T GetSingle<T>(Expression<Func<T, bool>> func) where T : class
        {
            var temptable = new DataTable();
            var exp = ExpressionHelper.GetSqlByExpression(func.Body);
            var fields = EntityHelper.GetFiledString<T>();
            var tableName = EntityHelper.GetTableName<T>();
            var sql = "SELECT " + fields + " FROM " + tableName + " WHERE " + exp;
            conn = new MySqlConnection(connectionString);
            da = new MySqlDataAdapter(sql, conn);
            da.Fill(temptable);
            if (temptable.Rows.Count == 0)
            {
                return null;
            }
            T obj = Activator.CreateInstance<T>();
            foreach (var k in EntityHelper.GetFields<T>(false))
            {
                SetValue(ref obj, temptable.Rows[0][k], k);
            }
            return obj;
        }

        /// <summary>
        /// 删除单个记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        public int Delete<T>(Expression<Func<T, bool>> func) where T : class
        {
            var tableName = EntityHelper.GetTableName<T>();
            var exp = ExpressionHelper.GetSqlByExpression(func.Body);
            var sql = "DELETE FROM " + tableName + " WHERE " + exp;
            return RunSingleSql<T>(sql);
        }


        /// <summary>
        /// 添加单个记录
        /// </summary>
        /// <param name="obj"></param>
        public int Add<T>(T obj) where T : class
        {
            var data = ObjectHelper.GetKeyValue(obj);

            var sql = "INSERT INTO {0}({1}) VALUES({2})";
            var tableName = EntityHelper.GetTableName<T>();
            var keys = string.Join(",", data.Keys.ToArray());
            var values = string.Join(",", data.Values.Select(a => a == null ? "''" : "'" + a.ToString() + "'"));

            sql = string.Format(sql, tableName, keys, values);

            return RunSingleSql<T>(sql);
        }

        /// <summary>
        /// 执行除了Select以外的SQL，请不要在循环中使用这个方法，会有性能问题
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int RunSingleSql<T>(string sql) where T : class
        {
            var conn = new MySqlConnection(connectionString);
            var dc = new MySqlCommand(sql, conn);

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            try
            {
                return dc.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 通过sql获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataBySql<T>(string sql) where T : class
        {
            var conn = new MySqlConnection(connectionString);
            da = new MySqlDataAdapter(sql, conn);
            var result = new DataTable();
            da.Fill(result);
            return result;
        }

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">要修改的数据的Entity的实例</param>
        /// <param name="func">要修改数据的条件</param>
        /// <returns></returns>
        public int Update<T>(T obj, Expression<Func<T, bool>> func) where T : class
        {
            if (func == null)
            {
                throw new ArgumentNullException("表达式不能为空！");
            }
            var tableName = EntityHelper.GetTableName<T>();
            var data = ObjectHelper.GetKeyValue(obj);
            var updateStr = data.Aggregate("", (current, i) => current + (i.Key + "='" + i.Value.ToString() + "',"));
            updateStr = updateStr.Substring(0, updateStr.Length - 1);
            var where = ExpressionHelper.GetSqlByExpression(func.Body);
            var sql = $"UPDATE {tableName} SET {updateStr} WHERE {@where}";
            return RunSingleSql<T>(sql);
        }

        /// <summary>
        /// 批量添加记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public int AddList<T>(List<T> objs) where T : class
        {
            if (!objs.Any())
            {
                throw new ArgumentNullException("列表为空！");
            }
            var tableName = EntityHelper.GetTableName(objs[0].GetType());
            var conn = new MySqlConnection(connectionString);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            var transaction = conn.BeginTransaction();
            var dc = conn.CreateCommand();
            dc.Transaction = transaction;
            var count = 0;
            try
            {
                foreach (var k in objs)
                {
                    var data = new Dictionary<string, object>();
                    foreach (var i in k.GetType().GetProperties())
                    {
                        var value = k.GetType().GetProperty(i.Name).GetValue(k, null);
                        data.Add(i.Name, value);
                    }
                    var keys = string.Join(",", data.Keys.ToArray());
                    var values = string.Join(",", data.Values.Select(a => "'" + a.ToString() + "'"));
                    var sql = $"INSERT INTO {tableName}({keys}) VALUES({values})";
                    dc.CommandText = sql;
                    dc.ExecuteNonQuery();
                    count++;
                }
                transaction.Commit();
                return count;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">不传值的话，返回总数</param>
        /// <returns></returns>
        public int Count<T>(Expression<Func<T, bool>> func = null) where T : class
        {
            var tableName = EntityHelper.GetTableName<T>();
            var sql = "";
            if (func == null)
            {
                sql = $"SELECT COUNT(*) FROM {tableName}";
            }
            else
            {
                var where = ExpressionHelper.GetSqlByExpression(func.Body);
                sql = $"SELECT COUNT(*) FROM {tableName} WHERE {@where}";
            }
            conn = new MySqlConnection(connectionString);
            da = new MySqlDataAdapter(sql, conn);
            var dataTable = new DataTable();
            da.Fill(dataTable);
            var result = (int)dataTable.Rows[0][0];
            return result;
        }

        /// <summary>
        /// 获取单个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="field">字段名</param>
        /// <param name="func">条件表达式</param>
        /// <returns></returns>
        public Target Scala<T, Target>(Expression<Func<T, Target>> field, Expression<Func<T, bool>> func)
        {
            var fieldName = ExpressionHelper.GetSqlByExpression(field.Body);
            var exp = ExpressionHelper.GetSqlByExpression(func.Body);
            var sql = $"SELECT {fieldName} FROM {EntityHelper.GetTableName<T>()} WHERE {exp}";
            conn = new MySqlConnection(connectionString);
            da = new MySqlDataAdapter(sql, conn);
            var dataTable = new DataTable();
            da.Fill(dataTable);
            if (dataTable.Rows.Count == 0)
            {
                return default(Target);
            }
            var result = (Target)dataTable.Rows[0][0];
            return result;
        }
    }
}