using System;
using FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Tipos;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores
{
    public static class ConversorPisParaZeus
    {
        public static PIS ToZeus(this ImpostoPis pis)
        {
            return new PIS
            {
                TipoPIS = GetPis(pis)
            };
        }

        private static PISBasico GetPis(ImpostoPis pis)
        {
            switch (pis.Cst.Id)
            {
                case "01":
                case "02":
                    return GetPisAliquota(pis);
                case "03":
                    return GetPisQtde(pis);
                case "04":
                case "05":
                case "06":
                case "07":
                case "08":
                case "09":
                    return GetPisNaoTributado(pis);
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
                    return GetPisOutros(pis);
            }

            throw new InvalidCastException("Erro de conversão do PIS para Pis Zeus");
        }

        private static PISBasico GetPisQtde(ImpostoPis pis)
        {
            throw new InvalidCastException("PIS quantidade não disponível");
        }

        private static PISBasico GetPisAliquota(ImpostoPis pis)
        {
            return new PISAliq
            {
                CST = pis.Cst.ToZeus(),
                pPIS = pis.AliquotaPis,
                vBC = pis.ValorBcPis,
                vPIS = pis.ValorPis
            };
        }

        private static PISBasico GetPisNaoTributado(ImpostoPis pis)
        {
            return new PISNT
            {
                CST = pis.Cst.ToZeus()
            };
        }

        private static PISBasico GetPisOutros(ImpostoPis pis)
        {
            return new PISOutr
            {
                CST = pis.Cst.ToZeus(),
                vBC = pis.ValorBcPis,
                vPIS = pis.ValorPis,
                pPIS = pis.AliquotaPis
            };
        }
    }
}