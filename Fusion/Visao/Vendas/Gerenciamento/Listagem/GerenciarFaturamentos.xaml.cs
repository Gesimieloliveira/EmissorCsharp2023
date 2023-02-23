using System;
using System.Windows;
using System.Windows.Input;
using Fusion.Helpers;
using Fusion.Visao.Vendas.Gerenciamento.Opcoes;
using FusionCore.Sessao;

namespace Fusion.Visao.Vendas.Gerenciamento.Listagem
{
    public partial class GerenciarFaturamentos
    {
        private readonly Action _atualizarListagens;

        public GerenciarFaturamentos(Action atualizarListagens)
        {
            _atualizarListagens = atualizarListagens;
            InitializeComponent();
            FiltroHelper.RegitrarAtalhoFiltro(PainelFiltro, BotaoFiltro);
        }

        private GerenciarFaturamentosContexto Contexto => DataContext as GerenciarFaturamentosContexto;

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            Contexto.Inicializar();
        }

        private void AplicarFiltroHandler(object sender, RoutedEventArgs e)
        {
            Contexto.AplicaFiltro();
        }

        private void DoubleClickRowHandler(object sender, MouseButtonEventArgs e)
        {
            var opcao = new FaturamentoOpcaoContexto(
                Contexto.Selecionado,
                new SessaoManagerAdm()
            );

            opcao.CompletouAcao += (o, slim) => { Contexto.AplicaFiltro(); };
            opcao.ShowDialog<FaturamentoOpcaoView>();

            Contexto.AplicaFiltro();
            _atualizarListagens.Invoke();
        }
    }
}
