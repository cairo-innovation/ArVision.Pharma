using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Data.Common;
using System.Data.SQLite;
using ArVision.Core.Logging;

namespace ArVision.Pharma.DataAccess
{
    public class SqliteDapperDbContext : IDisposable
    {
        private const string CLASS_NAME = nameof(SqliteDapperDbContext);

        private readonly string _connectionString;
        private SQLiteConnection _connection;

        public int? Timeout { get; set; } = null;
        public bool IsBuffered => true;

        public SqliteDapperDbContext(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

            SetupSqliteProviderFactory();
        }

        /// <summary>
        /// SQLite needs to be registered for it to be accessible through the DB provider factory.
        /// (This method registers the database provider factory if it is not already registered.)
        /// </summary>
        private void SetupSqliteProviderFactory()
        {
            var systemData = ConfigurationManager.GetSection("system.data") as DataSet;

            // If the DbProviderFactories table does not exist in system.data, create it.
            if (systemData.Tables.IndexOf("DbProviderFactories") == -1)
            {
                systemData.Tables.Add("DbProviderFactories");
            }
            var dbProviderFactoriesTable = systemData.Tables[systemData.Tables.IndexOf("DbProviderFactories")];

            // If the SQLite entry does not exist in the DbProviderFactories table, add it.
            if (dbProviderFactoriesTable.Rows.Find("System.Data.SQLite") == null)
            {
                dbProviderFactoriesTable.Rows.Add(
                    "SQLite Data Provider",
                    ".NET Framework Data Provider for SQLite",
                    "System.Data.SQLite",
                    "System.Data.SQLite.SQLiteFactory, System.Data.SQLite"
                );
            }
        }

        public string GetDatabaseFilePath()
        {
            string filePath = null;

            var parts = _connectionString.Split(';');
            if (parts.Length > 0)
            {
                var datasource = parts.FirstOrDefault(p => p.EndsWith(".sqlite"));
                if (datasource != null)
                {
                    var index = datasource.IndexOf("=");
                    if (index > 0)
                    {
                        filePath = datasource.Substring(index + 1);
                    }
                }
            }
            return filePath;
        }

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Serializable)
        {
            if (_connection == null)
            {
                _connection = CreateConnection(_connectionString);
                _connection.Open();
            }

            return _connection.BeginTransaction(isolationLevel);
        }

        public int Execute(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        {
            return MakeItSo<int>(conn => conn.Execute(sql, param, transaction, Timeout, commandType));
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, CommandType? commandType = null)
        {
            return MakeItSo<IEnumerable<T>>(conn => conn.Query<T>(sql, param, transaction, buffered: true, commandType: commandType));
        }

        public IEnumerable<TResult> Query<TResult>(string sql, Type[] types, Func<object[], TResult> map, object param = null, IDbTransaction transaction = null, string splitOn = "id", CommandType? commandType = null)
        {
            return MakeItSo<IEnumerable<TResult>>(conn => conn.Query<TResult>(sql, types, map, param, transaction, IsBuffered, splitOn, Timeout, commandType));
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return
                MakeItSo<IEnumerable<TReturn>>(
                    conn =>
                        conn.Query<TFirst, TSecond, TReturn>(sql, map, param, transaction, buffered, splitOn,
                            commandTimeout, commandType));
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return
                MakeItSo<IEnumerable<TReturn>>(
                    conn =>
                        conn.Query<TFirst, TSecond, TThird, TReturn>(sql, map, param, transaction, buffered,
                            splitOn, commandTimeout, commandType));
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return
                MakeItSo<IEnumerable<TReturn>>(
                    conn =>
                        conn.Query<TFirst, TSecond, TThird, TFourth, TReturn>(sql, map, param, transaction,
                            buffered, splitOn, commandTimeout, commandType));
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return
                MakeItSo<IEnumerable<TReturn>>(
                    conn =>
                        conn.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(sql, map, param,
                            transaction, buffered, splitOn, commandTimeout, commandType));
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return
                MakeItSo<IEnumerable<TReturn>>(
                    conn =>
                        conn.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(sql, map, param,
                            transaction, buffered, splitOn, commandTimeout, commandType));
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            return
                MakeItSo<IEnumerable<TReturn>>(
                    conn =>
                        conn.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(sql, map,
                            param, transaction, buffered, splitOn, commandTimeout, commandType));
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        {
            return MakeItSo<Task<IEnumerable<T>>>(conn => conn.QueryAsync<T>(sql, param, transaction, Timeout, commandType));
        }

        public Tuple<IEnumerable<TResult1>, IEnumerable<TResult2>> QueryMultiple<TResult1, TResult2>(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        {
            return MakeItSo(
                conn =>
                {
                    var reader = conn.QueryMultiple(sql, param, transaction, Timeout, commandType);

                    var result1 = reader.Read<TResult1>(IsBuffered);
                    var result2 = reader.Read<TResult2>(IsBuffered);
                    var results = new Tuple<IEnumerable<TResult1>, IEnumerable<TResult2>>(result1, result2);

                    return results;
                });
        }

        public IEnumerable<dynamic> Query(string sql, object param = null, IDbTransaction transaction = null, CommandType? commandType = null)
        {
            return MakeItSo<IEnumerable<dynamic>>(conn => conn.Query(sql, param, transaction, buffered: true, commandType: commandType));
        }

        private TResult MakeItSo<TResult>(Func<IDbConnection, TResult> func)
        {
            if (_connection == null)
            {
                _connection = CreateConnection(_connectionString);
                _connection.Open();
            }

            return func(_connection);
        }

        private SQLiteConnection CreateConnection(string connectionString)
        {
            string methodName = LogManager.GetCurrentMethodName(nameof(CLASS_NAME));
            LogManager.Logger.Trace($@"{methodName}: connectionString: ({connectionString})");

            //var dbProviderFactory = DbProviderFactories.GetFactory("System.Data.SQLite");
            //if (!(dbProviderFactory.CreateConnection() is SQLiteConnection connection))
            SQLiteConnection connection = new SQLiteConnection(connectionString, true);
            if (connection == null)
            {
                throw new InvalidOperationException("Unable to create connection of type System.Data.SQLite.");
            }

            connection.ConnectionString = connectionString;
            return connection;
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
