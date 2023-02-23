using System;
using FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Tipos;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtImpostoCofinsZeus
    {
        public static COFINS ToZeus(this NfceImpostoCofins impostoCofins)
        {
            return new COFINS
            {
                TipoCOFINS = GetCofins(impostoCofins)
            };
        }

        private static COFINSBasico GetCofins(NfceImpostoCofins impostoCofins)
        {
            switch (impostoCofins.Cofins.Id)
            {
                case "01":
                case "02":
                    return GetCofinsAliquota(impostoCofins);
                case "03":
                    return GetCofinsQtde(impostoCofins);
                case "04":
                case "05":
                case "06":
                case "07":
                case "08":
                case "09":
                    return GetCofinsNaoTributado(impostoCofins);
                case "49":
                    return GetCofinsOutros(impostoCofins);
            }

            throw new InvalidOperationException($"COFINS inválido: {impostoCofins.Cofins}");
        }

        private static COFINSBasico GetCofinsOutros(NfceImpostoCofins impostoCofins)
        {
            return new COFINSOutr
            {
                CST = impostoCofins.Cofins.ToZeus(),
                pCOFINS = impostoCofins.Aliquota,
                vBC = impostoCofins.BaseCalculo,
                vCOFINS = impostoCofins.Valor
            };
        }

        private static COFINSBasico GetCofinsNaoTributado(NfceImpostoCofins impostoCofins)
        {
            return new COFINSNT
            {
                CST = impostoCofins.Cofins.ToZeus(),
            };
        }

        private static COFINSBasico GetCofinsQtde(NfceImpostoCofins impostoCofins)
        {
            throw new InvalidCastException("Cofins quantidade não disponível");
        }

        private static COFINSBasico GetCofinsAliquota(NfceImpostoCofins impostoCofins)
        {
            return new COFINSAliq
            {
                CST = impostoCofins.Cofins.ToZeus(),
                pCOFINS = impostoCofins.Aliquota,
                vBC = impostoCofins.BaseCalculo,
                vCOFINS = impostoCofins.Valor
            };
        }
    }
}