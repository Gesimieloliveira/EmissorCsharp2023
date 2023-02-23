using System;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.CteEletronico.Autorizador.ConfiguracoesAutorizacoes
{
    public class CteConfiguracaoAutorizacao : EntidadeBase<Guid>
    {
        public Guid Id { get; private set; } = new Guid("6F7502F0-D9DF-49D3-BC6F-E52E1A0B942C");
        public int TempoEsperaConsultaRecibo { get; private set; }


        protected override Guid ChaveUnica => Id;
    }
}