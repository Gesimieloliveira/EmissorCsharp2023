using System;
using System.Windows.Input;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model
{
    public class SalvarCondutorEventArgs : EventArgs
    {
        public SalvarCondutorEventArgs(FlyoutAddCondutorModel model)
        {
            Model = model;
        }

        public FlyoutAddCondutorModel Model { get; set; }
    }

    public class FlyoutAddCondutorModel : ViewModel
    {
        private bool _isOpen;
        private PessoaEntidade _pessoaSelecionada;
        private string _cpf;
        private string _nome;

        public event EventHandler<SalvarCondutorEventArgs> SalvarCondutorHandler; 

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

        public FusionCore.FusionAdm.Pessoas.PessoaEntidade PessoaSelecionada
        {
            get { return _pessoaSelecionada; }
            set
            {
                _pessoaSelecionada = value;
                Cpf = _pessoaSelecionada.Cpf.Valor;
                Nome = _pessoaSelecionada.Nome;
            }
        }

        public ICommand CommandBuscarCondutor => GetSimpleCommand(BuscarCondutor);

        private void BuscarCondutor(object obj)
        {
            var pickerModel = new PessoaPickerModel(new PessoaEngine());
            pickerModel.PickItemEvent += CondutorSelecionadoCompleted;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void CondutorSelecionadoCompleted(object sender, GridPickerEventArgs e)
        {
            try
            {
                var pessoa = e.GetItem<PessoaEntidade>();

                if (pessoa.Tipo != PessoaTipo.Fisica)
                {
                    throw new ArgumentException("Somente e permitido pessoas do tipo Física");
                }

                PessoaSelecionada = pessoa;
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        public string Cpf
        {
            get { return _cpf; }
            set
            {
                if (value == _cpf) return;
                _cpf = value;
                PropriedadeAlterada();
            }
        }

        public string Nome
        {
            get { return _nome; }
            set
            {
                if (value == _nome) return;
                _nome = value;
                PropriedadeAlterada();
            }
        }

        public void LimpaCampos()
        {
            Cpf = string.Empty;
            Nome = string.Empty;
        }

        public void SalvarCondutor()
        {
            Validar();
            OnSalvarCondutorHandler();
        }

        private void Validar()
        {
            if (Cpf.IsNullOrEmpty()) throw new ArgumentException("O condutor deve ter CPF");
            if (PessoaSelecionada.Tipo == PessoaTipo.Juridica || PessoaSelecionada.Tipo == PessoaTipo.Extrangeiro)
                throw new ArgumentException("O condutor deve ser uma pessoa física");
        }

        protected virtual void OnSalvarCondutorHandler()
        {
            SalvarCondutorHandler?.Invoke(this, new SalvarCondutorEventArgs(this));
        }
    }
}