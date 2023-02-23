using ACBrFramework.PAF;
using FusionCore.Helpers.EmpresaDesenvolvedora;

namespace FusionPdv.Acbr.Paf
{
    public class DadosEmpresaDesenvolvedora : IConfiguracaoPaf
    {
        private readonly ACBrPAF _acbrPaf;

        public DadosEmpresaDesenvolvedora()
        {
            _acbrPaf = AcbrFactory.ObterAcbrPaf();
        }

        public void ExecutaConfiguracao()
        {
            _acbrPaf.PafN.RegistroN1.RazaoSocial = ResponsavelLegal.RazaoSocial;
            _acbrPaf.PafN.RegistroN1.UF = ResponsavelLegal.EstadoUf.Sigla;
            _acbrPaf.PafN.RegistroN1.CNPJ = ResponsavelLegal.Cnpj; 
            _acbrPaf.PafN.RegistroN1.IE = ResponsavelLegal.InscricaoEstadual;
            _acbrPaf.PafN.RegistroN1.IM = ResponsavelLegal.InscricaoMunicipal;
        }
    }
}
