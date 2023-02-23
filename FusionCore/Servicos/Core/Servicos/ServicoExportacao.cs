using FusionCore.FusionServico;
using FusionCore.Servicos.Core.Exportacao;
using FusionCore.Servicos.Core.Exportacao.Estrategias;
using FusionCore.Sessao;

namespace FusionCore.Servicos.Core.Servicos
{
    public class ServicoExportacao
    {
        private readonly ISessaoManager _sessaoManager;
        private ConfiguracaoExportacao _configuracao;

        public ServicoExportacao(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public void FazerExportacoes()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                _configuracao = FusionServicoFacade.CarregaConfiguraaco(sessao);
            }

            if (!_configuracao.ExportacaoAtiva)
            {
                return;
            }

            var exportador = new Exportador(_sessaoManager);

            exportador.Exportar(new ExportacaoNfe(), _configuracao);
            exportador.Exportar(new ExportacaoNfce(), _configuracao);
        }
    }
}