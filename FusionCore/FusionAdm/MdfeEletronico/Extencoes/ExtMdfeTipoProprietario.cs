using System;
using FusionCore.FusionAdm.CteEletronico.Flags;
using MDFe.Classes.Flags;

namespace FusionCore.FusionAdm.MdfeEletronico.Extencoes
{
    public static class ExtMdfeTipoProprietario
    {
        public static MDFeTpProp ToZeusMdfe(this TipoProprietario proprietario)
        {
            switch (proprietario)
            {
                case TipoProprietario.Outros:
                    return MDFeTpProp.Outros;
                case TipoProprietario.TacAgregado:
                    return MDFeTpProp.TacAgregado;
                case TipoProprietario.TacIndependente:
                    return MDFeTpProp.TacIndependente;
            }

            throw new InvalidOperationException("Tipo proprietario inválido");
        }
    }
}