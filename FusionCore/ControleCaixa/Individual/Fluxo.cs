using System;
using FusionCore.CadastroUsuario;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.ControleCaixa.Individual
{
    public class Fluxo : EntidadeBase<Guid>
    {
        public Fluxo()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.Now;
            EhUmEstorno = false;
        }

        protected override Guid ChaveUnica => Id;
        public Guid Id { get; private set; }
        public long Ordem { get; private set; }
        public CaixaIndividual Caixa { get; set; }
        public IUsuario Usuario { get; set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime DataOperacao { get; set; }
        public decimal ValorOperacao { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public ETipoPagamento TipoPagamento { get; set; }
        public EOrigemFluxoCaixaIndividual OrigemEvento { get; set; }
        public bool EhUmEstorno { get; set; }
        public string Historico { get; set; }
    }
}