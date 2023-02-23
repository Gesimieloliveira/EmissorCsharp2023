using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.FastReport.Relatorios;
using Fusion.Sessao;
using Fusion.Visao.Relatorios.Comum;
using FusionCore.FusionAdm.Setup.Conexao;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Relatorios
{
    public partial class ListagemRelatoriosControl
    {
        private readonly ListagemRelatoriosContexto _contexto;
        private readonly ISessaoManager _sessaoMananger = new SessaoManagerAdm();

        public ListagemRelatoriosControl(ListagemRelatoriosContexto contexto)
        {
            InitializeComponent();

            ToolbarModoDesign.Visibility = SessaoSistema.Instancia.IsAdmin
                ? Visibility.Visible
                : Visibility.Collapsed;

            _contexto = contexto;
            _contexto.QuandoMudarComPartida(i => i.ModoDesigner, ModoDesignerChanged);
        }

        private void ModoDesignerChanged(ListagemRelatoriosContexto o)
        {
            BtnAtivarDesign.Visibility = o.ModoDesigner ? Visibility.Collapsed : Visibility.Visible;
            GrupoBtnsDesignAtivo.Visibility = o.ModoDesigner ? Visibility.Visible : Visibility.Collapsed;
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = _contexto;
            Search.Focus();

            _contexto.ListarRelatorios();
        }

        private void RelatorioDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            _contexto.ItemSelecionado?.Visualizar();
        }

        private void EditarDesenhoClickHandler(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button b) || !(b.Tag is IOpcaoRelatorio o))
            {
                return;
            }

            AcaoEditarTemplate(o);
        }

        private void AcaoEditarTemplate(IOpcaoRelatorio o)
        {
            try
            {
                if (o.TemplateId != Guid.Empty)
                {
                    FrSettings.Instancia.RegistrarSalvamentoCustomizado(o.TemplateId, _contexto.SalvarTemplate);
                }

                o.EditarDesenho();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void NovoFastReportHandler(object sender, RoutedEventArgs e)
        {
            var contexto = new CadastroFastReportContexto(_sessaoMananger);
            var dialog = new CadastroFastReport(contexto);

            contexto.SalvouComSucesso += (o, r) =>
            {
                const string msg = "Deseja editar o relatório agora?";

                if (DialogBox.MostraConfirmacao(msg) != MessageBoxResult.Yes)
                {
                    return;
                }

                AcaoEditarTemplate(new OpcaoRelatorioProprio(r, _sessaoMananger));
            };

            if (dialog.ShowDialog() == true)
            {
                _contexto.ListarRelatorios();
            }
        }

        private void ClickEditarDevHandler(object sender, RoutedEventArgs e)
        {
            var opcao = (IOpcaoRelatorio) ((Button) sender).Tag;
            opcao.DevEditarDesenho();
        }

        private void ExportarClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var opcao = (IOpcaoRelatorio) ((Button) sender).Tag;
                opcao.ExportarTemplate();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void ExcluirClickHandler(object sender, RoutedEventArgs e)
        {
            const string msg = "Deseja continuar com a exclusão?";

            if (DialogBox.MostraConfirmacao(msg) != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                var opcao = (IOpcaoRelatorio) ((Button) sender).Tag;
                opcao.ExcluirRelatorio();

                _contexto.ListarRelatorios();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void EditarInformacoesClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var opcao = (IOpcaoRelatorio) ((Button) sender).Tag;
                opcao.EditarInforamacoesRelatorio();

                _contexto.ListarRelatorios();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void AtivarModoDesignerClickHandler(object sender, RoutedEventArgs e)
        {
            _contexto.AtivarModoDesigner();
        }

        private void FiltroClickHandler(object sender, RoutedEventArgs e)
        {
            _contexto.ListarRelatorios();
        }

        private void FiltroKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            _contexto.ListarRelatorios();

            e.Handled = true;
        }

        private void SairModoDesignClickHandler(object sender, RoutedEventArgs e)
        {
            _contexto.DesativarModoDesigner();
        }

        private void ImportarTemplateClickHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                var opcao = (IOpcaoRelatorio)((Button) sender).Tag;
                opcao.ImportarTemplate();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}