using System.IO;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionServico;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Configuracao.Model
{
    public class ConfiguracaoServicoModel : AutoSaveModel
    {
        private ConfiguracaoExportacao _configuracao;

        public string DiretorioExportacao
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public bool ExportacaoAtiva
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        protected override void OnInicializa()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                _configuracao = FusionServicoFacade.CarregaConfiguraaco(sessao);
            }

            DiretorioExportacao = _configuracao.DiretorioExportacao;
            ExportacaoAtiva = _configuracao.ExportacaoAtiva;
        }

        protected override void OnSalvaAlteracoes()
        {
            if (!Directory.Exists(DiretorioExportacao))
            {
                return;
            }

            _configuracao.ExportacaoAtiva = ExportacaoAtiva;
            _configuracao.DiretorioExportacao = DiretorioExportacao;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioFusionServico(sessao);
                repositorio.Update(_configuracao);
            }
        }
    }
}