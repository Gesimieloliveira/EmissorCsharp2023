using System;
using OpenAC.Net.Sat;

namespace FusionNfce.AutorizacaoSatFiscal.Configuracao.Configuracao
{
    public enum ModeloSatFusion
    {
        Cdecl,
        StdCall
    }

    public static class ExtModeloSatFusion
    {
        public static ModeloSat ToSat(this ModeloSatFusion modeloSatFusion)
        {
            switch (modeloSatFusion)
            {
                case ModeloSatFusion.Cdecl:
                    return ModeloSat.Cdecl;
                case ModeloSatFusion.StdCall:
                    return ModeloSat.StdCall;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modeloSatFusion), modeloSatFusion, null);
            }
        }
    }
}