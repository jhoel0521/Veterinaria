namespace Veterinaria.DataLayer.QueryBuilder
{
    /// <summary>
    /// Enumeración de operadores SQL válidos
    /// Evita errores de tipeo y hace el código más claro
    /// </summary>
    public enum SqlOperator
    {
        Equal,              // =
        NotEqual,           // !=
        LessThan,           // <
        LessThanOrEqual,    // <=
        GreaterThan,        // >
        GreaterThanOrEqual, // >=
        Like,               // LIKE
        NotLike,            // NOT LIKE
        In,                 // IN
        NotIn,              // NOT IN
        Between,            // BETWEEN
        NotBetween          // NOT BETWEEN
    }

    /// <summary>
    /// Extensión para convertir el enum a string SQL
    /// </summary>
    public static class SqlOperatorExtensions
    {
        public static string ToSqlString(this SqlOperator op)
        {
            return op switch
            {
                SqlOperator.Equal => "=",
                SqlOperator.NotEqual => "!=",
                SqlOperator.LessThan => "<",
                SqlOperator.LessThanOrEqual => "<=",
                SqlOperator.GreaterThan => ">",
                SqlOperator.GreaterThanOrEqual => ">=",
                SqlOperator.Like => "LIKE",
                SqlOperator.NotLike => "NOT LIKE",
                SqlOperator.In => "IN",
                SqlOperator.NotIn => "NOT IN",
                SqlOperator.Between => "BETWEEN",
                SqlOperator.NotBetween => "NOT BETWEEN",
                _ => "="
            };
        }
        public static SqlOperator ToStringSql(string sqlString)
        {
            // si no esta definido tiramo un Error SqlOperator
            switch (sqlString)
            {
                case "=":
                    return SqlOperator.Equal;
                case "!=":
                    return SqlOperator.NotEqual;
                case "<":
                    return SqlOperator.LessThan;
                case "<=":
                    return SqlOperator.LessThanOrEqual;
                case ">":
                    return SqlOperator.GreaterThan;
                case ">=":
                    return SqlOperator.GreaterThanOrEqual;
                case "LIKE":
                    return SqlOperator.Like;
                case "NOT LIKE":
                    return SqlOperator.NotLike;
                case "IN":
                    return SqlOperator.In;
                case "NOT IN":
                    return SqlOperator.NotIn;
                case "BETWEEN":
                    return SqlOperator.Between;
                case "NOT BETWEEN":
                    return SqlOperator.NotBetween;
                default:
                    throw new ArgumentException($"Operador SQL no reconocido: {sqlString}");
            }
        }
    }
}
