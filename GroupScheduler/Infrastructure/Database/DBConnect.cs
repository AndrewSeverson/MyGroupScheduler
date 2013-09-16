using System;
using System.Collections.Generic;
using System.Configuration;
using Dapper;
using GifinIt.Infrastucture.Data;
using MySql.Data.MySqlClient;

namespace GifinIt.Infrastucture.Database
{
    public abstract class DBConnect : IDisposable
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = ConfigurationManager.AppSettings["DatabaseServerName"];
            database = ConfigurationManager.AppSettings["DatabaseName"];
            uid = ConfigurationManager.AppSettings["DatabaseUsername"];
            password = ConfigurationManager.AppSettings["DatabasePassword"];
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + 
                               ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. This method will close the
        /// connection to the database.
        /// </summary>
        public void Dispose()
        {
            this.connection.Dispose();
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <param name="storedProcedure">The query to execute.</param>
        /// <param name="parameters">An object containing the parameters to pass to the query. Can be null if no parameters need to be passed.</param>
        /// <returns>
        /// The number of rows modified by the stored procedure
        /// </returns>
        protected int ExecuteQuery([NotNull] string storedProcedure, [CanBeNull] dynamic parameters = null)
        {
            try
            {
                this.connection.Open();
                return SqlMapper.Execute(this.connection, storedProcedure, param: parameters);
            }
            finally
            {
                this.connection.Close();
            }
        }

        /// <summary>
        /// Runs a provided query. The SQL results will be automatically mapped to a strongly typed object.
        /// </summary>
        /// <typeparam name="T">The type of object that the SQL results will be mapped to.</typeparam>
        /// <param name="query">The query to run.</param>
        /// <param name="parameters">An object containing the parameters to pass to the query. Can be null if no parameters need to be passed.</param>
        /// <returns>IEnumerable of the type T</returns>
        [NotNull]
        protected IEnumerable<T> RunQuery<T>([NotNull] string query, dynamic parameters = null)
        {
            try
            {
                this.connection.Open();
                return SqlMapper.Query<T>(this.connection, query, param: parameters);
            }
            finally
            {
                this.connection.Close();
            }
        }

        /// <summary>
        /// Runs a query. The SQL results will be returned as a dynamically typed object. This method should only be used when the columns
        /// returned by the stored procedure do not match up exactly with your C# models.
        /// </summary>
        /// <param name="storedProcedure">The query to run.</param>
        /// <param name="parameters">An object containing the parameters to pass to the stored procedure. Can be null if no parameters need to be passed.</param>
        /// <returns>IEnumerable of dynamic objects</returns>
        [NotNull]
        protected IEnumerable<dynamic> RunQuery(string storedProcedure, dynamic parameters = null)
        {
            try
            {
                this.connection.Open();
                return SqlMapper.Query(this.connection, storedProcedure, param: parameters);
            }
            finally
            {
                this.connection.Close();
            }
        }
    }
}