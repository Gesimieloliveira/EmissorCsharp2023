using FusionCore.FusionAdm.CteEletronico.Inutilizacao;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Emissores
{
    public class EmissorFiscalCTeOS : IInutilizar
    {
        public byte EmissorFiscalId { get; set; }
        public TipoAmbiente Ambiente { get; set; } = TipoAmbiente.Producao;
        public byte[] ArquivoLogo { get; set; }
        public ModeloDocumento Modelo { get; set; } = ModeloDocumento.CTeOS;
        public short Serie { get; set; }
        public int NumeroAtual { get; set; }
        public EmissorFiscal EmissorFiscal { get; set; }


        public EstadoDTO Estado => EmissorFiscal.Empresa.EstadoDTO;
        public string Cnpj => EmissorFiscal.Empresa.Cnpj;
        public ModeloDocumento ModeloDocumento => ModeloDocumento.CTeOS;
        public short SerieInutilizar => Serie;
        public TipoAmbiente TipoAmbienteSefaz => Ambiente;
    }
}