using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Controles.Objetos;
using Fusion.Controles.Providers;
using Fusion.Visao.Produto;
using FusionCore.Core.Estoque;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Controles;
using WpfControls.Editors;

namespace Fusion.Controles
{
    public class ProdutoComboPicker : Control
    {
        private const string PartAutoComplete = "PART_AutoComplete";
        private const string PartCodigo = "PART_Codigo";
        public static readonly DependencyProperty SelecionadoProperty = DependencyProperty.Register("Selecionado", typeof(ProdutoCombo), typeof(ProdutoComboPicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelecionadoChangedHandler));
        public static readonly DependencyProperty FocusOnSuccessProperty = DependencyProperty.RegisterAttached("FocusOnSuccess", typeof(UIElement), typeof(ProdutoComboPicker));

        private AutoCompleteTextBox _autoComplete;
        private SearchTextBox _tbCodigo;
        private bool _atualizandoPropertySelecionado;
        private string _ultimoCodigoBuscado;
        private ProdutoDTO _produto;
        private ITabelaPreco _tabelaPreco;

        public ProdutoCombo Selecionado
        {
            set => SetValue(SelecionadoProperty, value);
            get => (ProdutoCombo) GetValue(SelecionadoProperty);
        }

        public UIElement FocusOnSuccess
        {
            get => (UIElement) GetValue(FocusOnSuccessProperty);
            set => SetValue(FocusOnSuccessProperty, value);
        }

        private static void SelecionadoChangedHandler(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var combo = (ProdutoComboPicker)d;
            combo._atualizandoPropertySelecionado = true;

            try
            {
                if (e.NewValue == null)
                {
                    combo._tbCodigo.Text = string.Empty;
                    combo._ultimoCodigoBuscado = string.Empty;
                    combo._autoComplete.SelectedItem = null;
                    return;
                }

                if (e.NewValue is ProdutoCombo selecionado)
                {
                    combo._tbCodigo.Text = selecionado.CodigoUtilizado;
                    combo._ultimoCodigoBuscado = selecionado.CodigoUtilizado;
                    combo._autoComplete.SelectedItem = selecionado.Produto;
                }
            }
            finally
            {
                combo._atualizandoPropertySelecionado = false;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _autoComplete = (AutoCompleteTextBox) GetTemplateChild(PartAutoComplete);
            _tbCodigo = (SearchTextBox) GetTemplateChild(PartCodigo);

            if (_autoComplete != null)
            {
                _autoComplete.Provider = new ProdutoSuggestionProvider();
                _autoComplete.SelecteItemChanged += AutoCompleteSelectedItemChangeHandler;
            }

            if (_tbCodigo != null)
            {
                _ultimoCodigoBuscado = _tbCodigo.Text;
                _tbCodigo.PreviewKeyDown += TbCodigoKeyDownHandler;
                _tbCodigo.PreviewLostKeyboardFocus += TbCodigoLostFocusHandler;
                _tbCodigo.SearchEvent += ClickCodigoPickerHandler;
            }
        }

        private void AutoCompleteSelectedItemChangeHandler(object sender, RoutedEventArgs e)
        {
            if (_atualizandoPropertySelecionado)
            {
                return;
            }

            if (_autoComplete.SelectedItem == null)
            {
                Selecionado = null;
                return;
            }

            if (_autoComplete.SelectedItem is IProdutoSelecionado selected)
            {
                Selecionado = new ProdutoCombo(selected);
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (_tbCodigo != null)
            {
                Keyboard.Focus(_tbCodigo);
            }
        }

        private void TbCodigoKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            if (_ultimoCodigoBuscado == _tbCodigo.Text)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(_tbCodigo.Text))
            {
                _ultimoCodigoBuscado = _tbCodigo.Text;
                Selecionado = null;
                return;
            }

            e.Handled = true;
            try
            {
                CarregarProduto();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
                Selecionado = null;
                _tbCodigo.Text = string.Empty;
                _tbCodigo.CaretIndex = 0;
                return;
            }

            if (_produto != null)
            {
                Selecionado = new ProdutoCombo(_produto, _tbCodigo.Text);
                MoveFocusOnSuccess();
                return;
            }

            Selecionado = null;

            _tbCodigo.Text = _ultimoCodigoBuscado;
            _tbCodigo.CaretIndex = _tbCodigo.Text?.Length ?? 0;
        }

        private void MoveFocusOnSuccess()
        {
            if (GetValue(FocusOnSuccessProperty) is UIElement next)
            {
                next.Focus();
                return;
            }

            _tbCodigo.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void CarregarProduto()
        {
            _ultimoCodigoBuscado = _tbCodigo.Text;

            if (string.IsNullOrWhiteSpace(_tbCodigo.Text))
            {
                _produto = null;
                return;
            }

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProduto(sessao);
                var produto = repositorio.BuscaPeloCodigo(_tbCodigo.Text);

                if (produto == null)
                {
                    throw new InvalidOperationException($"Nenhum produto encontrado para essa busca: ({_tbCodigo.Text})!");
                }

                produto.NaoAtivoThrowInvalidOperation();
                
                _produto = produto;
            }
        }

        private void TbCodigoLostFocusHandler(object sender, RoutedEventArgs e)
        {
            if (_ultimoCodigoBuscado == _tbCodigo.Text)
            {
                return;
            }

            try
            {
                CarregarProduto();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
                Selecionado = null;
                _tbCodigo.Text = string.Empty;
                _tbCodigo.CaretIndex = 0;
                return;
            }

            if (_produto == null)
            {
                return;
            }

            e.Handled = true;
            Selecionado = new ProdutoCombo(_produto, _tbCodigo.Text);
            MoveFocusOnSuccess();
        }

        private void ClickCodigoPickerHandler(object sender, RoutedEventArgs e)
        {
            var pickerModel = new ProdutoGridPickerModel(_tabelaPreco);
            var selecionou = false;

            pickerModel.PickItemEvent += (s, ev) =>
            {
                Selecionado = new ProdutoCombo(ev.GetItem<IProdutoSelecionado>());
                selecionou = true;
            };

            pickerModel.GetPickerView().ShowDialog();

            if (selecionou)
            {
                MoveFocusOnSuccess();
            }
        }

        public void LimparCampos()
        {
            _tbCodigo.Text = string.Empty;
            _autoComplete.Text = string.Empty;
            Selecionado = null;
            _ultimoCodigoBuscado = string.Empty;
            _produto = null;
        }

        public void FocusTbCodigo()
        {
            _tbCodigo.Focus();
            _tbCodigo.SelectAll();
        }

        public void AtualizarTabelaPreco(ITabelaPreco tabelaPreco)
        {
            _tabelaPreco = tabelaPreco;
        }
    }
}
