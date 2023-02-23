using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.CCe;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.NfeCartaCorrecao
{
    public partial class NfeCartaCorrecaoForm
    {
        private readonly NfeCartaCorrecaoFormModel _visaoModelo;

        public NfeCartaCorrecaoForm(Nfeletronica nfe)
        {
            _visaoModelo = new NfeCartaCorrecaoFormModel(nfe);
            _visaoModelo.SucessoEmissao += SucessoEmissaoHandler;
            _visaoModelo.FalhaEmissao += FalhaEmissaoHandler;

            DataContext = _visaoModelo;
            InitializeComponent();
        }

        private void SucessoEmissaoHandler(object sender, CartaCorrecaoNfe e)
        {
            ProgressBarAgil4.CloseProgressBar();
            _visaoModelo.ImprimirCce(e);
            Close();
        }

        private void FalhaEmissaoHandler(object sender, string e)
        {
            ProgressBarAgil4.CloseProgressBar();
            DialogBox.MostraAviso(e);
            Close();
        }

        private void OnClickEnviar(object sender, RoutedEventArgs e)
        {
            ProgressBarAgil4.ShowProgressBar();
            _visaoModelo.EmitirCceAsync();
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            _visaoModelo.Validation_Error(sender, e);
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            _visaoModelo.PreencherViewModel();
        }

        private void OnClickImprimirCCe(object sender, MouseButtonEventArgs e)
        {
            _visaoModelo.ImprimirCCeSelecionada();
        }
    }
}