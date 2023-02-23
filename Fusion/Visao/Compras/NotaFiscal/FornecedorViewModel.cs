using System.Windows.Input;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Pessoa;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;

namespace Fusion.Visao.Compras.NotaFiscal
{
    public class FornecedorViewModel : ViewModel
    {
        private Fornecedor _fornecedor;

        public string Nome
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string DocumentoUnico
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string InscricaoEstadual
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool TemFornecedor => _fornecedor != null;

        public ICommand PickerCommand => GetSimpleCommand(PickerCommandAction);
        public ICommand ClearCommand => GetSimpleCommand(ClearCommandAction);

        private void ClearCommandAction(object obj)
        {
            CancelarEscolha();
        }

        private void PickerCommandAction(object obj)
        {
            var picker = new PessoaPickerModel(new FornecedorEngine());
            picker.PickItemEvent += PickerItemHandler;

            var view = picker.GetPickerView();
            view.ShowDialog();
        }

        private void PickerItemHandler(object sender, GridPickerEventArgs e)
        {
            var fornecedor = e.GetItem<Fornecedor>();
            CarregarCom(fornecedor);
        }

        private void CancelarEscolha()
        {
            _fornecedor = null;

            Nome = string.Empty;
            DocumentoUnico = string.Empty;
            InscricaoEstadual = string.Empty;

            PropriedadeAlterada(nameof(TemFornecedor));
        }

        public void CarregarCom(Fornecedor fornecedor)
        {
            _fornecedor = fornecedor;

            Nome = fornecedor.Nome;
            DocumentoUnico = fornecedor.GetDocumentoUnico();
            InscricaoEstadual = fornecedor.InscricaoEstadual;

            PropriedadeAlterada(nameof(TemFornecedor));
        }

        public Fornecedor Get()
        {
            return _fornecedor;
        }
    }
}