using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Fusion.Visao.NotaFiscalEletronica.Principal.Controles;
using Fusion.Visao.NotaFiscalEletronica.Principal.Controles.Events;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.NotaFiscalEletronica.Principal
{
    public partial class ItemView
    {
        private ChildWindow _childAberta;
        public readonly ItemViewModel ViewModel = new ItemViewModel();

        public ItemView()
        {
            InitializeComponent();

            RegistrarAtalho(Key.Escape, Close);
        }

        private async void ViewKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (_childAberta != null)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.F2:
                    AcaoSalvarItem();
                    e.Handled = true;
                    break;

                case Key.F3:
                    await AcaoOutrasOpcoes();
                    e.Handled = true;
                    break;
            }
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            ItemControl.Contexto.ProdutoAlterado += ProdutoAlteradoHandler;
            ItemControl.Contexto.PropertyChanged += ItemContextoPropertyChanged;
            IpiControl.Contexto.PropertyChanged += IpiContextoPropertyChanged;

            ViewModel.ItemContexto = ItemControl.Contexto;
            ViewModel.IpiContexto = IpiControl.Contexto;
            ViewModel.IcmsContexto = IcmsControl.Contexto;
            ViewModel.PisCofinsContexto = PisCofinsControl.Contexto;
            ViewModel.Inicializar();
            
            DataContext = ViewModel;
        }

        private void ProdutoAlteradoHandler(object sender, ProdutoAlteradoEvent e)
        {
            IpiControl.Contexto.CarregarCom(e.Produto, e.Contexto);
            IcmsControl.Contexto.PreencherCom(e.Produto, e.Contexto);
            PisCofinsControl.Contexto.CarregarCom(e.Produto, e.Contexto);
        }

        private void ItemContextoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ItemContexto.Total))
            {
                IpiControl.Contexto.ValorTributavel = ItemControl.Contexto.Total;
                IcmsControl.Contexto.ValorTributavel = ItemControl.Contexto.Total;
                PisCofinsControl.Contexto.ValorTributavel = ItemControl.Contexto.Total;
            }
        }

        private void IpiContextoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IpiContexto.ValorIpi))
            {
                IcmsControl.Contexto.ValorIpi = IpiControl.Contexto.ValorIpi;
            }
        }

        private void SalvarClickHandler(object sender, RoutedEventArgs e)
        {
            AcaoSalvarItem();
        }

        private void AcaoSalvarItem()
        {
            BtnSalvar.Focus();

            try
            {
                ViewModel.SalvarAlteracoes();
                DialogResult = true;
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private async void OutraConfigMouseUpHandler(object sender, MouseButtonEventArgs e)
        {
            await AcaoOutrasOpcoes();
        }

        private async Task AcaoOutrasOpcoes()
        {
            _childAberta = new OutrasOpcoesChildWindow(ViewModel);
            _childAberta.ClosingFinished += (o, args) => { _childAberta = null; };

            await this.ShowChildWindowAsync(_childAberta);
        }
    }
}