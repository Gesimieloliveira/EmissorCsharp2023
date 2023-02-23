using System;
using FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Tipos;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtImpostoPisZeus
    {
        public static PIS ToZeus(this NfceImpostoPis impostoPis)
        {
            return new PIS
            {
                TipoPIS = GetPis(impostoPis)
            };
        }

        private static PISBasico GetPis(NfceImpostoPis impostoPis)
        {
            switch (impostoPis.Pis.Id)
            {
                case "01":
                case "02":
                    return GetPisAliquota(impostoPis);
                case "03":
                    return GetPisQtde(impostoPis);
                case "04": 
                case "05": 
                case "06": 
                case "07": 
                case "08": 
                case "09": 
                    return GetPisNaoTributado(impostoPis);
                case "49": 
                    return GetPisOutros(impostoPis);
            }

            throw new InvalidOperationException($"PIS inválido: {impostoPis.Pis}");
        }

        private static PISBasico GetPisOutros(NfceImpostoPis impostoPis)
        {
            return new PISOutr
            {
                CST = impostoPis.Pis.ToZeus(),
                pPIS = impostoPis.Aliquota,
                vBC = impostoPis.BaseCalculo,
                vPIS = impostoPis.Valor
            };
        }

        private static PISBasico GetPisNaoTributado(NfceImpostoPis impostoPis)
        {
            return new PISNT
            {
                CST = impostoPis.Pis.ToZeus(),
            };
        }

        private static PISBasico GetPisQtde(NfceImpostoPis impostoPis)
        {
            throw new InvalidCastException("PIS quantidade não disponível");
        }

        private static PISBasico GetPisAliquota(NfceImpostoPis impostoPis)
        {
            return new PISAliq
            {
                CST = impostoPis.Pis.ToZeus(),
                pPIS = impostoPis.Aliquota,
                vBC = impostoPis.BaseCalculo,
                vPIS = impostoPis.Valor
            };
        }
    }
}