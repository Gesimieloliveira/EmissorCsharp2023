using System;
using FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Tipos;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores
{
    public static class ConversorCofinsParaZeus
    {
        public static COFINS ToZeus(this ImpostoCofins cofins)
        {
            return new COFINS
            {
                TipoCOFINS = GetCofins(cofins)
            };
        }

        private static COFINSBasico GetCofins(ImpostoCofins cofins)
        {
            switch (cofins.Cst.Id)
            {
                case "01":
                case "02":
                    return GetCofinsAliquota(cofins);
                case "03":
                    return GetCofinsQuantidade(cofins);
                case "04":
                case "05":
                case "06":
                case "07":
                case "08":
                case "09":
                    return GetCofinsNaoTributado(cofins);
                case "49":
                case "50":
                case "51":
                case "52":
                case "53":
                case "54":
                case "55":
                case "56":
                case "60":
                case "61":
                case "62":
                case "63":
                case "64":
                case "65":
                case "66":
                case "67":
                case "70":
                case "71":
                case "72":
                case "73":
                case "74":
                case "75":
                case "99":
                case "98":
                    return GetCofinsOutros(cofins);
            }

            throw new InvalidCastException("Erro de conversão do COFINS para Cofins Zeus");
        }

        private static COFINSBasico GetCofinsQuantidade(ImpostoCofins cofins)
        {
            throw new InvalidCastException("COFINS quantidade não disponível");
        }

        private static COFINSBasico GetCofinsAliquota(ImpostoCofins cofins)
        {
            return new COFINSAliq
            {
                CST = cofins.Cst.ToZeus(),
                vBC = cofins.ValorBcCofins,
                pCOFINS = cofins.AliquotaCofins,
                vCOFINS = cofins.ValorCofins
            };
        }

        private static COFINSBasico GetCofinsNaoTributado(ImpostoCofins cofins)
        {
            return new COFINSNT {CST = cofins.Cst.ToZeus()};
        }

        private static COFINSBasico GetCofinsOutros(ImpostoCofins cofins)
        {
            return new COFINSOutr
            {
                CST = cofins.Cst.ToZeus(),
                vBC = cofins.ValorBcCofins,
                vCOFINS = cofins.ValorCofins,
                pCOFINS = cofins.AliquotaCofins
            };
        }
    }
}
