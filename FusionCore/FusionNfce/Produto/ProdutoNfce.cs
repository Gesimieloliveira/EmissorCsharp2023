using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Fiscal.FlagsImposto;
using FusionCore.FusionAdm.TabelasDePrecos;
using FusionCore.FusionNfce.Fiscal.Regras;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace FusionCore.FusionNfce.Produto
{
    public class ProdutoNfce : IProdutoTabelaPreco
    {
        public ProdutoNfce() : this(null, 0) { }

        public ProdutoNfce(ProdutoEstoqueDTO produtoEstoque, decimal quantidade)
        {
            Referencia = string.Empty;
            CodigoAnp = string.Empty;
            Cest = string.Empty;

            CopiarInformacoes(produtoEstoque, quantidade);
        }

        public int Id { get; set; }
        public ProdutoUnidadeNfce UnidadeMedida { get; set; }
        public ProdutoUnidadeNfce UnidadeMedidaTributavel { get; set; }
        public string Nome { get; set; }
        public decimal Estoque { get; set; }
        public decimal PrecoVenda { get; set; }
        public string Ncm { get; set; }
        public decimal AliquotaIcms { get; set; }
        public bool Ativo { get; set; }
        public decimal PrecoCompra { get; set; }
        public string Cest { get; set; }
        public decimal Quantidade { get; set; }
        public decimal QuantidadeUnidadeTributavel { get; set; }
        public OrigemMercadoria OrigemMercadoria { get; set; }
        public IList<ProdutoAliasNfce> ProdutosAlias { get; set; } = new List<ProdutoAliasNfce>();
        public string CodigoAnp { get; set; }
        public string Referencia { get; set; }
        public int CodigoBalanca { get; set; }
        public NfceRegraTributacaoSaida RegraSaida { get; set; }
        public NfcePis Pis { get; set; }
        public NfceCofins Cofins { get; set; }
        public decimal AliquotaPis { get; set; }
        public decimal AliquotaCofins { get; set; }
        public string Observacao { get; set; }
        public bool UsarObservacaoNoItemFiscal { get; set; }

        public decimal PercentualGlpPetroleo { get; set; }
        public decimal PercentualGasNacional { get; set; }
        public decimal PercentualGasImportador { get; set; }
        public decimal ValorDePartida { get; set; }
        public decimal ReducaoIcms { get; set; }

        public bool PrecisaSolicitarTotal()
        {
            return UnidadeMedida.PodeFracionar && UnidadeMedida.SolicitaTotal;
        }

        public bool IsTemCodigoDeBarras()
        {
            return ProdutosAlias != null && ProdutosAlias.Count(x => x.IsCodigoBarras == true) > 0;
        }

        public ProdutoAliasNfce ObterPrimeiroCodigoDeBarras()
        {
            return ProdutosAlias.FirstOrDefault(x => x.IsCodigoBarras == true);
        }

        public bool IsTemAlias()
        {
            return ProdutosAlias != null && ProdutosAlias.Count > 0;
        }

        public void ThrowPodeFracionar(decimal quantidade)
        {
            if ((quantidade % 1) == 0)
                return;

            if (UnidadeMedida.PodeFracionar)
                return;

            throw new InvalidOperationException("Unidade do produto não permite o fracionar na quantidade");
        }


        private void CopiarInformacoes(ProdutoEstoqueDTO estoque, decimal quantidade)
        {
            if (estoque == null) return;

            var produto = estoque.ProdutoDTO;

            Id = produto.Id;
            UnidadeMedida = new ProdutoUnidadeNfce(produto.ProdutoUnidadeDTO);
            UnidadeMedidaTributavel = new ProdutoUnidadeNfce(produto.ProdutoUnidadeTributavel);
            Nome = produto.Nome;
            Estoque = estoque.Estoque;
            PrecoVenda = produto.PrecoVenda;
            Ncm = produto.Ncm;
            AliquotaIcms = produto.AliquotaIcms;
            Ativo = produto.Ativo;
            PrecoCompra = produto.PrecoCompra;
            Cest = produto.Cest;
            Quantidade = quantidade;
            QuantidadeUnidadeTributavel = produto.QuantidadeUnidadeTributavel;
            OrigemMercadoria = produto.OrigemMercadoria;

            ProdutosAlias = new List<ProdutoAliasNfce>();
            foreach (var produtoProdutosAlia in produto.ProdutosAlias)
            {
                ProdutosAlias.Add(new ProdutoAliasNfce(produtoProdutosAlia, this));
            }

            CodigoAnp = produto.CodigoAnp;
            Referencia = produto.Referencia;
            CodigoBalanca = produto.CodigoBalanca;
            RegraSaida = NfceRegraTributacaoSaida.From(produto.RegraTributacaoSaida);
            Pis = new NfcePis(produto.Pis);
            Cofins = new NfceCofins(produto.Cofins);
            AliquotaPis = produto.AliquotaPis;
            AliquotaCofins = produto.AliquotaCofins;
            PercentualGlpPetroleo = produto.PercentualGlpPetroleo;
            PercentualGasNacional = produto.PercentualGasNacional;
            PercentualGasImportador = produto.PercentualGasImportador;
            ValorDePartida = produto.ValorDePartida;
            ReducaoIcms = produto.ReducaoIcms;
            UsarObservacaoNoItemFiscal = produto.UsarObservacaoNoItemFiscal;
            Observacao = produto.Observacao;
           
        }
    }
}