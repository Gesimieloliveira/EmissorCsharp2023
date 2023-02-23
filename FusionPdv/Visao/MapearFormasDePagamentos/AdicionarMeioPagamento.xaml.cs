using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ACBrFramework;
using FusionCore.FusionPdv.Ecf;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.MapearFormasDePagamentos
{
    public partial class AdicionarMeioPagamento
    {
        private readonly AdicionarMeioPagamentoModel _adicionarMeioPagamentoModel;

        public AdicionarMeioPagamento(IList<FormaPagamento> formaPagamentosEcf)
        {
            InitializeComponent();
            _adicionarMeioPagamentoModel = new AdicionarMeioPagamentoModel(formaPagamentosEcf);
            DataContext = _adicionarMeioPagamentoModel;
        }

        private void BtFechar_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtAdicionar_OnClick(object sender, RoutedEventArgs e)
        {
            AdicionarFormaDePagamento();
        }

        private void AdicionarMeioPagamento_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    AdicionarFormaDePagamento();
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private void AdicionarFormaDePagamento()
        {
            try
            {

                _adicionarMeioPagamentoModel.SalvarMeioDePagamento();
            }
            catch (ACBrException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void NomeFinalizador_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
