using DFe.Utils;
using FusionCore.Core.Net;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Builders
{
    public interface IDadosServicoSefaz
    {
        ModeloDocumento Modelo { get; }
        TipoAmbiente Ambiente { get; }
        byte IbgeEstadoEmissao { get; }
        ConfiguracaoCertificado Certificado { get; }
        ProtocoloSeguranca ProtocoloSeguranca { get; }
    }
}