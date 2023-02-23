using System.Windows;
using System.Windows.Controls;
using Fusion.Visao.PerfilCfop;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.GridPicker;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Controles
{
    public partial class ItemControl
    {
        public ItemControl()
        {
            Contexto = new ItemContexto();
            Contexto.AtualizaTabelaPreco += delegate (object o, ITabelaPreco preco)
            {
                CbProduto.AtualizarTabelaPreco(preco);
            };
            InitializeComponent();
        }

        public ItemContexto Contexto { get; }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            DataContext = Contexto;
            CbProduto.Focus();
        }

        private void PickerCfopClickHandler(object sender, RoutedEventArgs e)
        {
            var vm = new PerfilCfopPickerModel();
            vm.PickItemEvent += CfopSelecionadoHandler;

            vm.GetPickerView().ShowDialog();
        }

        private void CfopSelecionadoHandler(object sender, GridPickerEventArgs e)
        {
            Contexto.CfopSelecionado = e.GetItem<PerfilCfopDTO>();
            TbQuantidade.Focus();
        }

        private void PickerCfopLostFocusHandler(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is TextBox tb))
            {
                return;
            }

            var len = tb.Text.Length;

            if (len != 4 && len != 6)
            {
                Contexto.CfopSelecionado = null;
                return;
            }

            Contexto.SelecionarCfopPeloCodigo(tb.Text);
        }
    }
}