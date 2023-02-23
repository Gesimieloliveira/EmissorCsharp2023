using System;
using Amazon.Runtime.Internal;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Produto;
using FusionCore.FusionNfce.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionNfce;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.ProdEspecifico;
using OpenAC.Net.Core.Extensions;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtNfceItemZeus
    {
        public static prod ToZeus(this NfceItem item)
        {
            var totalDesconto = item.Desconto + item.DescontoAlteraItem;

            var prod = new prod
            {
                vProd = item.ValorTotal,
                vDesc = totalDesconto == 0 ? (decimal?) null : totalDesconto,
                vOutro = item.Acrescimo == 0 ? (decimal?) null : item.Acrescimo,
                CFOP = int.Parse(item.Cfop.Id),
                cEAN = item.Gtin, 
                cEANTrib = item.Gtin, 
                cProd = item.Produto.Id.ToString(),
                indTot = IndicadorTotal.ValorDoItemCompoeTotalNF,
                qCom = item.Quantidade,
                uCom = item.SiglaUnidade,
                vUnCom = item.ValorUnitario,
                xProd = item.Nome.TrimSefaz(120),
                CEST = string.IsNullOrWhiteSpace(item.CodigoCest) ? null : item.CodigoCest,
                NCM = string.IsNullOrWhiteSpace(item.CodigoNcm) ? null : item.CodigoNcm,
                qTrib = item.Quantidade,
                uTrib = item.ObterSiglaUnidadeTributavel(),
                vUnTrib = item.ValorUnitario
            };

            if (item.SiglaUnidadeTributavel.IsNotNullOrEmpty())
            {
                prod.qTrib = item.QuantidadeUnidadeTributavel;
                prod.uTrib = item.ObterSiglaUnidadeTributavel();
                prod.vUnTrib = (item.ValorTotal / item.QuantidadeUnidadeTributavel).RoundABNT(10);
            }

            MontaProdutoComb(item, prod);

            return prod;
        }

        private static void MontaProdutoComb(NfceItem item, prod prod)
        {
            var produto = item.Produto;
            var cProdAnp = produto.CodigoAnp;

            if (string.IsNullOrWhiteSpace(cProdAnp)) return;

            var produtoCodigoAnp = BuscarProdutoCodigoAnp(cProdAnp);

            if (produtoCodigoAnp == null)
                throw new InvalidOperationException("Produto Código ANP não encontrado");

            prod.ProdutoEspecifico = new AutoConstructedList<ProdutoEspecifico>()
            {
                new comb
                {
                    cProdANP = cProdAnp,
                    UFCons = item.GetSiglaUfConsumo(),
                    descANP = produtoCodigoAnp.Descricao,
                    pGLP = produto.PercentualGlpPetroleo == 0 ? (decimal?)null : produto.PercentualGlpPetroleo,
                    pGNn = produto.PercentualGasNacional == 0 ? (decimal?)null : produto.PercentualGasNacional,
                    pGNi = produto.PercentualGasImportador == 0 ? (decimal?)null : produto.PercentualGasImportador,
                    vPart = produto.ValorDePartida == 0 ? (decimal?)null : produto.ValorDePartida
                }
            };
        }

        private static ProdutoCodigoAnpNfce BuscarProdutoCodigoAnp(string id)
        {
            using (var repositorioProduto = new RepositorioProdutoNfce(GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao()))
            {
                return repositorioProduto.BuscaPorCodigoAnp(id);
            }
        }
    }
}