using System;
using FusionCore.Repositorio.Legacy.Base.Helper;

namespace FusionCore.Repositorio.Exceptions
{
    public class DatabaseNotFoundException : SessaoHelperException
    {
        public DatabaseNotFoundException(Exception innerException) : base(innerException)
        {
        }
    }
}