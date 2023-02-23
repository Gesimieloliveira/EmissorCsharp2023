using System;
using System.Windows;
using System.Windows.Input;
using ACBrFramework;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.AdicionarImposto
{
    public partial class AdicionarImposto
    {
        private readonly AdicionaImpostoModel _adicionaImpostoModel;

        public AdicionarImposto()
        {
            InitializeComponent();
            _adicionaImpostoModel = new AdicionaImpostoModel();

            DataContext = _adicionaImpostoModel;
        }

        private void AdicionarImposto_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F2:

                    AdicionarAliquota();

                    break;
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private void AdicionarAliquota()
        {
            var messageBoxResult = DialogBox.MostraConfirmacao("Deseja adicionar alíquota? OBS: PROCESSO IRREVERSIVEL");

            if (messageBoxResult != MessageBoxResult.Yes) return;
            if (ValidarCampos()) return;

            try
            {

                _adicionaImpostoModel.ValidaRepetido();

                _adicionaImpostoModel.SalvarAliquotaNaEcf();

                _adicionaImpostoModel.TipoImposto = null;
                _adicionaImpostoModel.ValorAliquota = 0;

                DialogBox.MostraAviso("Alíquota adicionada com sucesso.");
            }
            catch (ACBrException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraErro(ex.Message);
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrEmpty(_adicionaImpostoModel.TipoImposto))
            {
                DialogBox.MostraInformacao("Selecione um tipo de alíquota.");
                return true;
            }

            if (_adicionaImpostoModel.ValorAliquota >= 1) return false;

            DialogBox.MostraInformacao("Digitar um valor a ser adicionado.");
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtConfirmar_OnClick(object sender, RoutedEventArgs e)
        {
            AdicionarAliquota();
        }
    }
}
