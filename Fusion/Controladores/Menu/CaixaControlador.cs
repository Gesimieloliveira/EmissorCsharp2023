using System;
using Fusion.Visao.ControleCaixa;
using FusionCore.DI;
using FusionCore.Sessao;
using FusionWPF.SharedViews.ControleCaixa;
using MahApps.Metro.Controls;

namespace Fusion.Controladores.Menu
{
    public class CaixaControlador : Controlador
    {
        private readonly GridCaixasIndividuaisView _gridCaixasIndivuaisVisao;
        private readonly GridLancamentosCaixaControl _gridLancamentosCaixaControl;

        public CaixaControlador(MetroTabControl tabControl, ISessaoManager sessaoManager, IControleCaixaProvider caixaProvider) : base(tabControl)
        {

            var gridCaixasIndividuaisContexto = new GridCaixasIndividuaisContexto(sessaoManager, caixaProvider);
            _gridCaixasIndivuaisVisao = new GridCaixasIndividuaisView(gridCaixasIndividuaisContexto);
            _gridCaixasIndivuaisVisao.MexeuEmLancamento += CaixaIndividualVisaoMexeuLancamento;

            _gridLancamentosCaixaControl = new GridLancamentosCaixaControl(sessaoManager, caixaProvider);
            _gridLancamentosCaixaControl.MexeuEmLancamento += ContentOnMexeuEmLancamento;
        }

        private void CaixaIndividualVisaoMexeuLancamento(object sender, EventArgs e)
        {
            _gridLancamentosCaixaControl?.Contexto.ListarItems();
        }

        public void GradeLancamentoAvulsos()
        {
            AbrirJanelaEmAba("Lançamentos Caixa", _gridLancamentosCaixaControl);
        }

        private void ContentOnMexeuEmLancamento(object sender, EventArgs e)
        {
            _gridCaixasIndivuaisVisao?.CarregarDados();
        }

        public void GerenciarCaixaAberto()
        {
            AbrirJanelaEmAba("Gerenciar Caixa", _gridCaixasIndivuaisVisao);
        }
    }
}