using System;
using FusionPdv.Ecf;

namespace FusionPdv.Servicos.Ecf
{
    public class EcfAtivo
    {
        public void EstaAtiva()
        {
            if (!SessaoEcf.EcfFiscal.Ativo)
            {
                throw new InvalidOperationException("Impressora não foi ativa, por favor verificar isso.");
            }
        }
    }
}
