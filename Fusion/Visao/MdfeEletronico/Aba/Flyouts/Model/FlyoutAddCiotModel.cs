using System;
using FusionCore.Extencoes;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model
{
    public class SalvarCiotEvent : EventArgs
    {
        public FlyoutAddCiotModel Model { get; }

        public SalvarCiotEvent(FlyoutAddCiotModel model)
        {
            Model = model;
        }
    }

    public class FlyoutAddCiotModel : ViewModel
    {
        private string _documentoUnico;
        private string _ciot;
        private bool _isOpen;

        public event EventHandler<SalvarCiotEvent> SalvarCiotHandler; 

        public string DocumentoUnico
        {
            get { return _documentoUnico; }
            set
            {
                if (value == _documentoUnico) return;
                _documentoUnico = value;
                PropriedadeAlterada();
            }
        }

        public string Ciot
        {
            get { return _ciot; }
            set
            {
                if (value == _ciot) return;
                _ciot = value;
                PropriedadeAlterada();
            }
        }

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

        public void SalvarCiot()
        {
            Validar();
            OnSalvarCiotHandler();
            LimpaCampos();
        }

        private void Validar()
        {
            if (DocumentoUnico.IsNullOrEmpty()) throw new ArgumentException("Cpf/Cnpj é obrigatório");   
            if (Ciot.IsNullOrEmpty()) throw new ArgumentException("Ciot é obrigatório");
        }

        public void LimpaCampos()
        {
            DocumentoUnico = string.Empty;
            Ciot = string.Empty;
        }

        protected virtual void OnSalvarCiotHandler()
        {
            SalvarCiotHandler?.Invoke(this, new SalvarCiotEvent(this));
        }
    }
}