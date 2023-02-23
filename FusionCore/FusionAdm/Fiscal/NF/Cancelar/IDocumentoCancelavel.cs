using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Fiscal.NF.Cancelar
{
    public interface IDocumentoCancelavel
    {
        int ReferenciaId { get; }
        int NumeroDocumento { get; } 
        string NumeroProtocolo { get; }
        string NumeroChave { get; }
        string CnpjCpfEmitente { get; }
        TipoEmissao TipoEmissaoCancelamento();
        string ObterXmlAutorizado();
    }
}