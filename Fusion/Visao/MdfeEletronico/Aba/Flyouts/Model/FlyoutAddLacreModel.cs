using System;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model
{
    public class SalvarLacreEventArgs : EventArgs
    {
        public SalvarLacreEventArgs(FlyoutAddLacreModel model)
        {
            Model = model;
        }

        public FlyoutAddLacreModel Model { get; set; }
    }
    public class FlyoutAddLacreModel : ViewModel
    {

        public event EventHandler<SalvarLacreEventArgs> SalvarLacreHandler;

        private string _numeroLacre;
        private bool _isOpen;

        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroLacre
        {
            get { return _numeroLacre; }
            set
            {
                if (value == _numeroLacre) return;
                _numeroLacre = value;
                PropriedadeAlterada();
            }
        }

        public void LimpaCampos()
        {
            NumeroLacre = string.Empty;
        }

        protected virtual void OnSalvarLacreHandler()
        {
            SalvarLacreHandler?.Invoke(this, new SalvarLacreEventArgs(this));
        }

        public void SalvarLacre()
        {
            Valida();
            Hidratacao();

            OnSalvarLacreHandler();
            LimpaCampos();
        }

        private void Valida()
        {
            if (NumeroLacre.IsNullOrEmpty()) throw new ArgumentException("Número Lacre é obrigatório");
        }

        private void Hidratacao()
        {
            NumeroLacre = NumeroLacre.TrimOrEmpty();
        }
    }
}