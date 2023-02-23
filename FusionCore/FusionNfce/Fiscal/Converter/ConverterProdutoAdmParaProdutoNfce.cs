using System;
using FusionCore.FusionNfce.Fiscal.Regras;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.FusionNfce.Produto;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate.Util;

namespace FusionCore.FusionNfce.Fiscal.Converter
{
    public class ConverterProdutoAdmParaProdutoNfce
    {
        private readonly ProdutoDTO _produto;
        private readonly NcmDTO _ncm;
        private readonly ProdutoEstoqueDTO _produtoEstoque;
        public event EventHandler<ProdutoNfce> DeletarAliasHandler; 

        public ConverterProdutoAdmParaProdutoNfce(ProdutoEstoqueDTO produtoEstoque, NcmDTO ncm)
        {
            _produtoEstoque = produtoEstoque;
            _produto = _produtoEstoque.ProdutoDTO;
            _ncm = ncm;
        }

        public ProdutoNfce Executar()
        {
            var produtoNfce = new ProdutoNfce
            {
                Id = _produto.Id,
                Ativo = _produto.Ativo,
                Nome = _produto.Nome,
                AliquotaIcms = _produto.AliquotaIcms,
                ReducaoIcms = _produto.ReducaoIcms,
                PrecoCompra = _produto.PrecoCusto == 0 ? _produto.PrecoCompra : _produto.PrecoCusto,
                PrecoVenda = _produto.PrecoVenda,
                UnidadeMedida = new ProdutoUnidadeNfce { Id = _produto.ProdutoUnidadeDTO.Id },
                UnidadeMedidaTributavel = _produto.ProdutoUnidadeTributavel != null ? new ProdutoUnidadeNfce { Id = _produto.ProdutoUnidadeTributavel.Id } : null,
                RegraSaida = NfceRegraTributacaoSaida.From(_produto.RegraTributacaoSaida),
                Ncm = _ncm.Id,
                Cest = _ncm.Cest,
                Estoque = _produtoEstoque.Estoque,
                OrigemMercadoria = _produto.OrigemMercadoria,
                CodigoAnp = _produto.CodigoAnp,
                Referencia = _produto.ReferenciaInterna,
                CodigoBalanca = _produto.CodigoBalanca,
                PercentualGasImportador = _produto.PercentualGasImportador,
                PercentualGasNacional = _produto.PercentualGasNacional,
                PercentualGlpPetroleo = _produto.PercentualGlpPetroleo,
                ValorDePartida = _produto.ValorDePartida,
                AliquotaCofins = _produto.AliquotaCofins,
                AliquotaPis = _produto.AliquotaPis,
                Cofins = new NfceCofins { Id = _produto.Cofins.Id },
                Pis = new NfcePis { Id = _produto.Pis.Id },
                QuantidadeUnidadeTributavel = _produto.QuantidadeUnidadeTributavel,
                Observacao = _produto.Observacao,
                UsarObservacaoNoItemFiscal = _produto.UsarObservacaoNoItemFiscal
                
                
            };

            OnDeletarAliasHandler(produtoNfce);

            _produtoEstoque.ProdutoDTO.ProdutosAlias?.ForEach(pa =>
            {
                var produtoAliasNFce = new ProdutoAliasNfce
                {
                    Id = pa.Id,
                    Produto = produtoNfce,
                    Alias = pa.Alias,
                    IsCodigoBarras = pa.IsCodigoBarras
                };

                produtoNfce.ProdutosAlias.Add(produtoAliasNFce);
            });

            return produtoNfce;
        }

        protected virtual void OnDeletarAliasHandler(ProdutoNfce produtoNfce)
        {
            DeletarAliasHandler?.Invoke(this, produtoNfce);
        }
    }
}