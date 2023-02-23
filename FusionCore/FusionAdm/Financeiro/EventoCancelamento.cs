using System;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionAdm.Financeiro
{
    public sealed class EventoCancelamento
    {
        private readonly int documentoId;

        private EventoCancelamento()
        {
            //nhibernate
        }

        public EventoCancelamento(DocumentoReceber documento, DateTime data, UsuarioDTO usuario) : this()
        {
            Documento = documento;
            DataCancelamento = data;
            Usuario = usuario;
        }

        public DocumentoReceber Documento { get; private set; }
        public UsuarioDTO Usuario { get; private set; }
        public DateTime DataCancelamento { get; private set; }
    }
}