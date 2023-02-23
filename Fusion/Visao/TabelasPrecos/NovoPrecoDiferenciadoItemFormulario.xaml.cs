using System;
using System.Windows;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Factories;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.TabelasPrecos
{
    public partial class NovoPrecoDiferenciadoItemFormulario
    {
        private readonly NovoPrecoDiferenciadoItemFormularioModel _model;

        public NovoPrecoDiferenciadoItemFormulario(NovoPrecoDiferenciadoItemFormularioModel model)
        {
            _model = model;
            InitializeComponent();
            DataContext = _model;
        }

        private void Adicionar_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.SalvarPrecoDiferenciado();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private async void Calculadora_OnClick(object sender, RoutedEventArgs e)
        {
            if (_model.ProdutoDTO == null)
            {
                DialogBox.MostraAviso("Selecione um produto antes de usar a Calculadora.");
                return;
            }

            var calculadoraTela = new CalculadoraAjusteItem();
            var contexto = new CalculadoraAjusteItemContexto(_model.DescricaoTabelaPreco,
                _model.ProdutoDTO.PrecoVenda,
                _model.ObterTabelaPreco().TipoAjustePreco);

            var child = ChildWindowFactory.Cria(calculadoraTela, contexto);

            contexto.HandlerPorcentagemCalculada += ReceberPorcentagemAcao;
            contexto.Fechar += delegate
            {
                child.Close();
            };

            

            await this.ShowChildWindowAsync(child);
        }

        private void ReceberPorcentagemAcao(object sender, Porcentagem e)
        {
            _model.PercentualAjuste = e.Valor;
        }
    }
}
