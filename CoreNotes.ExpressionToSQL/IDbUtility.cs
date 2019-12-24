using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace CoreNotes.ExpressionToSQL
{
    public interface IDbUtility
    {
        /// <summary>
        /// 获取Sql描述对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        SqlSession<T> GetSqlExpression<T>() where T : class;
        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        List<T> GetList<T>(SqlSession<T> exp) where T : class;
        /// <summary>
        /// 获取多条数据，并填入DTO中
        /// </summary>
        /// <typeparam name="Target"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        List<Target> GetList<Target, T>(SqlSession<T> exp) where T : class where Target : class;
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="By">OrderBy表达式</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<T> Paged<T>(Expression<Func<T, object>> By, int pageIndex, int pageSize = 1) where T : class;
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">linq表达式，代表条件</param>
        /// <returns></returns>
        T GetSingle<T>(Expression<Func<T, bool>> func) where T : class;
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">linq表达式，代表条件</param>
        /// <returns></returns>
        int Delete<T>(Expression<Func<T, bool>> func) where T : class;
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        int Add<T>(T obj) where T : class;
        /// <summary>
        /// 直接执行Sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        int RunSingleSql<T>(string sql) where T : class;
        /// <summary>
        /// 通过Sql获取DataTable，业务层不使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable GetDataBySql<T>(string sql) where T : class;
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <typeparam name="T">数据表实体</typeparam>
        /// <param name="obj">实体对象</param>
        /// <param name="func">linq表达式，代表条件</param>
        /// <returns></returns>
        int Update<T>(T obj, Expression<Func<T, bool>> func) where T : class;
        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs">实体对象列表</param>
        /// <returns></returns>
        int AddList<T>(List<T> objs) where T : class;
        /// <summary>
        /// 计数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">linq表达式，代表条件</param>
        /// <returns></returns>
        int Count<T>(Expression<Func<T, bool>> func = null) where T : class;
        /// <summary>
        /// 获取一个字段数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="field">linq表达式，代表字段</param>
        /// <param name="func">linq表达式，代表条件</param>
        /// <returns></returns>
        Target Scala<T, Target>(Expression<Func<T, Target>> field, Expression<Func<T, bool>> func);
    }
}