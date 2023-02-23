using System;
using System.Windows;
using FusionCore.FusionAdm.Emissores;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.EmissorFiscalEletronico
{
    public partial class EmissorFiscalForm
    {
        private readonly EmissorFiscalFormModel _viewModel;

        public EmissorFiscalForm(EmissorFiscalFormModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (_viewModel == null)
            {
                return;
            }

            DataContext = _viewModel.InicializaContexto();
            ConfigurarTela();
        }
        private void ConfigurarTela()
        {
            if (_viewModel.Id == 0)
            {
                BotaoSalvar.Content = "Salvar Inclusão";
                BotaoDeletar.Visibility = Visibility.Collapsed;
            };
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModel.SalvarModel();
                DialogBox.MostraInformacao("Salvamos seu registro.");
                Close();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }

        private void OnClickDeletar(object sender, RoutedEventArgs e)
        {

            if (!DialogBox.MostraDialogoDeConfirmacao("Quer mesmo excluir?"))
            {
                return;
            }

            try
            {
                _viewModel.DeletarModel();
                DialogBox.MostraInformacao("Emissor deletado");
                Close();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }

        public static EmissorFiscalFormModel CriaModel()
        {
            return new EmissorFiscalFormModel(new EmissorFiscal
            {
                EmissorFiscalNfe = new EmissorFiscalNFE(),
                EmissorFiscalNfce = new EmissorFiscalNFCE(),
                EmissorFiscalCte = new EmissorFiscalCTE(),
                EmissorFiscalCteOs = new EmissorFiscalCTeOS(),
                EmissorFiscalMdfe = new EmissorFiscalMDFE(),
                EmissorFiscalSat = new EmissorFiscalSAT(),
                AutorizadoBaixarXml = new AutorizadoBaixarXml()
            });
        }

        public static EmissorFiscalFormModel CriaModel(EmissorFiscal emissorFiscal)
        {
            return new EmissorFiscalFormModel(emissorFiscal);
        }
    }
}