using System.Windows;
using System.Windows.Controls;
using Fusion.Visao.NotaFiscalEletronica.Principal.Aba.Models;
using Fusion.Visao.Pessoa.Picker;
using Fusion.Visao.Veiculos;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Fiscal.Contratos;
using FusionCore.FusionAdm.Pessoas;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Aba
{
    public partial class AbaDestinatario
    {
        private AbaDestinatarioModel _viewModel;

        public AbaDestinatario()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is AbaDestinatarioModel model))
            {
                return;
            }

            _viewModel = model;
            TextBoxDestinatario.Focus();
        }

        private void ClickPickerVeiculoHandler(object sender, RoutedEventArgs e)
        {
            var pickerModel = new VeiculoPickerModel();
            pickerModel.PickItemEvent += SelecionaVeiculoPicker;
            pickerModel.GetPickerView().ShowDialog();
        }

        private void SelecionaVeiculoPicker(object sender, GridPickerEventArgs e)
        {
            var veiculo = e.GetItem<Veiculo>();
            _viewModel.TransportadoraModel.PreecherCom(veiculo);
        }

        private void OnClickProximoPasso(object sender, RoutedEventArgs e)
        {
            _viewModel.OnProximoPassoCalled();
        }

        private void OnClickPassoAnterior(object sender, RoutedEventArgs e)
        {
            _viewModel.OnPassoAnteriorCalled();
        }

        private void OnClickAdicionarVolume(object sender, RoutedEventArgs e)
        {
            _viewModel.AdicionarVolume();
        }

        private void RemoverVolumeClickHandler(object sender, RoutedEventArgs e)
        {
            var msg = "Continuar com a exclusão do volume selecionado?";

            if (!DialogBox.MostraDialogoDeConfirmacao(msg))
            {
                return;
            }

            var volume = (sender as Button)?.Tag as IVolume;

            _viewModel.RemoverVolume(volume);
        }

        private void TransportadorClickHandler(object sender, RoutedEventArgs e)
        {
            var picker = new PessoaPickerModel(new TransportadoraEngine());
            picker.PickItemEvent += (o, args) => _viewModel.Com(args.GetItem<Transportadora>());
            picker.GetPickerView().ShowDialog();
        }

        private void ClearTransportadorHandler(object sender, RoutedEventArgs e)
        {
            _viewModel.RemoverTransportadora();
        }
    }
}