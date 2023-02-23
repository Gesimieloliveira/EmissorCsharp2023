using System;
using System.Windows;
using System.Windows.Input;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.Controls;
using static MahApps.Metro.SimpleChildWindow.ChildWindowManager;

namespace Fusion.Visao.NotaFiscalEletronica.Principal
{
    public partial class NfeletronicaWizzard
    {
        private readonly NfeletronicaGrid _nfeEdicao;
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private TotaisNfeChildWindow _dialogTotais;

        public NfeletronicaWizzard(NfeletronicaGrid nfeEdicao = null)
        {
            _nfeEdicao = nfeEdicao;
            DataContext = new NfeletronicaWizzardModel(_sessaoManager);
            InitializeComponent();
        }

        private NfeletronicaWizzardModel GetView => DataContext as NfeletronicaWizzardModel;

        private void Wizzard_OnLoaded(object sender, RoutedEventArgs e)
        {
            GetView.CloseRequest += CloseRequestHandler;
            GetView.AbaItensModel.AlterarTotaisFixoCalled += AlterarTotiasFixoCalled;
            KeyDown += KeyDownAtalhos;

            GetView.Inicializar();

            if (_nfeEdicao != null)
            {
                GetView.InicializarParaEdicao(_nfeEdicao);
            }
        }

        private void KeyDownAtalhos(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                AbrirChildWindowValoresPeloAtalho();
                e.Handled = true;
            }
        }

        private void AbrirChildWindowValoresPeloAtalho()
        {
            if (!GetView.AbaItensModel.Selecionado || _dialogTotais?.IsOpen == true)
            {
                return;
            }

            foreach (var item in Flyouts.Items)
            {
                if (item is Flyout flyout && flyout.IsOpen)
                {
                    return;
                }
            }

            GetView.AbaItensModel.OnAlterarTotaisFixoCalled();
        }

        private async void AlterarTotiasFixoCalled(object sender, Nfeletronica e)
        {
            try
            {
                e.ChecarCobranca();

                _dialogTotais = new TotaisNfeChildWindow(e);

                _dialogTotais.Contexto.AlteracoesSalva += (o, nfe) => GetView.AbaItensModel.AtualizaDadosView();
                _dialogTotais.Closing += (o, args) => _dialogTotais = null;

                await this.ShowChildWindowAsync(_dialogTotais, OverlayFillBehavior.FullWindow);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void CloseRequestHandler(object sender, EventArgs e)
        {
            Close();
        }
    }
}