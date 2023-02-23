using FusionCore.Configuracoes;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Configuracao.Model
{
    public class ConfiguracaoControleCaixaModel : AutoSaveModel
    {
        private readonly ISessaoManager _sessaoManager = new SessaoManagerAdm();
        private ConfiguracaoControleDeCaixa _configuracao;

        public bool ControlaCaixaNoGestor
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool ControlaCaixaNoNfce
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        protected override void OnInicializa()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var rep = new RepositorioConfiguracaoCaixa(sessao);

                _configuracao = rep.ObterUnica();

                ControlaCaixaNoGestor = _configuracao.ControlaCaixaNoGestor;
                ControlaCaixaNoNfce = _configuracao.ControlaCaixaNoNfce;
            }
        }

        protected override void OnSalvaAlteracoes()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                _configuracao.ControlaCaixaNoNfce = ControlaCaixaNoNfce;
                _configuracao.ControlaCaixaNoGestor = ControlaCaixaNoGestor;

                new RepositorioConfiguracaoCaixa(sessao).Alterar(_configuracao);
            }
        }
    }
}