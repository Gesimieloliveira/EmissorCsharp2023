using System.Windows;
using Fusion.Visao.Compras.Importacao.Models;

namespace Fusion.Visao.Compras.Importacao.Contents
{
    public partial class ImportacaoContent
    {
        public ImportacaoContent()
        {
            InitializeComponent();
        }

        private void VisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool) e.NewValue)
            {
                TbSerie.Focus();
            }
        }

        private void FatorLostFocusHandler(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement el && el.DataContext is ItemImportacaoVM vm)
            {
                vm.ArmazenaVinculo();
            }
        }
    }
}