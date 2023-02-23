using System;
using System.Windows;
using System.Windows.Controls;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Ecf
{
    public partial class EcfForm
    {
        private readonly EcfFormModel _formModel;

        public EcfForm(EcfFormModel formModel)
        {
            _formModel = formModel;
            DataContext = _formModel;
            InitializeComponent();
        }

        private void ClickBotaoFechar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClickBotaoSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                _formModel.SalvarModel();
                DialogBox.MostraInformacao("Emissor salvo com sucesso");
                Close();
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            _formModel.Validation_Error(sender, e);
        }
    }
}