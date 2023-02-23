using System;
using FusionCore.Tributacoes.Flags;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class RegimeTributarioExt
    {
        public static NFe.Classes.Informacoes.Emitente.CRT ToZeus(this RegimeTributario regime)
        {
            switch (regime)
            {
                case RegimeTributario.SimplesNacional:
                    return NFe.Classes.Informacoes.Emitente.CRT.SimplesNacional;
                case RegimeTributario.SimplesNacionalExcesso:
                    return NFe.Classes.Informacoes.Emitente.CRT.SimplesNacionalExcessoSublimite;
                case RegimeTributario.RegimeNormal:
                    return NFe.Classes.Informacoes.Emitente.CRT.RegimeNormal;
            }

            throw new InvalidCastException("Regime Tributario para Zeus Inválida");
        }
    }
}