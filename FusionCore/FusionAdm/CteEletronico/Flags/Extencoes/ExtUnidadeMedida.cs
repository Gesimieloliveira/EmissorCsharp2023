using System;
using FusionCore.DFe.XmlCte;

namespace FusionCore.FusionAdm.CteEletronico.Flags.Extencoes
{
    public static class ExtUnidadeMedida
    {
        public static FusionUnidadeMedidaCTe ToXml(this UnidadeMedida medida)
        {
            switch (medida)
            {
                case UnidadeMedida.M3:
                    return FusionUnidadeMedidaCTe.M3;
                case UnidadeMedida.Kg:
                    return FusionUnidadeMedidaCTe.Kg;
                case UnidadeMedida.Ton:
                    return FusionUnidadeMedidaCTe.Ton;
                case UnidadeMedida.Unidade:
                    return FusionUnidadeMedidaCTe.Unidade;
                case UnidadeMedida.Litros:
                    return FusionUnidadeMedidaCTe.Litros;
                case UnidadeMedida.Mmbtu:
                    return FusionUnidadeMedidaCTe.Mmbtu;
            }

            throw new ArgumentException("Unidade medida inválida");
        }
    }
}