using System;
using MDFe.Classes.Flags;

namespace FusionCore.FusionAdm.MdfeEletronico.Extencoes
{
    public static class ExtMdfeTipoEmitente
    {
        public static MDFeTipoEmitente ToZeusMFDe(this Flags.MDFeTipoEmitente fusionTipoEmitente)
        {
            switch (fusionTipoEmitente)
            {
                case Flags.MDFeTipoEmitente.PrestadorServicoDeTransporte:
                    return MDFeTipoEmitente.PrestadorServicoDeTransporte;
                case Flags.MDFeTipoEmitente.TransportadorDeCargaPropria:
                    return MDFeTipoEmitente.TransportadorCargaPropria;
            }

            throw new InvalidOperationException("Tipo Emitente Inválido");
        }
    }
}