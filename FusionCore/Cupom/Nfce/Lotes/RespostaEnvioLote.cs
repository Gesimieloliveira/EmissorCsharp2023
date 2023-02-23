using System.Collections.Generic;

namespace FusionCore.Cupom.Nfce.Lotes
{
    public class RespostaEnvioLote
    {
        public IList<LinhaMensagemSefaz> LinhaMensagemSefazes { get; }

        public RespostaEnvioLote(IList<LinhaMensagemSefaz> linhaMensagemSefazes)
        {
            LinhaMensagemSefazes = linhaMensagemSefazes;
        }
    }
}