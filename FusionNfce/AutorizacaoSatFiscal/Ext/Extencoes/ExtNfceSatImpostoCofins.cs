using System;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Tributacoes.Flags;
using OpenAC.Net.Sat;
using OpenAC.Net.Sat.Interfaces;

namespace FusionNfce.AutorizacaoSatFiscal.Ext.Extencoes
{
    public static class ExtNfceSatImpostoCofins
    {
        public static ICFeCofins GetCofinsSat(this NfceImpostoCofins impostoCofins, RegimeTributario regimeTributario)
        {
            if (regimeTributario == RegimeTributario.SimplesNacional)
            {
                return new ImpostoCofinsSn { Cst = "49" };
            }

            switch (impostoCofins.Cofins.Id)
            {
                case "01":
                case "02":
                case "05":
                    return new ImpostoCofinsAliq
                    {
                        Cst = impostoCofins.Cofins.Id,
                        VBc = impostoCofins.BaseCalculo,
                        PCofins = decimal.Round(impostoCofins.Aliquota / 100, 4)
                    };
                case "04":
                case "06":
                case "07":
                case "08":
                case "09":
                    return new ImpostoCofinsNt
                    {
                        Cst = impostoCofins.Cofins.Id
                    };
            }

            throw new InvalidOperationException($"COFINS {impostoCofins.Cofins.Id} não disponível para Regime Normal com SAT");
        }
    }
}