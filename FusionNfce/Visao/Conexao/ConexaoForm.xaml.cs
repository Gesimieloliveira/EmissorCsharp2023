using System;
using System.Windows;
using System.Windows.Input;
using FusionLibrary.Execoes;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Conexao
{
    public partial class ConexaoForm
    {
        private readonly ConexaoFormModel _formModel;

        public ConexaoForm()
        {
            _formModel = new ConexaoFormModel();
            DataContext = _formModel;
            InitializeComponent();
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            try
            {
                _formModel.Salvar();
                Close();
            }
            catch (ViewModelErrorsException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
            catch (Exception ex)
            {
                DialogBox.MostraErro("Falha ao salvar informações", ex);
            }
        }

        private void ConexaoForm_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
            }
        }

        private void OnClickTestarConexaoServidor(object sender, RoutedEventArgs e)
        {
            try
            {
                _formModel.TestaConexaoServidor();
                DialogBox.MostraInformacao("Conexão com o banco de dados no servidor realizada com sucesso");
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}