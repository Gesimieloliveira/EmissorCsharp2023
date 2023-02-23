using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fusion.Visao.Empresa.ConsultaNaSefaz;
using Fusion.Visao.Pessoa.SubFormularios;
using FusionCore.FusionAdm.Pessoas;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Factories;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.Pessoa
{
    public partial class PessoaForm
    {
        private ChildWindow _childInicio;

        public PessoaForm(PessoaFormModel viewModel)
        {
            DataContext = viewModel;
            GetModel.CloseRequest += (s, e) => { Close(); };

            InitializeComponent();
        }

        private PessoaFormModel GetModel => (PessoaFormModel) DataContext;

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {

            if (_childInicio == null && GetModel.IsNovo)
            {
                var vm = new PessoaFormInicioModel();

                BotaoSalvar.Content = "Salvar Inclusão";

                vm.ConsultaCnpj += ConsultaCnpjHandler;
                vm.PessoaJuridica += IniciaComoPessoaJuridica;
                vm.PessoaFisica += IniciaCompPessoaFisica;

                _childInicio = ChildWindowFactory.Cria(new PessoaFormInicio(), vm);
                await this.ShowChildWindowAsync(_childInicio, ChildWindowManager.OverlayFillBehavior.FullWindow);
                return;
            }

            GetModel.Inicializar();

            TextBoxNome?.Focus();
        }

        private async void ConsultaCnpjHandler(object sender, EventArgs e)
        {
            try
            {
                var vm = new BuscarEmpresaNaSefazModel();

                vm.EmpresaEncontrada += (s, emp) =>
                {
                    GetModel.CarregaComEmpresaReceita(emp);
                    TextBoxNome?.Focus();
                };

                var child = new BuscarEmpresaNaSefazForm(vm);

                await this.ShowChildWindowAsync(child, ChildWindowManager.OverlayFillBehavior.FullWindow);
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void IniciaComoPessoaJuridica(object sender, EventArgs e)
        {
            GetModel.Tipo = PessoaTipo.Juridica;
            TextBoxNome?.Focus();
        }

        private void IniciaCompPessoaFisica(object sender, EventArgs e)
        {
            GetModel.Tipo = PessoaTipo.Fisica;
            TextBoxNome?.Focus();
        }

        private void OnClickSalvar(object sender, RoutedEventArgs e)
        {
            GetModel.Salvar();
        }

        private void OnClickFechar(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditarTelefoneHandler(object sender, MouseButtonEventArgs e)
        {
            var janela = new TelefoneForm(GetModel.GetTelefoneModelEdicao());
            janela.ShowDialog();
        }

        private void AdicionarTelefoneHandler(object sender, RoutedEventArgs e)
        {
            var janela = new TelefoneForm(GetModel.GetTelefoneModelNovo());
            janela.ShowDialog();
        }

        private void ChangeTipoHandler(object sender, SelectionChangedEventArgs e)
        {
            if (TabControlTipo == null)
                return;

            switch (GetModel?.Tipo)
            {
                case PessoaTipo.Extrangeiro:
                    TabControlTipo.SelectedIndex = 2;
                    break;
                case PessoaTipo.Fisica:
                    TabControlTipo.SelectedIndex = 0;
                    break;
                case PessoaTipo.Juridica:
                    TabControlTipo.SelectedIndex = 1;
                    break;
            }
        }

        private void ExcluirTelefoneClickHandler(object sender, RoutedEventArgs e)
        {
            GetModel.TelefoneSelecionado = (PessoaTelefone)((Button)sender).Tag;

            if (!DialogBox.MostraDialogoDeConfirmacao("Quer mesmo excluir este telefone?"))
            {
                return;
            }

            try
            {
                GetModel.DeletarTelefoneSelecionado();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }
    }
}