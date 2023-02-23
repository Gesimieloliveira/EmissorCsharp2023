using System.Windows;
using FusionPdv.Ecf;

namespace FusionPdv.Servicos.Ecf.EstadoEcf
{
    public class RequerLeituraZ : VerificaEstado
    {
        public RequerLeituraZ()
        {
        }

        public RequerLeituraZ(EstadoEcfFiscal estadoEcf) : base(estadoEcf)
        {
        }

        protected override void CondicaoDeVerificacao()
        {
            if (Estado != EstadoEcfFiscal.RequerZ) return;
            SessaoEcf.EcfFiscal.ReducaoZ();
            MessageBox.Show("Redução Z feita com sucesso.");
        }
    }
}
