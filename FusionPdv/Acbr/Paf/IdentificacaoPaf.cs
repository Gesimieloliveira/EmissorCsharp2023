using ACBrFramework.ECF;
using ACBrFramework.PAF;
using FusionCore.Helpers.EmpresaDesenvolvedora;

namespace FusionPdv.Acbr.Paf
{
    public class IdentificacaoPaf : IConfiguracaoPaf
    {

        private readonly ACBrPAF _acbrPaf;
        private readonly ACBrECF _acbrEcf;

        public IdentificacaoPaf()
        {
            _acbrPaf = AcbrFactory.ObterAcbrPaf();
            _acbrEcf = AcbrFactory.ObterAcbrEcf();
        }

        public void ExecutaConfiguracao()
        {
            const string laudo = "";
            var nome = ResponsavelLegal.NomeAplicacaoPdv;
            var versao = ResponsavelLegal.VersaoAplicacaoPdv;

            _acbrPaf.PafN.RegistroN2.LAUDO = laudo;
            _acbrPaf.PafN.RegistroN2.NOME = nome;
            _acbrPaf.PafN.RegistroN2.VERSAO = versao;
        }
    }
}
