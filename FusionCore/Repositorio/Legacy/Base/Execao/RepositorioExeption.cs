using System;

namespace FusionCore.Repositorio.Legacy.Base.Execao
{
    public class RepositorioExeption : Exception
    {
        public RepositorioExeption(Exception e) : base(e.Message, e)
        {
        }

        public RepositorioExeption(string message) : base(message)
        {
        }

        public RepositorioExeption(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}