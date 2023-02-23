using System;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Input;
using FusionWPF.Base.Utils.Dialogs;
using TextBox = System.Windows.Controls.TextBox;

namespace Fusion.Visao.TabelasPrecos
{
    public partial class TabelaPrecoFormulario
    {
        private readonly TabelaPrecoFormularioModel _model;

        public TabelaPrecoFormulario(TabelaPrecoFormularioModel model)
        {
            _model = model;
            InitializeComponent();
            DataContext = model;
        }

        private void AdicionarPrecoDiferenciadoItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.TabelaPrecoEstaSalva();

                var model = new NovoPrecoDiferenciadoItemFormularioModel(_model.ObterTabelaPreco());
                new NovoPrecoDiferenciadoItemFormulario(model).ShowDialog();

                _model.TabelaPreco = model.ObterTabelaPreco();

                _model.AtualizaListagem();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void Salvar_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.Salvar();

                DialogBox.MostraInformacao("Tabela de preço salva com sucesso!");
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void ClickExcluir_OnClick(object sender, RoutedEventArgs e)
        {
            _model.ExcluirAjusteDiferenciado();
        }

        private void TextPesquisar_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            var textBox = e.OriginalSource as TextBox;

            var textoParaPesquisa = textBox.Text;

            _model.PesquisarProdutos(textoParaPesquisa);
        }

        private void ManipularUmAjuste(object sender, MouseButtonEventArgs e)
        {
            if (_model.AjusteDiferenciadoSelecionada == null) return;

            try
            {
                _model.TabelaPrecoEstaSalva();

                var model = new NovoPrecoDiferenciadoItemFormularioModel(_model.ObterTabelaPreco(), _model.AjusteDiferenciadoSelecionada);
                new NovoPrecoDiferenciadoItemFormulario(model).ShowDialog();

                _model.TabelaPreco = model.ObterTabelaPreco();

                _model.AtualizaListagem();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }
    }
}
