using System;

namespace Veterinaria.DataLayer.Database
{
    public class QueryException : Exception
    {
        public string Sql { get; }

        public QueryException(string message, string sql) : base(message)
        {
            Sql = sql;
        }

        public QueryException(string message, string sql, Exception innerException) : base(message, innerException)
        {
            Sql = sql;
        }
    }
}
