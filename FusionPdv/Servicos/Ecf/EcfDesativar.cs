using ACBrFramework.ECF;
using FusionPdv.Acbr;

namespace FusionPdv.Servicos.Ecf
{
    public class EcfDesativar
    {
        private readonly ACBrECF _acbrEcf;

        public EcfDesativar()
        {
            _acbrEcf = AcbrFactory.ObterAcbrEcf();
        }

        public void Desativar()
        {
            if (_acbrEcf.Ativo)
            {
                _acbrEcf.Desativar();
            }
        }
    }
}
