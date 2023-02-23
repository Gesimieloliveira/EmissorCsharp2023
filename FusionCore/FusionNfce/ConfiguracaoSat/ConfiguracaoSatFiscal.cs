using FusionCore.FusionAdm.Emissores.Flags;

namespace FusionCore.FusionNfce.ConfiguracaoSat
{
    public class ConfiguracaoSatFiscal
    {
        public int Id { get; set; }
        public bool Ativo { get; set; }
        public bool AssociadoAssinatura { get; set; }
        public string CodigoAtivacao { get; set; }
        public string CodigoAssociacao { get; set; }
        public Modelo FabricanteModelo { get; set; }
        public CodificacaoArquivoXml CodificacaoArquivoXml { get; set; }
    }
}