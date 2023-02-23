using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.Wpf.Componentes;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronicoOs.Grid
{
    public partial class GridCteOs
    {
        private readonly GridCteOsModel _model;

        public GridCteOs(GridCteOsModel model)
        {
            _model = model;
            DataContext = _model;
            InitializeComponent();
        }

        private void OnLoadedHanlder(object sender, RoutedEventArgs e)
        {
            _model.Inicializar();
        }

        private void DoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {
            _model.OpcoesCteOs();
        }

        private void ClickBtnOpcoesHandler(object sender, RoutedEventArgs e)
        {
            _model.OpcoesCteOs();
        }

        private void TextBoxPesquisa_OnOnSearch(object sender, RoutedEventArgs e)
        {
            try
            {
                var componentePesquisa = sender as TextBoxPesquisa;

                if (componentePesquisa == null)
                {
                    MessageBox.Show("Não foi possível obter o texto de pesquisa.");
                    return;
                }

                _model.AplicarPesquisa(componentePesquisa.Texto.TrimOrEmpty());
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro(ex.Message, ex);
            }
        }

        private void CopiarChave(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string chave)
            {
                Clipboard.SetText(chave);
            }
        }
    }
}
