using FusionCore.FusionAdm.CteEletronico.Inutilizacao;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionAdm.Emissores
{
    public class EmissorFiscalCTE : IInutilizar
    {
        public byte EmissorFiscalId { get; set; }
        public TipoAmbiente Ambiente { get; set; } = TipoAmbiente.Producao;
        public byte[] ArquivoLogo { get; set; }
        public ModeloDocumento Modelo { get; set; } = ModeloDocumento.CTe;
        public int NumeroAtual { get; set; }
        public EmissorFiscal EmissorFiscal { get; set; }
        public short Serie { get; set; }


        public EstadoDTO Estado => EmissorFiscal.Empresa.EstadoDTO;
        public string Cnpj => EmissorFiscal.Empresa.Cnpj;
        public ModeloDocumento ModeloDocumento => ModeloDocumento.CTe;
        public short SerieInutilizar => EmissorFiscal.EmissorFiscalCte.Serie;
        public TipoAmbiente TipoAmbienteSefaz => Ambiente;
    }
}