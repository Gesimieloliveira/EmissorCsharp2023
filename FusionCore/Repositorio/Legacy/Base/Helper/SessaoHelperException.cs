using System;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Base.Helper
{
    public class SessaoHelperException : ADOException
    {
        public SessaoHelperException(string message) : base(message, null)
        {
        }

        public SessaoHelperException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }
    }
}