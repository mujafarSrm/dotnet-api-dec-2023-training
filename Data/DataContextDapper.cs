using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotNetAPI.Data
{
    class DataContextDapper
    {
        private readonly IConfiguration _config;
        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<T> LoadData<T>(String Sql)
        {
            IDbConnection DbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return DbConnection.Query<T>(Sql);
        }
       

//      public IEnumerable<T> LoadData<T>(string sql)
// {
//     try
//     {
//         IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
//         return dbConnection.Query<T>(sql);
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"Error in LoadData: {ex.Message}");
//         return Enumerable.Empty<T>();
//     }
// }



        public T LoadDataSingle<T>(String Sql)
        {
            IDbConnection DbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            Console.WriteLine($"Executing SQL: {Sql}");
            return DbConnection.QuerySingle<T>(Sql);
        }

        public bool ExcuteSql(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql) > 0;
        }

        public int ExcuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql);
        }

        public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
        {
            SqlCommand commandWithParams = new SqlCommand(sql);

            foreach(SqlParameter parameter in parameters)
            {
                commandWithParams.Parameters.Add(parameter);
            }

            SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            dbConnection.Open();

            commandWithParams.Connection = dbConnection;

            int rowsAffected = commandWithParams.ExecuteNonQuery();

            dbConnection.Close();

            return rowsAffected > 0;
        }
    }
}