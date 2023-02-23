using System;
using ACBrFramework;
using FusionCore.FusionPdv.Sessao;
using FusionPdv.Ecf;

namespace FusionPdv.Servicos.Ecf
{
    public class EcfAddFormaPagamento
    {
        public void Add(string formaPagamento, bool permiteVinculado = false)
        {
            try
            {
                SessaoEcf.EcfFiscal.ProgramaFormaPagamento(formaPagamento, permiteVinculado);
                SessaoSistema.FormasPagamentoEcf = new EcfPegarTiposPagamentos().TipoPagamento();
            }
            catch (ACBrException ex)
            {
                throw new ACBrException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Falha ao adicionar uma forma de pagamento na ecf.", ex);
            }
        }
    }
}
