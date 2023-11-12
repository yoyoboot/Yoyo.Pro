using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using DatabaseType = SqlSugar.DbType;

namespace Yoyo.LowCode.Extension
{
    /// <summary>
    /// 扩展数据参数化
    /// </summary>
    public class DbParameters
    {
        /// <summary>
        /// 参数集合
        /// </summary>
        public List<DbParameter> parameterList = new List<DbParameter>();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        public void Add(string name, object value, DatabaseType databaseType, DbType? dbType = null, ParameterDirection? direction = null)
        {
            DbParameter parameter;
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    parameter = new SqlParameter();
                    break;

                case DatabaseType.MySql:
                    parameter = new MySqlParameter();
                    break;

                case DatabaseType.Oracle:
                    parameter = new OracleParameter();
                    break;

                default:
                    parameter = null;
                    break;
            }
            parameter.ParameterName = name;
            parameter.Value = value;
            if (dbType != null)
                parameter.DbType = (DbType)dbType;
            if (direction != null)
                parameter.Direction = (ParameterDirection)direction;
            parameterList.Add(parameter);
        }

        public DbParameter CreateParameter(string name, object value, DbType? dbType = null, ParameterDirection? direction = null)
        {
            DbParameter parameter = new SqlParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            if (dbType != null)
                parameter.DbType = (DbType)dbType;
            if (direction != null)
                parameter.Direction = (ParameterDirection)direction;
            return parameter;
        }
    }
}
