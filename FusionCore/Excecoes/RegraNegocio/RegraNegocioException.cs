using System;
using System.Collections.Generic;

namespace FusionCore.Excecoes.RegraNegocio
{
    public class RegraNegocioException : InvalidOperationException
    {
        public RegraNegocioException(string message, IList<string> detalhes) : base(message)
        {
            Detalhes = detalhes;
        }

        public RegraNegocioException(string message) : base(message)
        {
        }

        public bool TemDetalhes => Detalhes?.Count > 0;
        public IList<string> Detalhes { get; } = new List<string>();

        public string JoinMessages(string separator)
        {
            var errors = new List<string> {Message};
            errors.AddRange(Detalhes);

            return TemDetalhes 
                ? string.Join(separator, errors) 
                : Message;
        }
    }
}
