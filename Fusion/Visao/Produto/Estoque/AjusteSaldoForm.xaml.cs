using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.Controls;

namespace Fusion.Visao.Produto.Estoque
{
    public partial class AjusteSaldoForm : MetroWindow
    {
        private readonly AjusteSaldoFormModel _formModel;

        public AjusteSaldoForm(AjusteSaldoFormModel formModel)
        {
            DataContext = formModel;
            _formModel = formModel;
            InitializeComponent();
        }

        private void OnLoadedForm(object sender, RoutedEventArgs e)
        {
            try
            {
                _formModel.PreencherForm();
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void OnClickFechar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                _formModel.SalvarForm();
                DialogBox.MostraInformacao("Salvo com sucesso");
                Close();
            }
            catch (Exception ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}