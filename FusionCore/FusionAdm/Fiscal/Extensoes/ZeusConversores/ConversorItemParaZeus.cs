using System;
using Amazon.Runtime.Internal;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Basico;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using NFe.Classes.Informacoes.Detalhe;
using NFe.Classes.Informacoes.Detalhe.ProdEspecifico;
using static System.Decimal;

namespace FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores
{
    public static class ConversorItemParaZeus
    {
        public static prod ToZeus(this ItemNfe item)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioProduto(sessao);

                repositorio.Refresh(item.Produto);

                var quantidade = item.Quantidade;
                var valorUnitario = item.ValorUnitario;
                var descontoBruto = Round(item.TotalDescontoItem + item.ValorDescontoFixoRateio, 2);
                var valorBruto = item.TotalBruto;

                var ncm = item.Produto.Ncm;
                var cest = item.Produto.Cest;
                var codigoDfe = item.Produto.CodigoDfe.TrimSefaz(60);

                var prod = new prod
                {
                    vOutro = item.ValorDespesasFixaRateio,
                    vSeg = item.ValorSeguroFixoRateio,
                    vFrete = item.ValorFreteFixoRateio,
                    cProd = item.Produto.Id.ToString(),
                    vDesc = descontoBruto,
                    vProd = valorBruto,
                    qTrib = item.QuantidadeTributavelParaNfe(),
                    uTrib = item.UnidadeTributavelParaNfe(),
                    vUnTrib = item.ValorTributavelParaNfe(),
                    qCom = quantidade,
                    uCom = item.SiglaUnidade.TrimSefaz(),
                    vUnCom = valorUnitario,
                    xProd = item.Produto.Nome.TrimSefaz(120),
                    CFOP = int.Parse(item.Cfop.Cfop.Id),
                    indTot = IndicadorTotal.ValorDoItemCompoeTotalNF,
                    NCM = ncm,
                    CEST = !string.IsNullOrWhiteSpace(cest) ? cest : null,
                    cBenef = item.CodigoBeneficioFiscal.TrimSefazOrNull(10)
                };

                if (codigoDfe.IsNotNullOrEmpty())
                {
                    prod.cProd = codigoDfe;
                }

                DefineIndicadorEscala(item, prod);

                var mercadoriaCodigoBarras = item.CodigoBarras;
                prod.cEAN = mercadoriaCodigoBarras.TrimSefazOrNull(14);
                prod.cEANTrib = mercadoriaCodigoBarras.TrimSefazOrNull(14);

                const string semGtin = "SEM GTIN";

                if (mercadoriaCodigoBarras.IsNullOrEmpty())
                {
                    prod.cEAN = semGtin;
                    prod.cEANTrib = semGtin;
                }

                if (prod.cEAN != semGtin)
                {
                    if (mercadoriaCodigoBarras != string.Empty && !Gs1GtinHelper.EhUmGtinValido(mercadoriaCodigoBarras))
                    {
                        throw new InvalidOperationException($"Código de Barras é inválido {mercadoriaCodigoBarras} (item: {item.NumeroItem}");
                    }

                    prod.cEAN = mercadoriaCodigoBarras;
                    prod.cEANTrib = mercadoriaCodigoBarras;

                    if (mercadoriaCodigoBarras.IsNullOrEmpty())
                    {
                        prod.cEAN = semGtin;
                        prod.cEANTrib = semGtin;
                    }
                }

                if (item.NumeroPedido.IsNotNullOrEmpty())
                {
                    prod.xPed = item.NumeroPedido;
                }

                if (item.NumeroItemPedido != 0)
                {
                    prod.nItemPed = item.NumeroItemPedido;
                }

                if (!string.IsNullOrWhiteSpace(item.Produto.CodigoAnp))
                {
                    var produto = item.Produto;
                    var anp = produto.CarregaAnp();

                    prod.ProdutoEspecifico = new AutoConstructedList<ProdutoEspecifico>()
                    {
                        new comb
                        {
                            cProdANP = anp.Id,
                            UFCons = item.GetSiglaUfDestino(),
                            descANP = anp.Descricao,
                            pGLP = produto.PercentualGlpPetroleo == 0 ? (decimal?) null : produto.PercentualGlpPetroleo,
                            pGNn = produto.PercentualGasNacional == 0 ? (decimal?) null : produto.PercentualGasNacional,
                            pGNi = produto.PercentualGasImportador == 0
                                ? (decimal?) null
                                : produto.PercentualGasImportador,
                            vPart = produto.ValorDePartida == 0 ? (decimal?) null : produto.ValorDePartida
                        }
                    };
                }

                return prod;
            }
        }

        private static void DefineIndicadorEscala(ItemNfe item, prod prod)
        {
            if (item.Produto.IsControlaIndicadorEscala == false) return;

            if (!item.Produto.IsIndicadorEscalaRelevante)
            {
                prod.indEscala = indEscala.N;
                prod.CNPJFab = item.Produto.CnpjFabricante.TrimSefaz(14);
            }

            if (!item.Produto.IsIndicadorEscalaRelevante)
            {
                prod.indEscala = indEscala.S;
            }
        }
    }
}