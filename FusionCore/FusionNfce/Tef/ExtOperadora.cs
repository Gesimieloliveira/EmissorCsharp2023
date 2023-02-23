using System;

namespace FusionCore.FusionNfce.Tef
{
    public static class ExtOperadora
    {
        public static global::Tef.Dominio.Enums.Operadora ToAcTef(this Operadora operadora)
        {
            switch (operadora)
            {
                case Operadora.PayGo:
                    return global::Tef.Dominio.Enums.Operadora.PayGo;
                case Operadora.TefExpress:
                    return global::Tef.Dominio.Enums.Operadora.TefExpress;
                case Operadora.Cappta:
                    return global::Tef.Dominio.Enums.Operadora.Cappta;
                case Operadora.TefDialHomologacao:
                    return global::Tef.Dominio.Enums.Operadora.TefDialHomologacao;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operadora), operadora, null);
            }
        }
    }
}