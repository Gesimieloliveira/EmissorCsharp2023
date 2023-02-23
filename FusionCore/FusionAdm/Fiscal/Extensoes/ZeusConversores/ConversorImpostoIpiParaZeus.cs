using System;
using FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores.Tipos;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal;
using NFe.Classes.Informacoes.Detalhe.Tributacao.Federal.Tipos;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores
{
    public static class ConversorImpostoIpiParaZeus
    {
        public static IPI ToZeus(this ImpostoIpi ipi, bool usarTagPropria)
        {
            if (usarTagPropria) return null;

            var ipiZeus = new IPI
            {
                CNPJProd = string.IsNullOrWhiteSpace(ipi.CnpjProdutor) ? null : ipi.CnpjProdutor.TrimSefaz(),
                cEnq = ipi.CodigoEnquadramentoLegal,
                cSelo = string.IsNullOrWhiteSpace(ipi.Selo) ? null : ipi.Selo.TrimSefaz(),
                clEnq = string.IsNullOrWhiteSpace(ipi.ClasseEnquadramento) ? null : ipi.ClasseEnquadramento.TrimSefaz(),
                qSelo = ipi.QuantidadeSelo > 0 ? ipi.QuantidadeSelo : (int?) null,
                TipoIPI = GetIpi(ipi)
            };

            return ipiZeus;
        }

        private static IPIBasico GetIpi(ImpostoIpi ipi)
        {
            switch (ipi.TributacaoIpi.Codigo)
            {
                case "01":
                case "02":
                case "03":
                case "04":
                case "51":
                case "52":
                case "53":
                case "54":
                case "55":
                    return GetIpiIsento(ipi);
                case "00":
                case "49":
                case "50":
                case "99":
                    return GetIpiTributado(ipi);
                default:
                    throw new InvalidOperationException("Não foi possível converter o IPI para um IPI válido da NF-e");
            }
        }

        private static IPIBasico GetIpiTributado(ImpostoIpi ipi)
        {
            return new IPITrib
            {
                CST = ipi.TributacaoIpi.ToZeus(),
                vBC = ipi.ValorBcIpi,
                vIPI = ipi.ValorIpi,
                pIPI = ipi.AliquotaIpi
            };
        }

        private static IPIBasico GetIpiIsento(ImpostoIpi ipi)
        {
            return new IPINT
            {
                CST = ipi.TributacaoIpi.ToZeus()
            };
        }
    }
}