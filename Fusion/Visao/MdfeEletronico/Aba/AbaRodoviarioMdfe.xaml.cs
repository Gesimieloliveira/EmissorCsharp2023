using System;
using System.Windows;
using Fusion.Visao.MdfeEletronico.Aba.Model;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.MdfeEletronico.Aba
{
    public partial class AbaRodoviarioMdfe
    {
        private AbaRodoviarioMdfeModel _model;

        public AbaRodoviarioMdfe()
        {
            InitializeComponent();
        }

        private void ClickAdicionarCondutorHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                TabCondutor.IsSelected = true;
                _model.FlyoutAddCondutor();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void ClickAdicionarReboqueHandler(object sender, RoutedEventArgs e)
        {
            TabReboque.IsSelected = true;
            _model.FlyoutAddReboqueMdfe();
        }

        private void ClickAdicionarValePedagioHandler(object sender, RoutedEventArgs e)
        {
            TabPedagio.IsSelected = true;
            _model.FlyoutAddValePedagio();
        }

        private void OnClickBotaoAnterior(object sender, RoutedEventArgs e)
        {
            _model.OnAnteriorHandler();
        }

        private void ClickAdicionarVeiculoTracaoHandler(object sender, RoutedEventArgs e)
        {
            TabVeiculoTracao.IsSelected = true;
            _model.AbirFlyoutVeiculoTracao();
        }

        private void ClickAdicionarContratanteHandler(object sender, RoutedEventArgs e)
        {
            TabContratanteTomador.IsSelected = true;
            _model.AbrirFlyoutContratante();
        }

        private void ClickAdicionarCiotHandler(object sender, RoutedEventArgs e)
        {
            TabCiot.IsSelected = true;
            _model.FlyoutAddCiot();
        }

        private void EmitirMDFeOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.Emitir();
            }
            catch (ArgumentException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void AbaRodoviarioMdfe_OnLoaded(object sender, RoutedEventArgs e)
        {
            var model = DataContext as AbaRodoviarioMdfeModel;

            if (model == null) return;

            _model = model;
        }

        private void ClickAdicionarPagamentoHandler(object sender, RoutedEventArgs e)
        {
            TabPagamento.IsSelected = true;
            _model.AddInformacaoPagamento(false);
        }
    }
}
