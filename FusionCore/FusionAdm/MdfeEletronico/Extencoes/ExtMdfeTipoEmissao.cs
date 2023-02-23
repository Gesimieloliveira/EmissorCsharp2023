using System;
using MDFe.Classes.Flags;

namespace FusionCore.FusionAdm.MdfeEletronico.Extencoes
{
    public static class ExtMdfeTipoEmissao
    {
        public static MDFeTipoEmissao ToZeusMdfe(this Flags.MDFeTipoEmissao fusionTipoEmissao)
        {
            switch (fusionTipoEmissao)
            {
                case Flags.MDFeTipoEmissao.Normal:
                    return MDFeTipoEmissao.Normal;
            }

            throw new InvalidOperationException("Tipo emissão inválida");
        }
    }
}