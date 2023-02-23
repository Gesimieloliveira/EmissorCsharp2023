using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using ZeusTipoAmbiente = DFe.Classes.Flags.TipoAmbiente;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class TipoAmbienteExt
    {
        public static ZeusTipoAmbiente ToZeus(this TipoAmbiente tipo)
        {
            switch (tipo)
            {
                case TipoAmbiente.Homologacao:
                    return ZeusTipoAmbiente.Homologacao;
                case TipoAmbiente.Producao:
                    return ZeusTipoAmbiente.Producao;
            }

            throw new InvalidCastException("TipoAmbiente para zeus é inválido");
        }
    }
}