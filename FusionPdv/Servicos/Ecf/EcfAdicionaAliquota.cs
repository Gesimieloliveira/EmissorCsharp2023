using System;
using ACBrFramework;
using FusionCore.FusionPdv.Sessao;
using FusionPdv.Ecf;

namespace FusionPdv.Servicos.Ecf
{
    public class EcfAdicionaAliquota
    {

        public void Adiciona(Modelos.Aliquota aliquota)
        {
            try
            {
                SessaoEcf.EcfFiscal.ProgramaAliquota(aliquota.Percentual, aliquota.Tipo);
                SessaoSistema.AliquotasDoEcf = new EcfPegarAliquotas().Aliquotas();
            }
            catch (ACBrException ex)
            {
                throw new ACBrException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }
    }
}
