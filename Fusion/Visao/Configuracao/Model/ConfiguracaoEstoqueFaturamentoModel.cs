using FusionCore.Configuracoes;
using FusionCore.FusionAdm.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Configuracao.Model
{
    public class ConfiguracaoEstoqueFaturamentoModel : AutoSaveModel
    {
        private ConfiguracaoEstoqueFaturamento _configuracaoEstoqueFaturamento;
        private bool _movimentarEstoqueFaturamento = true;

        public bool MovimentarEstoqueFaturamento
        {
            get => _movimentarEstoqueFaturamento;
            set
            {
                _movimentarEstoqueFaturamento = value;
                PropriedadeAlterada();
            }
        }

        protected override void OnInicializa()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioMovimentaEstoqueFaturamento = new RepositorioMovimentaEstoqueFaturamento(sessao);
                _configuracaoEstoqueFaturamento = repositorioMovimentaEstoqueFaturamento.ObterConfiguracaoEstoqueFaturamento();
            }

            AtualizarTela();
        }

        private void AtualizarTela()
        {
            MovimentarEstoqueFaturamento = _configuracaoEstoqueFaturamento.MovimentarEstoque;
        }

        protected override void OnSalvaAlteracoes()
        {
            _configuracaoEstoqueFaturamento.MovimentarEstoque = MovimentarEstoqueFaturamento;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorioMovimentaEstoqueFaturamento = new RepositorioMovimentaEstoqueFaturamento(sessao);
                repositorioMovimentaEstoqueFaturamento.SalvarOuAtualizar(_configuracaoEstoqueFaturamento);
            }
        }
    }
}