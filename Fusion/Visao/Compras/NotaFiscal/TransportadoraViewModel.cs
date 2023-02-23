using System.Windows.Input;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Pessoa;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;

namespace Fusion.Visao.Compras.NotaFiscal
{
    public class TransportadoraViewModel : ViewModel
    {
        private Transportadora _transportadora;

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

        public bool TemTransportadora => _transportadora != null;
        public ICommand PickerCommand => GetSimpleCommand(PickerCommandAction);
        public ICommand ClearCommand => GetSimpleCommand(ClearCommandAction);

        private void ClearCommandAction(object obj)
        {
            CancelarEscolha();
        }

        private void PickerCommandAction(object obj)
        {
            var picker = new PessoaPickerModel(new TransportadoraEngine());
            picker.PickItemEvent += PickerItemHandler;

            var view = picker.GetPickerView();
            view.ShowDialog();
        }

        private void CancelarEscolha()
        {
            _transportadora = null;

            Nome = string.Empty;
            DocumentoUnico = string.Empty;
            InscricaoEstadual = string.Empty;

            PropriedadeAlterada(nameof(TemTransportadora));
        }

        private void PickerItemHandler(object sender, GridPickerEventArgs e)
        {
            var selecionado = e.GetItem<Transportadora>();
            CarregarCom(selecionado);
        }

        public void CarregarCom(Transportadora transportadora)
        {
            _transportadora = transportadora;

            Nome = transportadora?.Nome;
            DocumentoUnico = transportadora?.GetDocumentoUnico();
            InscricaoEstadual = transportadora?.InscricaoEstadual;

            PropriedadeAlterada(nameof(TemTransportadora));
        }

        public Transportadora Get()
        {
            return _transportadora;
        }
    }
}
