using System;
using FusionPdv.Ecf;

namespace FusionPdv.Servicos.Ecf.EstadoEcf
{
    public class Bloqueada : VerificaEstado
    {
        public Bloqueada()
        {
        }

        public Bloqueada(EstadoEcfFiscal estadoEcf) : base(estadoEcf)
        {
        }

        protected override void CondicaoDeVerificacao()
        {
            if (Estado == EstadoEcfFiscal.Bloqueada)
                throw new InvalidOperationException("ECF esta no estado bloqueada.");
        }
    }
}
