using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using FusionPdv.Servicos.ValidacaoInicial;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.AdicionarImposto
{
    public partial class Imposto
    {
        private readonly ImpostoModel _impostoModel;

        public Imposto()
        {
            InitializeComponent();
            _impostoModel = new ImpostoModel();
            DataContext = _impostoModel;
        }

        private void Imposto_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:
                    AtualizarListaDeAliquota();
                    
                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private void BtAdicionarAliquota_OnClick(object sender, RoutedEventArgs e)
        {
            AtualizarListaDeAliquota();
        }

        private void BtFechar_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AtualizarListaDeAliquota()
        {
            try
            {
                new AdicionarImposto().ShowDialog();
                _impostoModel.AtualizaListaAliquota();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraErro(ex.Message);
            }
        }

        private void Imposto_OnClosing(object sender, CancelEventArgs e)
        {
            try
            {
                _impostoModel.ValidarSeExisteImposto();
            }
            catch (ExceptionExisteAliquota ex)
            {
                e.Cancel = true;
                MessageBox.Show(ex.Message);
            }
        }
    }
}
