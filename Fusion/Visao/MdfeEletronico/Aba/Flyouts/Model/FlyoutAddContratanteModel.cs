using System;
using System.Windows.Input;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Helpers.Pessoa;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;

namespace Fusion.Visao.MdfeEletronico.Aba.Flyouts.Model
{
    public class SalvarContratanteEvent : EventArgs
    {
        public FlyoutAddContratanteModel Model { get; }

        public SalvarContratanteEvent(FlyoutAddContratanteModel model)
        {
            Model = model;
        }
    }

    public class FlyoutAddContratanteModel : ViewModel
    {
        private string _documentoUnico;
        private string _nome;
        private bool _isOpen;

        public event EventHandler<SalvarContratanteEvent> SalvarContratanteHandler; 

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

        public PessoaEntidade PessoaEntidade { get; set; }

        public ICommand CommandBuscarCondutor => GetSimpleCommand(BuscarCondutorAction);

        private void BuscarCondutorAction(object obj)
        {
            var pickerModel = new PessoaPickerModel(new PessoaEngine());
            pickerModel.PickItemEvent += ContratanteSelecionadoCompleted;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void ContratanteSelecionadoCompleted(object sender, GridPickerEventArgs e)
        {
            PessoaEntidade = e.GetItem<PessoaEntidade>();
            Nome = PessoaEntidade.Nome;
            DocumentoUnico = PessoaEntidade.GetDocumentoUnico();
        }

        public void LimpaCampos()
        {
            Nome = string.Empty;
            DocumentoUnico = string.Empty;
            PessoaEntidade = null;
        }

        public void SalvarContratante()
        {
            Validar();
            OnSalvarContratanteHandler();
            LimpaCampos();
        }

        private void Validar()
        {
            if (PessoaEntidade == null) throw new ArgumentException("Selecionar um Contratante");

            if (PessoaEntidade.GetDocumentoUnico().IsNullOrEmpty())
                throw new ArgumentException("Contratante deve ter cpf, cnpj ou id estrangeiro");
        }

        protected virtual void OnSalvarContratanteHandler()
        {
            SalvarContratanteHandler?.Invoke(this, new SalvarContratanteEvent(this));
        }
    }
}