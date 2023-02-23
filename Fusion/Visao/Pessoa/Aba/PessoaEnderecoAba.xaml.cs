using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Visao.Pessoa.SubFormularios;
using FusionCore.FusionAdm.Pessoas;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Pessoa.Aba
{
    public partial class PessoaEnderecoAba
    {
        public PessoaEnderecoAba()
        {
            InitializeComponent();
        }

        public PessoaFormModel ViewModel => DataContext as PessoaFormModel;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void AdicionarEnderecoHandler(object sender, RoutedEventArgs e)
        {
            var model = ViewModel.GetEnderecoModelNovo();
            var janela = new EnderecoForm(model);
            janela.ShowDialog();
        }

        private void DoubleClickEditarEnderecoHandler(object sender, MouseButtonEventArgs e)
        {
            var model = ViewModel.GetEnderecoModelEdicao();
            var janela = new EnderecoForm(model);
            janela.ShowDialog();
        }

        private void ExcluirEnderecoClickHandler(object sender, RoutedEventArgs e)
        {
            if (!DialogBox.MostraDialogoDeConfirmacao("Quer mesmo excluir este endereço?"))
            {
                return;
            }

            try
            {
                var pessoaEndereco = ((Button) sender).Tag as PessoaEndereco;

                ViewModel.DeletarPessoaEndereco(pessoaEndereco);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}