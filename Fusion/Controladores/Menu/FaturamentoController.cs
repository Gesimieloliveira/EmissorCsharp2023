using System;
using System.Windows;
using Fusion.Visao.ControlarNfces;
using Fusion.Visao.Vendas.FaturamentoCheckout;
using Fusion.Visao.Vendas.Gerenciamento.Listagem;
using MahApps.Metro.Controls;

namespace Fusion.Controladores.Menu
{
    public class FaturamentoController : Controlador
    {
        private GerenciarFaturamentosContexto _gerenciarFaturamentosContexto;
        private ListarTodasNfcesDados _listarDadosNfce;

        public FaturamentoController(MetroTabControl tabControl) : base(tabControl)
        {
        }

        public void AbrirFaturamento()
        {
            FaturamentoCheckout.Factory.CurrentView.ShowView();
        }

        public void AbirListagemFaturamentos()
        {
            _gerenciarFaturamentosContexto = new GerenciarFaturamentosContexto(SessaoManager);

            var content = new GerenciarFaturamentos(AtualizarListagens());
            AbrirJanelaEmAba("Faturamentos", content, _gerenciarFaturamentosContexto);
        }

        public void AbrirListagemNfce()
        {
            _listarDadosNfce = new ListarTodasNfcesDados();

            AbrirJanelaEmAba("Gerenciar NFC-e", new ListarTodasNfces(_listarDadosNfce, AtualizarListagens()));
        }

        public Action AtualizarListagens()
        {
            return () =>
            {
                _gerenciarFaturamentosContexto?.AplicaFiltro();
                _listarDadosNfce?.PesquisarNfces();
            };
        }
    }
}