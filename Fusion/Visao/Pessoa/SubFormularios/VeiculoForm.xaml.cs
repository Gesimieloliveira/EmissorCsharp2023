using System.Windows;
using Fusion.Visao.Veiculos;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.CteEletronico.Flags;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Pessoa.SubFormularios
{
    public partial class VeiculoForm
    {
        private readonly VeiculoFormModel _formModel;

        public VeiculoForm(VeiculoFormModel formModel)
        {
            InitializeComponent();
            DataContext = formModel;
            _formModel = formModel;
            _formModel.OperacaoFinalizada += delegate { Close(); };
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _formModel?.CarregarDados();
        }

        private void ConfirmarHandler(object sender, RoutedEventArgs e)
        {
            _formModel.ConfirmaVeiculo();
        }

        private void FecharHandler(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ProprietarioClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoSelecionarProprietario();
        }

        private void AcaoSelecionarProprietario()
        {
            if (_formModel.TipoProprietario != TipoPropriedadeVeiculo.Terceiro)
            {
                DialogBox.MostraAviso("Apenas quando tipo for Terceiro que é permitido selecionar o proprietário");
                return;
            }

            var picker = new ProprietarioVeiculoPickerModel();

            picker.PickItemEvent += (a, e) 
                => _formModel.Proprietario = e.GetItem<ProprietarioVeiculo>();

            picker.ShowPickerDialog();
        }

        private void ClearProprietarioHandler(object sender, RoutedEventArgs e)
        {
            _formModel.Proprietario = null;
        }
    }
}