using System;
using FusionCore.ControleCaixa;
using FusionCore.Repositorio.Base;

namespace FusionCore.NfceSincronizador.ControleCaixa
{
    public class SyncCaixaIndividual : EntidadeBase<Guid>
    {
        private readonly Guid _id;

        private SyncCaixaIndividual()
        {
            //nhibernate
        }

        public SyncCaixaIndividual(CaixaIndividual caixa) : this()
        {
            Caixa = caixa;
        }

        protected override Guid ChaveUnica => _id;
        public CaixaIndividual Caixa { get; private set; }
    }
}