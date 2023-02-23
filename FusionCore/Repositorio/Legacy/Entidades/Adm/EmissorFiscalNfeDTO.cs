using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.Repositorio.Legacy.Contratos.Entidades;
using FusionLibrary.Helper.Criptografia;
using NFe.Utils;

namespace FusionCore.Repositorio.Legacy.Entidades.Adm
{
    public class EmissorFiscalNfeDTO : IEntidade
    {
        public byte EmissorFiscalId { get; set; }
        public TipoAmbiente Ambiente { get; set; } = TipoAmbiente.Producao;
        public byte[] ArquivoLogo { get; set; }
        public ModeloDocumento Modelo { get; set; } = ModeloDocumento.NFe;
        public short Serie { get; set; }
        public int NumeroAtual { get; set; }
        public EmissorFiscalDTO EmissorFiscal { get; set; }
    }
}