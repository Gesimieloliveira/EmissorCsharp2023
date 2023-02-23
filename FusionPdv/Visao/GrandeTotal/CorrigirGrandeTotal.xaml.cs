using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using FusionPdv.Servicos.Ecf;
using FusionPdv.Servicos.ValidacaoInicial;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.GrandeTotal
{
    public partial class CorrigirGrandeTotal
    {
        private readonly CorrigirGrandeTotalModel _corrigirGrandeTotalModel;

        public CorrigirGrandeTotal()
        {
            _corrigirGrandeTotalModel = new CorrigirGrandeTotalModel();
            InitializeComponent();
            DataContext = _corrigirGrandeTotalModel;

            TbCodigoAutorizacao.Focus();
        }

        private void CorrigirGrandeTotal_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    CorrigirGt();
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private void CorrigirGt()
        {
            var messageBoxResult = DialogBox.MostraConfirmacao("Deseja realmente corrigir o Grande Total (GT)");

            if (messageBoxResult != MessageBoxResult.Yes) return;

            try
            {
                _corrigirGrandeTotalModel.CorrigirGt();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraErro(ex.Message);
            }
            Close();
        }

        private void CorrigirGrandeTotal_OnClosing(object sender, CancelEventArgs e)
        {
            try
            {
                new EcfVerificaGt().Evalido();
            }
            catch (ExceptionGtEcf ex)
            {
                e.Cancel = true;
                DialogBox.MostraErro(ex.Message);
            }
        }

        private void BtConfirmar_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                CorrigirGt();
                DialogBox.MostraAviso("Grande Total foi atualizado com sucesso.");
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            
        }

        private void BtFechar_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtGerarSha1_OnClick(object sender, RoutedEventArgs e)
        {
            _corrigirGrandeTotalModel.GerarCodigoAtorizacao();
        }
    }
}
