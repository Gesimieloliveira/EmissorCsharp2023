using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Configuracao.Model
{
    public class ConfiguracaoFinanceiroModel : AutoSaveModel
    {
        private ConfiguracaoFinanceiro _configuracao;

        public decimal TaxaDeJurosMensal
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        protected override void OnInicializa()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _configuracao = new RepositorioConfiguracaoFinanceiro(sessao).BuscarUnico();
            }

            if (_configuracao == null)
            {
                _configuracao = new ConfiguracaoFinanceiro();
            }

            TaxaDeJurosMensal = _configuracao.TaxaDeJurosMensal;
        }

        protected override void OnSalvaAlteracoes()
        {
            _configuracao.TaxaDeJurosMensal = TaxaDeJurosMensal;
            _configuracao.ImprimirComprovanteCrediario = false;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioConfiguracaoFinanceiro(sessao);

                repositorio.Salvar(_configuracao);
            }
        }
    }
}