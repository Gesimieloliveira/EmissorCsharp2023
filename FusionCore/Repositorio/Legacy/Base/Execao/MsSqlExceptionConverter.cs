using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using NHibernate.Exceptions;

// Usado pelo Nhibernate via reflection
// ReSharper disable UnusedMember.Global

namespace FusionCore.Repositorio.Legacy.Base.Execao
{
    // ReSharper disable once UnusedMember.Global
    public class MsSqlExceptionConverter : ISQLExceptionConverter
    {
        public Exception Convert(AdoExceptionContextInfo exInfo)
        {
            var dbException = ADOExceptionHelper.ExtractDbException(exInfo.SqlException);

            if (dbException is SqlException error)
            {
                return TraduzException(error, exInfo.Sql);
            }

            return SQLStateConverter.HandledNonSpecificException(
                exInfo.SqlException, 
                exInfo.Message, 
                exInfo.Sql
            );
        }

        private static Exception TraduzException(SqlException error, string sql)
        {
            const string fkMessage = "The DELETE statement conflicted with the REFERENCE constraint";
            var isMatch = Regex.IsMatch(error.Message, $"({fkMessage}).+", RegexOptions.IgnoreCase);

            if (isMatch && error.Number == 547)
            {
                return CriaExeptionDeDependencia(error, sql);
            }

            return SQLStateConverter.HandledNonSpecificException(error, error.Message, sql);
        }

        private static Exception CriaExeptionDeDependencia(SqlException error, string sql)
        {
            var constrainedError = new ConstraintViolationException(error.Message, error.InnerException, sql, null);

            return new InvalidOperationException(
                "Opa... o registro já foi utilizado. Não posso exclui-lo agora e nem futuramente!",
                constrainedError
            );
        }
    }
}