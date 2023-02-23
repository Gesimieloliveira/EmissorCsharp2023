using System.Collections.Generic;
using FusionCore.FusionPdv.Ecf;
using FusionPdv.Ecf;

namespace FusionPdv.Servicos.Ecf
{
    public class EcfPegarTiposPagamentos
    {
        public IList<FormaPagamento> TipoPagamento()
        {
            IList<FormaPagamento> formaPagamentos = new List<FormaPagamento>(SessaoEcf.EcfFiscal.FormasPagamentos());
            
            return formaPagamentos;

        }

    }
}
