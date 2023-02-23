using FusionLibrary.VisaoModel;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba.Model
{
    public class AbaCTeOSViewModel : ViewModel
    {
        protected AbaCTeOSViewModel()
        {
            DesabilitaAba();
        }

        private bool _isHabilitado;
        private bool _isSelecionado;

        public bool IsHabilitado
        {
            get => _isHabilitado;
            set
            {
                if (value == _isHabilitado) return;
                _isHabilitado = value;
                PropriedadeAlterada();
            }
        }

        public bool IsSelecionado
        {
            get => _isSelecionado;
            set
            {
                if (value == _isSelecionado) return;
                _isSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public void HabilitaAba()
        {
            IsHabilitado = true;
            IsSelecionado = true;
        }

        public void DesabilitaAba()
        {
            IsHabilitado = false;
            IsSelecionado = false;
        }
    }
}