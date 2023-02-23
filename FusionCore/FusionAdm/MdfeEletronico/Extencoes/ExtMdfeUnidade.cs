using System;
using FusionCore.FusionAdm.MdfeEletronico.Flags;
using MDFe.Classes.Flags;

namespace FusionCore.FusionAdm.MdfeEletronico.Extencoes
{
    public static class ExtMdfeUnidade
    {
        public static MDFeCUnid ToZeusMdfe(this MDFeUnidadeMedida unidadeMedida)
        {
            switch (unidadeMedida)
            {
                case MDFeUnidadeMedida.KG:
                    return MDFeCUnid.KG;
                case MDFeUnidadeMedida.TON:
                    return MDFeCUnid.TON;
            }

            throw new InvalidOperationException("Unidade Medida Inválida");
        }
    }
}