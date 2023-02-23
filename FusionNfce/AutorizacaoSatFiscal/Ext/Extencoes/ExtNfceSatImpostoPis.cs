using System;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Tributacoes.Flags;
using OpenAC.Net.Sat;
using OpenAC.Net.Sat.Interfaces;

namespace FusionNfce.AutorizacaoSatFiscal.Ext.Extencoes
{
    public static class ExtNfceSatImpostoPis
    {
        public static ICFePis GetPisSat(this NfceImpostoPis impostoPis, RegimeTributario regimeTributario)
        {
            if (regimeTributario == RegimeTributario.SimplesNacional)
            {
                return new ImpostoPisSn {Cst = "49"};
            }

            switch (impostoPis.Pis.Id)
            {
                case "01":
                case "02":
                case "05":
                    return new ImpostoPisAliq
                    {
                        Cst = impostoPis.Pis.Id,
                        VBc = impostoPis.BaseCalculo,
                        PPis = decimal.Round(impostoPis.Aliquota / 100, 4)
                    };
                case "04":
                case "06":
                case "07":
                case "08":
                case "09":
                    return new ImpostoPisNt
                    {
                        Cst = impostoPis.Pis.Id,
                    };
            }

            throw new InvalidOperationException($"PIS {impostoPis.Pis.Id} não disponível para Regime Normal com SAT");
        }
    }
}