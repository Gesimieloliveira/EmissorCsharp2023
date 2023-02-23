using System;
using FusionCore.ControleCaixa;
using FusionCore.Repositorio.Base;

namespace FusionCore.NfceSincronizador.ControleCaixa
{
    public class SyncLancamentoCaixa : EntidadeBase<Guid>
    {
        private readonly Guid _id;

        private SyncLancamentoCaixa()
        {
            //nhibernate
        }

        public SyncLancamentoCaixa(LancamentoAvulsoCaixa lancamento) : this()
        {
            Lancamento = lancamento;
        }

        protected override Guid ChaveUnica => _id;
        public LancamentoAvulsoCaixa Lancamento { get; private set; }
    }
}