using FusionCore.Papeis.Anotacoes;
using FusionCore.Papeis.Enums;
using Newtonsoft.Json;
using System;

namespace FusionCore.AutorizacaoOperacao
{
    public class EventoOperacaoAutorizada
    {
        private EventoOperacaoAutorizada()
        {
            //Nhibernate
        }

        public EventoOperacaoAutorizada(DateTime dataCriacao, int usuarioLogadoId, int usuarioAutorizouId, Permissao permissao, IPayload payload, string agregado)
        {
            Id = Guid.NewGuid();
            DataCriacao = dataCriacao;
            UsuarioAutorizouId = usuarioAutorizouId;
            UsuarioLogadoId = usuarioLogadoId;
            PermissaoAutorizada = permissao;
            Payload = JsonConvert.SerializeObject(payload);
            Agregado = agregado;

            var typePermissao = typeof(Permissao);
            var info = typePermissao.GetField(permissao.ToString());
            var attrs = (PermissaoDetalhe[])info.GetCustomAttributes(typeof(PermissaoDetalhe), false);

            PermissaoTexto = attrs[0].Descricao;
        }

        public Guid Id { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public int UsuarioLogadoId { get; private set; }
        public int UsuarioAutorizouId { get; private set; }
        public Permissao PermissaoAutorizada { get; private set; }
        public string PermissaoTexto { get; private set; }
        public string Payload { get; private set; }
        public string Agregado { get; private set; } 

    }
}
