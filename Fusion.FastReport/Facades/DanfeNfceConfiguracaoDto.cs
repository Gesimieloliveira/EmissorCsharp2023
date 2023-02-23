using FusionCore.FusionNfce.Preferencias;

namespace Fusion.FastReport.Facades
{
    public class DanfeNfceConfiguracaoDto
    {
        public DanfeNfceConfiguracaoDto()
        {
        }

        public string Impressora { get; private set; }
        public bool PreVisualizar { get; private set; }
        public string NomeFantasiaCustomizado { get; private set; }
        public bool NaoImprimir { get; private set; }
        public LayoutImpressao PlanoDeImpressao { get; private set; }
        public bool IsSegundaViaContingencia { get; private set; }
        public bool SegundaViaForcada { get; private set; }
        public bool ImprimirFinalizacao { get; private set; }

        public void ImprimirComImpressora(string impressora)
        {
            Impressora = impressora;
        }

        public void SemImpressora()
        {
            Impressora = string.Empty;
        }

        public void PreVisualizarSomente()
        {
            PreVisualizar = true;
        }

        public void NaoVisualizarAntesDeImprimir()
        {
            PreVisualizar = false;
        }

        public void PreVisualizarImpressao(bool preVisualizar)
        {
            if (preVisualizar)
            {
                PreVisualizarSomente();
                return;
            }

            NaoVisualizarAntesDeImprimir();
        }

        public void CustomizarNomeFantasia(string nomeFantasiaCustomizado)
        {
            NomeFantasiaCustomizado = nomeFantasiaCustomizado;
        }

        public void UsarPlanoDeImpressao(LayoutImpressao layoutImpressao)
        {
            PlanoDeImpressao = layoutImpressao;
        }

        public void InterromperImpressao(bool naoImprimir)
        {
            NaoImprimir = naoImprimir;
        }

        public void ImprimirSegundaVia(bool segundaVia)
        {
            IsSegundaViaContingencia = segundaVia;
        }

        public void SemNomeFantasiaCustomizado()
        {
            NomeFantasiaCustomizado = string.Empty;
        }

        public void ForcarSegundaVia(bool imprimeDuasVias)
        { 
            SegundaViaForcada = imprimeDuasVias;
        }

        public void ImprimirAposFinalizacao()
        {
            ImprimirFinalizacao = true;
        }

        public void NaoImprimirAposFinalizacao()
        {
            ImprimirFinalizacao = false;
        }
    }
}