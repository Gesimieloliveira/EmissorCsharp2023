using FusionCore.FusionAdm.Emissores.Flags;

namespace FusionNfce.Visao.Configuracao.Impressao
{
    public class ConfiguracaoImpressora
    {
        public ModeloMiniImpressora ModeloMiniImpressora { get; set; }
        public CodificacaoArquivoXml Codificacao { get; set; }
        public string Porta { get; set; }
        public bool Ativo { get; set; }
    }
}