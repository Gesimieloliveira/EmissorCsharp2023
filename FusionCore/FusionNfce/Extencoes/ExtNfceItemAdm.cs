using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Federal;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtNfceItemAdm
    {
        public static NfceItemAdm ToAdm(this NfceItem item, NfceAdm nfceAdm)
        {
            var itemAdm = new NfceItemAdm
            {
                Id = 0,
                Nfce = nfceAdm,
                Quantidade = item.Quantidade,
                Cancelado = item.Cancelado,
                ValorUnitario = item.ValorUnitario,
                Nome = item.Nome,
                Desconto = item.Desconto,
                DescontoAlteraItem = item.DescontoAlteraItem,
                NumeroItem = item.NumeroItem,
                Produto = item.Produto.ToAdm(),
                SiglaUnidade = item.SiglaUnidade,
                SiglaUnidadeTributavel = item.SiglaUnidadeTributavel,
                CodigoNcm = item.CodigoNcm,
                CodigoCest = item.CodigoCest,
                Gtin = item.Gtin,
                ValorTributoAproximado = item.ValorTributoAproximado,
                ValorTributoEstadual = item.ValorTributoEstadual,
                ValorTributoFederal = item.ValorTributoFederal,
                Cfop = item.Cfop.ToAdm(),
                Acrescimo = item.Acrescimo,
                PrecoCusto = item.PrecoCusto,
                PrecoVenda = item.PrecoVenda,
                Observacao = item.Observacao
            };

            var impostoIcms = new NfceImpostoCsosnAdm
            {
                Id = 0,
                Item = itemAdm,
                OrigemMercadoria = item.ImpostoIcms.OrigemMercadoria,
                CST = new TributacaoCst
                {
                    Id = item.ImpostoIcms.CST.Id
                },
                AliquotaIcms = item.ImpostoIcms.AliquotaIcms,
                ReducaoBcIcms = item.ImpostoIcms.ReducaoBcIcms,
                BcIcms = item.ImpostoIcms.BcIcms,
                ValorIcms = item.ImpostoIcms.ValorIcms
            };


            var impostoCofins = new NfceImpostoCofinsAdm
            {
                Id = 0,
                Item = itemAdm,
                Cofins = new TributacaoCofins
                {
                    Id = item.ImpostoCofins.Cofins.Id
                },
                Aliquota = item.ImpostoCofins.Aliquota,
                BaseCalculo = item.ImpostoCofins.BaseCalculo,
                Valor = item.ImpostoCofins.Valor
            };


            var impostoPis = new NfceImpostoPisAdm
            {
                Id = 0,
                Item = itemAdm,
                Pis = new TributacaoPis
                {
                    Id = item.ImpostoPis.Pis.Id
                },
                Aliquota = item.ImpostoPis.Aliquota,
                BaseCalculo = item.ImpostoPis.BaseCalculo,
                Valor = item.ImpostoPis.Valor
            };

            itemAdm.ImpostoIcms = impostoIcms;
            itemAdm.ImpostoCofins = impostoCofins;
            itemAdm.ImpostoPis = impostoPis;

            return itemAdm;
        }
    }
}