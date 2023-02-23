using System;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using MDFe.Classes.Flags;

namespace FusionCore.FusionAdm.MdfeEletronico.Extencoes
{
    public static class ExtMdfeTipoDoTransportador
    {
        public static MDFeTpTransp ToZeusMdfe(this MDFeTipoDoTransportador tipoDoTransportador)
        {
            switch (tipoDoTransportador)
            {
                case MDFeTipoDoTransportador.ETC:
                    return MDFeTpTransp.ETC;
                case MDFeTipoDoTransportador.TAC:
                    return MDFeTpTransp.TAC;
                case MDFeTipoDoTransportador.CTC:
                    return MDFeTpTransp.CTC;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tipoDoTransportador), tipoDoTransportador, null);
            }
        }
    }
}