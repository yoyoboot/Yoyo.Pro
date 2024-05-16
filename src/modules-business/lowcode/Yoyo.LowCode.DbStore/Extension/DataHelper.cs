using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using DatabaseType = SqlSugar.DbType;

namespace Yoyo.LowCode.Extension
{
    /// <summary>
    ///     数据库访问扩展
    /// </summary>
    public static class DataHelper
    {
        private static readonly string dataBaseType = "SqlServer";

        /// <summary>
        ///     转换连接字符串
        /// </summary>
        /// <param name="dbType">数据驱动</param>
        /// <param name="host">主机</param>
        /// <param name="port">端口</param>
        /// <param name="userName">账户</param>
        /// <param name="password">密码</param>
        /// <param name="database">数据库</param>
        /// <returns></returns>
        public static string ToConnectionString(DatabaseType dbType, string host, int? port, string userName,
            string password, string database)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    return string.Format("Data Source={0},{1};Initial Catalog={2};User ID={3};Password={4};MultipleActiveResultSets=True", host, port,
                        database, userName, password);
                //return string.Format("Server={0};Database={1};User ID={2};Password={3};MultipleActiveResultSets=True", host, database, userName, password);
                case DatabaseType.Oracle:
                    return string.Format(
                        "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVER = DEDICATED)(SERVICE_NAME={2})));User Id={3};Password={4}",
                        host, port, database, userName, password);
                case DatabaseType.MySql:
                    return string.Format("server={0};port={1};database={2};user={3};password={4}", host, port, database,
                        userName, password);
                default:
                    throw new Exception("数据库类型目前不支持");
            }
        }

        /// <summary>
        /// 获取表中数据总条数
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetDataCount(DatabaseType dbType, string tableName)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    return string.Format("SELECT COUNT(*) FROM {0} WHERE 1=1", tableName);

                case DatabaseType.Oracle:
                    return string.Format("SELECT sum(num_rows) FROM {0}", tableName);

                case DatabaseType.MySql:
                    return string.Format("SELECT COUNT(*) FROM {0} WHERE 1=1", tableName);

                default:
                    throw new Exception("数据库类型目前不支持");
            }
        }

        /// <summary>
        ///     根据数据库类型获取不同数据库连接对象
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="connectionString"></param>
        public static DbConnection GetConnection(DatabaseType dbType, string connectionString)
        {
            switch (dbType)
            {
                case DatabaseType.SqlServer:
                    return new SqlConnection(connectionString);

                case DatabaseType.MySql:
                    return new MySqlConnection(connectionString);

                case DatabaseType.Oracle:
                    return new OracleConnection(connectionString);

                default:
                    throw new Exception(dbType + "不支持");
            }
        }

        /// <summary>
        ///     根据连接字符串的providerName 转换数据库类型
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static DatabaseType ToDatabaseType(this string providerName)
        {
            switch (providerName.ToLower())
            {
                case DbStoreConsts.SqlServer:
                    return DatabaseType.SqlServer;

                case DbStoreConsts.DataSqlClient:
                    return DatabaseType.SqlServer;

                case DbStoreConsts.SqlClientSqlConnection:
                    return DatabaseType.SqlServer;

                case DbStoreConsts.MicrosoftSqlClientSqlConnection:
                    return DatabaseType.SqlServer;

                case DbStoreConsts.Oracle:
                    return DatabaseType.Oracle;

                case DbStoreConsts.MySql:
                    return DatabaseType.MySql;

                case DbStoreConsts.MySqlClientMySqlConnection:
                    return DatabaseType.MySql;

                default:
                    throw new Exception(providerName + "不支持");
            }
        }

        /// <summary>
        ///     CreateParameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static DbParameter CreateParameter(string name, object value, DbType? dbType = null,
            ParameterDirection? direction = null)
        {
            DbParameter parameter = null;
            switch (dataBaseType)
            {
                case "SqlServer":
                    parameter = new SqlParameter();
                    break;

                case "MySql":
                    parameter = new MySqlParameter();
                    break;

                case "Oracle":
                    parameter = new OracleParameter();
                    break;
            }

            if (parameter != null)
            {
                parameter.ParameterName = name;
                parameter.Value = value;
                if (dbType != null)
                {
                    parameter.DbType = (DbType)dbType;
                }

                if (direction != null)
                {
                    parameter.Direction = (ParameterDirection)direction;
                }
            }

            return parameter;
        }

        /// <summary>
        ///     执行查询 返回IDataReader
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(this DbConnection cnn, string sql, DbParameter[] param = null,
            DbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var cmd = cnn.CreateCommand();
            PrepareCommand(cnn, cmd, transaction, commandTimeout, commandType, sql, param);
            var reader = cmd.ExecuteReader();
            cmd.Parameters.Clear();
            return reader;
        }

        /// <summary>
        ///     执行查询 返回IDataReader
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<IDataReader> ExecuteReaderAsync(this DbConnection cnn, string sql,
            DbParameter[] param = null, DbTransaction transaction = null, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var cmd = cnn.CreateCommand();
            PrepareCommand(cnn, cmd, transaction, commandTimeout, commandType, sql, param);
            var reader = await cmd.ExecuteReaderAsync();
            cmd.Parameters.Clear();
            return reader;
        }

        /// <summary>
        ///     执行查询，返回结果集
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static object ExecuteScalar(this DbConnection cnn, string sql, DbParameter[] param = null,
            DbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var cmd = cnn.CreateCommand();
            PrepareCommand(cnn, cmd, transaction, commandTimeout, commandType, sql, param);
            var obj = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return obj;
        }

        /// <summary>
        ///     执行查询，返回结果集
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<object> ExecuteScalarAsync(this DbConnection cnn, string sql,
            DbParameter[] param = null, DbTransaction transaction = null, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            var cmd = cnn.CreateCommand();
            PrepareCommand(cnn, cmd, transaction, commandTimeout, commandType, sql, param);
            var obj = await cmd.ExecuteScalarAsync();
            cmd.Parameters.Clear();
            return obj;
        }

        /// <summary>
        ///     为即将执行准备一个命令
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmd"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        private static void PrepareCommand(DbConnection conn, DbCommand cmd, DbTransaction transaction,
            int? commandTimeout, CommandType? commandType, string sql, DbParameter[] param = null)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.CommandType = commandType == null ? CommandType.Text : (CommandType)commandType;
            if (commandTimeout != null)
            {
                cmd.CommandTimeout = (int)commandTimeout;
            }

            if (transaction != null)
            {
                cmd.Transaction = transaction;
            }

            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }
        }

        #region MySql

        /// <summary>
        ///     mysql查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="mySqlConnection"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<DataTable> GetMySqlResult(string sql, MySqlConnection mySqlConnection,
            params MySqlParameter[] parameters)
        {
            //存放结果集
            var dataset = new DataSet();
            mySqlConnection.Open();
            var cmd = new MySqlCommand(sql, mySqlConnection);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }

            var adapter = new MySqlDataAdapter(cmd);
            await adapter.FillAsync(dataset);
            cmd.Dispose();
            mySqlConnection.Close();
            var dt = dataset.Tables[0];
            return dt;
        }

        /// <summary>
        ///     mysql查询
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="mySqlConnection"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable GetMySqlResultByNoAsync(string sql, MySqlConnection mySqlConnection,
            params MySqlParameter[] parameters)
        {
            //存放结果集
            var dataset = new DataSet();
            mySqlConnection.Open();
            var cmd = new MySqlCommand(sql, mySqlConnection);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }

            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dataset);
            cmd.Dispose();
            mySqlConnection.Close();
            var dt = dataset.Tables[0];
            return dt;
        }

        /// <summary>
        ///     mysql增删改
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="mySqlConnection"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteMySql(string sql, MySqlConnection mySqlConnection,
            params MySqlParameter[] parameters)
        {
            //存放结果集
            mySqlConnection.Open();
            var cmd = new MySqlCommand(sql, mySqlConnection);
            if (parameters != null && parameters.Length > 0)
            {
                cmd.Parameters.AddRange(parameters);
            }

            var result = await cmd.ExecuteNonQueryAsync();
            cmd.Dispose();
            mySqlConnection.Close();
            return result;
        }

        #endregion MySql
    }
}
