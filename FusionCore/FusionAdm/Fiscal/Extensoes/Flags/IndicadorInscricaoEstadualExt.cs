using System;
using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.Flags
{
    public static class IndicadorInscricaoEstadualExt
    {
        public static NFe.Classes.Informacoes.Destinatario.indIEDest ToZeus(this IndicadorIE indicador)
        {
            switch (indicador)
            {
                case IndicadorIE.ContribuinteIcms:
                    return NFe.Classes.Informacoes.Destinatario.indIEDest.ContribuinteICMS;
                case IndicadorIE.Isento:
                    return NFe.Classes.Informacoes.Destinatario.indIEDest.Isento;
                case IndicadorIE.NaoContribuinte:
                    return NFe.Classes.Informacoes.Destinatario.indIEDest.NaoContribuinte;
            }

            throw new InvalidCastException("Convers�o de tipo para Zeus Inv�lida");
        }
    }
}