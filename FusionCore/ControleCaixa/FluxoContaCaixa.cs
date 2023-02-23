using System;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace FusionCore.ControleCaixa
{
    public class FluxoContaCaixa : EntidadeBase<Guid>
    {
        private FluxoContaCaixa()
        {
            //nhibernate
            //Fluxo utiliza identity value

            Id = Guid.NewGuid();
            DataCriacao = DateTime.Now;
        }

        public FluxoContaCaixa(
            DateTime dataOperacao,
            UsuarioDTO usuario,
            TipoOperacao tipoOperacao,
            EOrigemFluxoContaCaixa origemEvento,
            decimal totalOperacao,
            decimal novoSaldo,
            string historico
        ) : this()
        {
            DataOperacao = dataOperacao;
            Usuario = usuario;
            TipoOperacao = tipoOperacao;
            Historico = historico;
            TotalOperacao = totalOperacao;
            SaldoAtual = novoSaldo;
            OrigemEvento = origemEvento;
        }

        protected override Guid ChaveUnica => Id;
        public Guid Id { get; private set; }
        public long Fluxo { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime DataOperacao { get; private set; }
        public UsuarioDTO Usuario { get; private set; }
        public TipoOperacao TipoOperacao { get; private set; }
        public decimal TotalOperacao { get; private set; }
        public decimal SaldoAtual { get; private set; }
        public string Historico { get; private set; }
        public Guid OrigemReferencia { get; private set; }
        public EOrigemFluxoContaCaixa OrigemEvento { get; private set; }

        public void AnexarComoReferencia(Guid referenciaId)
        {
            if (OrigemReferencia != Guid.Empty)
            {
                throw new InvalidOperationException("Fluxo de conta caixa já possui um caixa anexado");
            }

            OrigemReferencia = referenciaId;
        }
    }
}