using System;
using FusionPdv.Ecf;

namespace FusionPdv.Servicos.Ecf.EstadoEcf
{
    public class NaoInicializada : VerificaEstado
    {
        public NaoInicializada(EstadoEcfFiscal estadoEcf) : base(estadoEcf)
        {
        }

        public NaoInicializada()
        {
        }

        protected override void CondicaoDeVerificacao()
        {
            if (Estado == EstadoEcfFiscal.NaoInicializada)
                throw new InvalidOperationException("ECF não foi inicializada.");
        }
    }
}
