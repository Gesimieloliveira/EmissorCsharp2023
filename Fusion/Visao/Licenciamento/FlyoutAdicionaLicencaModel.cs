using System;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Licenciamento
{
    public sealed class FlyoutAdicionaLicencaModel : ViewModel
    {
        public bool IsOpen
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public string ChaveMaquina
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        public string ContraChave
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        public FlyoutAdicionaLicencaModel(string cm, bool isOpen = true)
        {
            ChaveMaquina = cm;
            IsOpen = true;
        }

        public event EventHandler<FlyoutAdicionaLicencaModel> ClickAtivar;

        public void ClickAtivarHandler()
        {
            OnClickAtivar(this);
        }

        private void OnClickAtivar(FlyoutAdicionaLicencaModel e)
        {
            ClickAtivar?.Invoke(this, e);
        }
    }
}