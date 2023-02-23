using FusionCore.Configuracoes;
using FusionCore.FusionAdm.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Configuracao.Model
{
    public class ConfiguracaoEstoqueModel : AutoSaveModel
    {
        private ConfiguracaoEstoque _configuracao;
        private bool _bloqueiaEstoqueNegativo;

        public bool BloqueiaEstoqueNegativo
        {
            get => _bloqueiaEstoqueNegativo;
            set
            {
                _bloqueiaEstoqueNegativo = value;
                PropriedadeAlterada();
            }
        }

        protected override void OnInicializa()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioConfiguracaoEstoque(sessao);
                _configuracao = repositorio.GetConfiguracaoUnica();
            }

            RefreshControls();
        }

        private void RefreshControls()
        {
            if (_configuracao == null)
            {
                return;
            }

            BloqueiaEstoqueNegativo = _configuracao.BloqueiaEstoqueNegativo;
        }

        protected override void OnSalvaAlteracoes()
        {
            _configuracao.BloqueiaEstoqueNegativo = BloqueiaEstoqueNegativo;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioConfiguracaoEstoque(sessao);
                repositorio.Update(_configuracao);
            }
        }
    }
}