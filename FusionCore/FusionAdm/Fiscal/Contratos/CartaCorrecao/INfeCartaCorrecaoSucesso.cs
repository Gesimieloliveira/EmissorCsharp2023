using System;

namespace FusionCore.FusionAdm.Fiscal.Contratos.CartaCorrecao
{
    public interface INfeCartaCorrecaoSucesso
    {
        DateTime? OcorreuEm { get; set; }
        string Protocolo { get; set; }
        int StatusRetorno { get; set; }
        int TipoEvento { get; set; }
        byte SequenciaEvento { get; set; }
    }
}