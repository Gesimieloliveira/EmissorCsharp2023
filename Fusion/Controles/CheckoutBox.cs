using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Fusion.Controles.Checkout;
using Fusion.Visao.Produto;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;

namespace Fusion.Controles
{
    public class CheckoutBox : Control
    {
        private ISessaoManager _sessaoManager = new SessaoManagerAdm();
        public static readonly DependencyProperty CheckoutItemProperty = DependencyProperty.Register("CheckoutItem", typeof(CheckoutItem), typeof(CheckoutBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, CheckoutItemCallback));
        public static readonly DependencyProperty DontLostFocusProperty = DependencyProperty.Register("DontLostFocus", typeof(bool), typeof(CheckoutBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly RoutedEvent CheckoutItemChangedEvent = EventManager.RegisterRoutedEvent("CheckoutItemChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckoutBox));
        public static readonly RoutedEvent CheckoutErrrorEvent = EventManager.RegisterRoutedEvent("CheckoutErrror", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CheckoutBox));

        private IComando _comando;
        private ITabelaPreco _tabelaPrecos;
        private decimal _quantidade;
        private Grid _gridInfo;
        private TextBox _tbComando;
        private TextBlock _tbTextoInfo;
        private TextBlock _tbBuscaProduto;
        private Run _runQuantidade;
        private Button _iaBuscaProduto;

        public CheckoutItem CheckoutItem
        {
            set => SetValue(CheckoutItemProperty, value);
            get => (CheckoutItem)GetValue(CheckoutItemProperty);
        }

        public bool DontLostFocus
        {
            set => SetValue(DontLostFocusProperty, value);
            get => (bool)GetValue(DontLostFocusProperty);
        }

        private static void CheckoutItemCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is CheckoutBox checkout))
            {
                return;
            }

            checkout.RaiseEvent(new RoutedEventArgs(CheckoutItemChangedEvent, checkout));
            checkout.SetQuantidade(1.00M);
        }

        public event RoutedEventHandler CheckoutItemChanged
        {
            add => AddHandler(CheckoutItemChangedEvent, value);
            remove => RemoveHandler(CheckoutItemChangedEvent, value);
        }

        public event RoutedEventHandler CheckoutErrror
        {
            add => AddHandler(CheckoutErrrorEvent, value);
            remove => RemoveHandler(CheckoutErrrorEvent, value);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            e.Handled = true;

            if (_tbComando != null)
            {
                Keyboard.Focus(_tbComando);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _gridInfo = (Grid)GetTemplateChild("PART_GridInfo");
            _tbTextoInfo = (TextBlock)GetTemplateChild("PART_TextoInfo");
            _tbComando = (TextBox)GetTemplateChild("PART_TextoComando");
            _runQuantidade = (Run)GetTemplateChild("PART_RunQuantidade");
            _tbBuscaProduto = (TextBlock)GetTemplateChild("PART_TBBuscaProduto");
            _iaBuscaProduto = (Button)GetTemplateChild("PART_BTBuscaProduto");

            SetQuantidade(1.00M);
            SetComando(new ComandoBuscaCodigo(_sessaoManager));

            if (_tbComando != null)
            {
                _tbComando.PreviewTextInput += PreviewTextInputComandoHandler;
                _tbComando.PreviewKeyDown += PreviewKeyDownComandoHandler;
                _tbComando.PreviewLostKeyboardFocus += PreviewLostFocusComandoHandler;
            }

            if (_tbBuscaProduto != null)
            {
                _tbBuscaProduto.MouseDown += delegate { PickerProduto(); };
            }

            if (_iaBuscaProduto != null)
            {
                _iaBuscaProduto.Click += delegate { PickerProduto(); };
            }
        }

        public void SetQuantidade(decimal quantidade)
        {
            _quantidade = decimal.Round(quantidade, 4);
            _runQuantidade.Text = _quantidade.ToString("N3");
        }

        private void SetComando(IComando comando)
        {
            _comando = comando;
            _tbTextoInfo.Text = comando.TextoInformativo;
            _gridInfo.Background = comando.Background;
        }

        public decimal GetQuantidade()
        {
            return _quantidade;
        }

        public string GetTextoComando()
        {
            return _tbComando.Text;
        }

        private void PreviewTextInputComandoHandler(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "*")
            {
                return;
            }

            e.Handled = true;
            SetComando(new ComandoQuantidade());
        }

        private void PreviewKeyDownComandoHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.B)
            {
                e.Handled = true;
                PickerProduto();
                return;
            }

            if (e.Key != Key.Enter)
            {
                return;
            }

            e.Handled = true;

            try
            {
                _comando.Executar(this);
                _tbComando.Text = string.Empty;
                SetComando(new ComandoBuscaCodigo(_sessaoManager));
            }
            catch (InvalidOperationException ex)
            {
                RaiseEvent(new RoutedEventArgs(CheckoutErrrorEvent, ex));
            }
        }

        private void PreviewLostFocusComandoHandler(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (DontLostFocus)
            {
                e.Handled = true;
            }
        }

        private void PickerProduto()
        {
            var contexto = new ProdutoGridPickerModel(_tabelaPrecos);
            var dialog = contexto.GetPickerView();
            dialog.Closed += (sender, args) => { _tbComando.Focus(); };

            contexto.PickItemEvent += (sender, args) => { CheckoutItem = new CheckoutItem(args.GetItem<ProdutoDTO>(), GetQuantidade()); };


            dialog.ShowDialog();
        }

        public void ComTabelaPrecos(ITabelaPreco tabelaPreco)
        {
            _tabelaPrecos = tabelaPreco;
        }
    }
}