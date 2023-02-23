using System;
using FusionCore.FusionAdm.CteEletronico.Flags;
using MDFe.Classes.Flags;

namespace FusionCore.FusionAdm.MdfeEletronico.Extencoes
{
    public static class ExtMdfeTipoRodado
    {
        public static MDFeTpRod ToZeusMdfe(this TipoRodado tipoRodado)
        {
            switch (tipoRodado)
            {
                case TipoRodado.CavaloMecanico:
                    return MDFeTpRod.CavaloMecanico;

                case TipoRodado.Outros:
                    return MDFeTpRod.Outros;

                case TipoRodado.Toco:
                    return MDFeTpRod.Toco;

                case TipoRodado.Truck:
                    return MDFeTpRod.Truck;

                case TipoRodado.Utilitario:
                    return MDFeTpRod.Utilitario;

                case TipoRodado.Van:
                    return MDFeTpRod.VAN;
            }

            throw new InvalidOperationException("Tipo rodado para mdf-e inválido");
        }
    }
}