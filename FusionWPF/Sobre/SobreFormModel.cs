using FusionCore.Helpers.EmpresaDesenvolvedora;
using FusionLibrary.VisaoModel;

namespace FusionWPF.Sobre
{
    public class SobreFormModel : ViewModel
    {

        public SobreFormModel()
        {
            VersaoSistema = ResponsavelLegal.VersaoSistema;
        }

        private string _nomeSistema;
        private string _versaoSistema;

        public string NomeSistema
        {
            get => _nomeSistema;
            set
            {
                if (value == _nomeSistema) return;
                _nomeSistema = value;
                PropriedadeAlterada();
            }
        }

        public string VersaoSistema
        {
            get => _versaoSistema;
            set
            {
                if (value == _versaoSistema) return;
                _versaoSistema = value;
                PropriedadeAlterada();
            }
        }
    }
}