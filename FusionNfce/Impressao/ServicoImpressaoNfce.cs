using Fusion.FastReport.Facades;
using Fusion.FastReport.Facades.Infra;
using FusionCore.FusionNfce.Preferencias;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Repositorio.FusionNfce;
using FusionCore.Sessao;

namespace FusionNfce.Impressao
{
    public class ServicoImpressaoNfce
    {
        private readonly int _nfceId;
        private readonly DanfeNfceFacade _danfeNfceFacade;

        public ServicoImpressaoNfce(int nfceId, DanfeNfceFacade danfeNfceFacade)
        {
            _nfceId = nfceId;
            _danfeNfceFacade = danfeNfceFacade;
        }

        public void Imprimir()
        {
            using (var sessao = new SessaoManagerNfce().CriaSessao())
            {
                var preferenciaTerminal = SessaoSistemaNfce.Preferencia;
                IDadosParaImpressaoNfce dadosParaImpressaoNfce = new RepositorioNfce(sessao).GetPeloId(_nfceId);
                var cfg = new RepositorioConfiguracaoFrenteCaixaNfce(sessao).BuscarUnico();

                var danfeNfceConfiguracao = new DanfeNfceConfiguracaoDto();
                danfeNfceConfiguracao.ImprimirComImpressora(preferenciaTerminal.NomeImpressora);
                danfeNfceConfiguracao.PreVisualizarImpressao(preferenciaTerminal.VisualizaAntesDeImprimir);
                danfeNfceConfiguracao.CustomizarNomeFantasia(preferenciaTerminal.NomeFantasiaCustomizado);
                danfeNfceConfiguracao.UsarPlanoDeImpressao(preferenciaTerminal.LayoutImpressao);
                danfeNfceConfiguracao.InterromperImpressao(preferenciaTerminal.NaoImprimir);
                danfeNfceConfiguracao.ImprimirSegundaVia(cfg.IsSegundaViaContingencia);
                danfeNfceConfiguracao.ImprimirAposFinalizacao();

                if (preferenciaTerminal.NaoImprimir)
                    danfeNfceConfiguracao.NaoImprimirAposFinalizacao();


                _danfeNfceFacade.Imprimir(
                    danfeNfceConfiguracao
                    , dadosParaImpressaoNfce
                    , new ServicoObterXml(new RepositorioDanfeNfce(sessao))
                );
            }
        }
    }
}