using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionLibrary.Helper.Diversos;
using NHibernate.Util;
using ProdutoPdvRepositorio = FusionCore.Repositorio.Legacy.Ativos.Pdv.ProdutoRepositorio;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public class ReceberProduto : SincronizacaoBase
    {
        public override string Tag { get; } = @"receber-produto";

        public override void Sincronizar(DateTime ultimaSincronizacao)
        {
            var produtosEstoque = ObtemProdutos(ultimaSincronizacao);
            if (produtosEstoque.Count == 0)
                return;

            var tributacoesIbpt = ObterTributacoesIbpt();

            var repositorioPdv = new ProdutoPdvRepositorio(SessaoPdv);
            SessaoPdv.Clear();

            var transacao = SessaoPdv.BeginTransaction();
            var produtoAdm = default(ProdutoDTO);
            var produtoPdv = default(ProdutoDt);

            try
            {
                produtosEstoque.ForEach(produtoEstoque =>
                {
                    produtoAdm = produtoEstoque.ProdutoDTO;

                    var ibpt = RecuperaIbpt(tributacoesIbpt, produtoAdm.Ncm);
                    var codigotStDoProduto = produtoAdm.RegraTributacaoSaida.Cst;
                    var icmsDoEcf = codigotStDoProduto.ConverteParaAliquotaEcf(produtoAdm.AliquotaIcms);

                    produtoPdv = new ProdutoDt
                    {
                        Id = produtoAdm.Id,
                        AliquotaIcmsPaf = produtoAdm.AliquotaIcms,
                        SituacaoTributariaIcms = codigotStDoProduto.Codigo,
                        IcmsEcf = icmsDoEcf,
                        Ativo = produtoAdm.Ativo ? 1 : 0,
                        CodigoNcm = produtoAdm.Ncm,
                        Nome = produtoAdm.Nome,
                        PodeFracionar = produtoAdm.ProdutoUnidadeDTO.PodeFracionar,
                        ChaveIbpt = ibpt != null ? ibpt.ChaveIbpt : string.Empty,
                        PrecoCompra = produtoAdm.PrecoCompra.Trunca(),
                        PrecoVenda = produtoAdm.PrecoVenda.Arredonda(),
                        SiglaUnidade = produtoAdm.ProdutoUnidadeDTO.Sigla,
                        TributacaoEstadual = ibpt?.Estadual ?? 0,
                        TributacaoImportado = ibpt?.Importado ?? 0,
                        TributacaoNacional = ibpt?.Nacional ?? 0,
                        Estoque = produtoEstoque.Estoque,
                        SolicitaTotal = produtoAdm.ProdutoUnidadeDTO.SolicitaTotalPdv,
                        CodigoBalanca = produtoAdm.CodigoBalanca
                    };


                    repositorioPdv.DeletarProdutoAlias(produtoPdv);

                    produtoEstoque.ProdutoDTO.ProdutosAlias?.ForEach(pa =>
                    {
                        var produtoAliasNFce = new ProdutoAliasDt
                        {
                            Id = pa.Id,
                            Produto = produtoPdv,
                            Alias = pa.Alias,
                            IsCodigoBarras = pa.IsCodigoBarras
                        };

                        produtoPdv.ProdutosAlias.Add(produtoAliasNFce);
                    });

                    repositorioPdv.Salvar(produtoPdv);
                });

                transacao.Commit();
                RegistraEvento = true;
            }
            catch (Exception)
            {
                Console.WriteLine("Falha ao syncronizar o produto: " + produtoAdm?.Nome);
                Console.WriteLine("Objeto PDV em persistência: " + produtoPdv?.Nome);

                transacao.Rollback();
                throw;
            }
        }

        private IList<ProdutoEstoqueDTO> ObtemProdutos(DateTime ultimaSincronizacao)
        {
            var repositorio = new RepositorioProduto(SessaoAdm);
            var estoques = repositorio.ParaSincronizacaoPdv(ultimaSincronizacao);

            return estoques;
        }

        private IEnumerable<Ibpt> ObterTributacoesIbpt()
        {
            var repositorio = new RepositorioIbpt(SessaoAdm);
            return repositorio.BuscaTodos();
        }

        private static Ibpt RecuperaIbpt(IEnumerable<Ibpt> tributacoesIbpt,
            string codigoNcm)
        {
            if (string.IsNullOrEmpty(codigoNcm))
                return null;

            var where = tributacoesIbpt.Where(t =>
                t.Codigo == codigoNcm &&
                t.Tipo == 0 &&
                t.ExcecaoFiscal == string.Empty);

            return (Ibpt) where.FirstOrNull();
        }
    }
}