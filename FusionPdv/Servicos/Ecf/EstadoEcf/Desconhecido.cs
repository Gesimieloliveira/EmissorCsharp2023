using System;
using FusionPdv.Ecf;

namespace FusionPdv.Servicos.Ecf.EstadoEcf
{
    public class Desconhecido : VerificaEstado
    {
        public Desconhecido()
        {
        }

        public Desconhecido(EstadoEcfFiscal estadoEcf) : base(estadoEcf)
        {
        }

        protected override void CondicaoDeVerificacao()
        {
            if (Estado == EstadoEcfFiscal.Desconhecido)
                throw new InvalidOperationException("ECF esta no estado desconhecido.");
        }
    }
}
