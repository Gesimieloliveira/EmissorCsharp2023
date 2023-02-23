using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Sessao;
using FusionCore.Helpers.Basico;
using FusionCore.Papeis;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.Usuario
{
    public partial class GerenciarPermissao
    {
        private readonly GerenciarPermissaoContexto _model;

        public GerenciarPermissao(SessaoSistema sessaoSistema)
        {
            sessaoSistema.UsuarioLogado.VerificaPermissao.IsTemPermissaoThrow(Permissao.GERENCIAR_PAPEL_USUARIO);

            InitializeComponent();

            _model = new GerenciarPermissaoContexto();
        }

        private void GerenciarPermissao_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = _model;

            CriarElementosParaOpcao();

            _model.CarregarOsPapeis();
        }

        private void CriarElementosParaOpcao()
        {
            _model.CarregarListaDeOpcoes();

            var items = new List<MetroTabItem>();
            
            foreach (var opcao in _model.ListaDeOpcoes.GroupBy(i => i.Grupo))
            {
                var tab = new MetroTabItem
                {
                    Header = opcao.Key.GetDescription(),
                    Content = GerarListaDeOpcoes(opcao)
                };

                items.Add(tab);
            }

            TabPermissoes.ItemsSource = items;
            TabPermissoes.SelectedIndex = 0;
        }

        private ListBox GerarListaDeOpcoes(IEnumerable<OpcaoPermissao> opcao)
        {
            var colecao = CollectionViewSource.GetDefaultView(opcao.ToList());

            colecao.GroupDescriptions.Clear();
            colecao.GroupDescriptions.Add(new PropertyGroupDescription("SubGrupoTexto"));

            var stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            var dockPanel = new FrameworkElementFactory(typeof(DockPanel));

            var textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetValue(DockPanel.DockProperty, Dock.Left);
            textBlock.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
            textBlock.SetValue(TextBlock.FontSizeProperty, 14.0);
            textBlock.SetValue(TextBlock.TextProperty, new Binding("Descricao"));

            var toogleSwitch = new FrameworkElementFactory(typeof(ToggleSwitch));
            toogleSwitch.SetValue(ToggleSwitch.ContentDirectionProperty, FlowDirection.LeftToRight);
            toogleSwitch.SetValue(HorizontalContentAlignmentProperty, HorizontalAlignment.Right);
            toogleSwitch.SetValue(ToggleSwitch.OnContentProperty, "Sim");
            toogleSwitch.SetValue(ToggleSwitch.OffContentProperty, "Não");
            toogleSwitch.SetValue(DockPanel.DockProperty, Dock.Left);
            toogleSwitch.SetValue(ToggleSwitch.IsOnProperty, new Binding("IsChecked"));

            dockPanel.AppendChild(textBlock);
            dockPanel.AppendChild(toogleSwitch);
            stackPanel.AppendChild(dockPanel);

            var listBox = new ListBox
            {
                Height = 453.0,
                Style = FindResource("FusionListBox") as Style,
                ItemTemplate = new DataTemplate
                {
                    VisualTree = stackPanel
                },
                ItemsSource = colecao,
            };

            listBox.GroupStyle.Add(new GroupStyle
            {
                ContainerStyle = (Style)Resources["GerenciaPermissao_GroupHeaderStyle"]
            });

            listBox.SetValue(VirtualizingStackPanel.VirtualizationModeProperty, VirtualizationMode.Standard);

            return listBox;
        }

        private async void InserirPapel_OnClick(object sender, RoutedEventArgs e)
        {
            var model = new PapelUsuarioFormModel(new Papel());
            var view = new PapelUsuarioForm(model);

            model.OperacaoSucesso += (o, papel) =>
            {
                _model.CarregarOsPapeis();
                _model.PapelSelecionado = papel;

                view.Close();
            };

            await this.ShowChildWindowAsync(view, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private async void AlterarPapel_OnClick(object sender, RoutedEventArgs e)
        {
            if (NaoExistePapel()) return;

            var model = new PapelUsuarioFormModel(_model.PapelSelecionado);
            var view = new PapelUsuarioForm(model);

            model.OperacaoSucesso += (o, papel) =>
            {
                _model.CarregarOsPapeis();

                _model.PapelSelecionado = null;
                _model.PapelSelecionado = papel;

                view.Close();
            };

            await this.ShowChildWindowAsync(view, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private bool NaoExistePapel()
        {
            if (_model.PapelSelecionado != null) return false;
            DialogBox.MostraInformacao("Insira um papel para editar");
            return true;

        }

        private async void VincularUsuarioNoPapel_OnClick(object sender, RoutedEventArgs e)
        {
            if (NaoExistePapel())
            {
                return;
            }

            var model = new VincularUsuarioAoPapelFormModel(_model.PapelSelecionado);
            var view = new VincularUsuarioAoPapelForm(model);

            model.UsuarioFoiAdicionado += (o, usuario) =>
            {
                _model.AtualizarListagemDeUsuarios();

                view.Close();
            };

            await this.ShowChildWindowAsync(view, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private void DesvincularUsuario_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.Tag is UsuarioDTO u)
            {
                _model.DesvincularUsuario(u);
            }
        }

        private void PermitirTudoClickHandler(object sender, RoutedEventArgs e)
        {
            const string msg = "Permitir todas as opções para esse Papel?";

            DialogBox.MostraDialogoDeConfirmacao(msg, () => _model.PermitirTudo());
        }

        private void NegarTudoClickHandler(object sender, RoutedEventArgs e)
        {
            const string msg = "Negar todas as opções para esse Papel?";

            DialogBox.MostraDialogoDeConfirmacao(msg, () => _model.NegarTudo());
        }
    }
}
