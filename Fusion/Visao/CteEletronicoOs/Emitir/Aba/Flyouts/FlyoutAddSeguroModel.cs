using System;
using System.Windows.Input;
using FusionCore.FusionAdm.CteEletronicoOs.Flags;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronicoOs.Emitir.Aba.Flyouts
{
    public class FlyoutAddSeguroModel : ViewModel
    {
        public FlyoutAddSeguroModel()
        {
            ResponsavelSeguroSelecionado = ResponsavelSeguro.TomadorDeServico;
        }

        public event EventHandler<FlyoutAddSeguroModel> Salvar; 

        public ICommand CommandSalvarSeguro => GetSimpleCommand(SalvarSeguroAction);

        private bool _isOpen;
        private ResponsavelSeguro _responsavelSeguroSelecionado;
        private string _nomeSeguradora;
        private string _numeroApolice;

        public ResponsavelSeguro ResponsavelSeguroSelecionado
        {
            get => _responsavelSeguroSelecionado;
            set
            {
                if (value == _responsavelSeguroSelecionado) return;
                _responsavelSeguroSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public string NomeSeguradora
        {
            get => _nomeSeguradora;
            set
            {
                if (value == _nomeSeguradora) return;
                _nomeSeguradora = value;
                PropriedadeAlterada();
            }
        }

        public string NumeroApolice
        {
            get => _numeroApolice;
            set
            {
                if (value == _numeroApolice) return;
                _numeroApolice = value;
                PropriedadeAlterada();
            }
        }

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                PropriedadeAlterada();
            }
        }

        private void SalvarSeguroAction(object obj)
        {
            NomeSeguradora = NomeSeguradora.TrimOrEmpty();
            NumeroApolice = NumeroApolice.TrimOrEmpty();

            try
            {
                if (ResponsavelSeguroSelecionado == ResponsavelSeguro.Nenhum)
                    throw new ArgumentException("Responsável pelo seguro obrigatório");

                OnSalvar();
                IsOpen = false;
            }
            catch (ArgumentException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        protected virtual void OnSalvar()
        {
            Salvar?.Invoke(this, this);
        }
    }
}