﻿using System;
using System.Configuration;

namespace CoreNotes.ExpressionToSQL
{
    /// <summary>
    /// 工厂类
    /// </summary>
    public class DbUtilityFactory
    {
        public static IDbUtility GetDbUtility()
        {
            var getDbType = ConfigurationManager.AppSettings["DbType"];
            if (getDbType == "MySql")
            {
                return MySqlDbUtility.DbUtility.GetInstance();
            }
            else if (getDbType == "MsSql")
            {
                return MsSqlDbUtility.DbUtility.GetInstance();
            }
            else
            {
                return MsSqlDbUtility.DbUtility.GetInstance();
                // throw new NotSupportedException("不支持的数据库");
            }
        }
    }
}