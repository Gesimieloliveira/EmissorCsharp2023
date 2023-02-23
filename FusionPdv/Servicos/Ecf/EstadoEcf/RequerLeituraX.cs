using FusionPdv.Ecf;

namespace FusionPdv.Servicos.Ecf.EstadoEcf
{
    public class RequerLeituraX : VerificaEstado
    {
        protected override void CondicaoDeVerificacao()
        {
            if(Estado == EstadoEcfFiscal.Relatorio)
                SessaoEcf.EcfFiscal.FechaRelatorio();

            if (Estado == EstadoEcfFiscal.RequerX)
                SessaoEcf.EcfFiscal.LeituraX();
        }
    }
}
