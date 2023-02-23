using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Ecf;
using FusionCore.Repositorio.Legacy.Buscas.Adm.PdvPagamento;
using FusionCore.Repositorio.Legacy.Buscas.Adm.VendaEcf;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionCore.Repositorio.Legacy.Flags;
using NHibernate.Util;

namespace FusionCore.PdvSincronizador.Sync.Estrategia
{
    public class EnviarVendasEcf : SincronizacaoBase
    {
        public override string Tag { get; } = "enviar-vendas-ecf";

        public override void Sincronizar(DateTime ultimaSincronizacao)
        {
            var vendasDoPdv = ObtemVendasParaSincronziar(ultimaSincronizacao);
            if (vendasDoPdv.Count == 0)
                return;

            var emissoresDoAdm = ObtemEmissoresDoAdm();
            if (emissoresDoAdm.Count == 0)
                return;

            SessaoPdv.Clear();
            SessaoAdm.Clear();

            vendasDoPdv.ForEach(vendaPdv =>
            {
                if (IgnoraNessaSincronizacao(vendaPdv))
                    return;

                var ecfCompativel = BuscaEcfCompativelDoAdm(emissoresDoAdm, vendaPdv);
                if (ecfCompativel == null)
                    return;

                Cliente pessoaComaptivel = null;

                if (vendaPdv.ClienteDt != null)
                    pessoaComaptivel = BuscaPessoaCompativel(vendaPdv);

                if (vendaPdv.ClienteDt != null && pessoaComaptivel == null)
                    return;

                var transacaoAdm = SessaoAdm.BeginTransaction();
                var transacaoPdv = SessaoPdv.BeginTransaction();

                if (!transacaoAdm.IsActive || !transacaoPdv.IsActive)
                    throw new InvalidOperationException("Transações não foram iniciadas");

                var vendaAdm = CriarVendaAdm(vendaPdv, pessoaComaptivel, ecfCompativel);

                try
                {
                    PersitirVenda(vendaPdv, vendaAdm);
                    EnviarItens(vendaPdv.VendaEcfItens, vendaAdm);
                    EnviarPagamentos(vendaPdv.VendaEcfPagamentos, vendaAdm);

                    transacaoAdm.Commit();
                    transacaoPdv.Commit();
                    RegistraEvento = true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        transacaoAdm.Rollback();
                        transacaoPdv.Rollback();
                        throw;
                    }
                    catch (Exception ez)
                    {
                        throw new Exception(ez.Message, ex);
                    }
                }
            });
        }

        private static bool IgnoraNessaSincronizacao(VendaEcfDt vendaPdv)
        {
            if (vendaPdv.Status == VendaStatus.Aberta)
                return true;

            if (vendaPdv.Cancelado == IntBinario.Nao && vendaPdv.IdentificadorRemoto != null)
                return true;

            return false;
        }

        private IList<VendaEcfDt> ObtemVendasParaSincronziar(DateTime ultimaSincronizacao)
        {
            var repositorioVendaPdv = new VendaEcfRepositorio(SessaoPdv);
            return repositorioVendaPdv.BuscaParaSinronizacao(ultimaSincronizacao);
        }

        private Cliente BuscaPessoaCompativel(VendaEcfDt vendaPdv)
        {
            var repositorioPessoa = new RepositorioPessoa(SessaoAdm);

            return repositorioPessoa.GetClientePeloId(vendaPdv.ClienteDt.Id);
        }

        private IList<PdvEcfDTO> ObtemEmissoresDoAdm()
        {
            var repositorio = new RepositorioComun<PdvEcfDTO>(SessaoAdm);
            return repositorio.Busca(new TodosEcf());
        }

        private static PdvEcfDTO BuscaEcfCompativelDoAdm(IEnumerable<PdvEcfDTO> emissoresDoAdm, VendaEcfDt vendaPdv)
        {
            var query = emissoresDoAdm.Where(emissor => emissor.Id == vendaPdv.EcfDt.Id);
            return (PdvEcfDTO) query.FirstOrNull();
        }

        private static PdvVendaDTO CriarVendaAdm(VendaEcfDt vendaPdv, Cliente pessoa, PdvEcfDTO ecf)
        {
            var vendaEcf = new PdvVendaDTO
            {
                Id = vendaPdv.IdentificadorRemoto ?? 0,
                PessoaDTO = pessoa,
                PdvEcfDTO = ecf,
                SerieEcf = vendaPdv.SerieEcf,
                Cfop = vendaPdv.Cfop,
                Coo = vendaPdv.Coo,
                Ccf = vendaPdv.Ccf,
                VendidoEm = vendaPdv.VendidoEm,
                TotalFinal = vendaPdv.TotalFinal,
                TotalCupom = vendaPdv.TotalCupom,
                TaxaDesconto = vendaPdv.TaxaDesconto,
                Desconto = vendaPdv.Desconto,
                TaxaAcrescimo = vendaPdv.TaxaDesconto,
                Acrescimo = vendaPdv.Acrescimo,
                TotalRecebido = vendaPdv.TotalFinal,
                Troco = vendaPdv.Troco,
                TotalCancelado = vendaPdv.TotalCancelado,
                TotalProdutos = vendaPdv.TotalProdutos,
                TotalBaseIcms = vendaPdv.TotalBaseIcms,
                AcrescimoItens = vendaPdv.AcrescimoItens,
                DescontoItens = vendaPdv.DescontoItens,
                Status = vendaPdv.Status,
                NomeCliente = vendaPdv.NomeCliente,
                DocumentoCliente = vendaPdv.DocumentoCliente,
                Cancelado = vendaPdv.Cancelado,
                QuantidadeItens = vendaPdv.QuantidadeItens,
                EnderecoCliente = vendaPdv.EnderecoCliente,
                Observacao = vendaPdv.Observacao,
                AlteradoEm = vendaPdv.AlteradoEm,
                IndicadorPagamento = vendaPdv.IndicadorPagamento,
                Malote = null,
                UuidVenda = vendaPdv.UuidVenda
            };

            if (vendaEcf.Malote != null)
                vendaEcf.Malote.OrigemUuid = vendaPdv.UuidVenda;

            return vendaEcf;
        }

        private void PersitirVenda(VendaEcfDt vendaPdv, PdvVendaDTO vendaPdvAdm)
        {
            var repositorioAdm = new RepositorioComun<PdvVendaDTO>(SessaoAdm);

            var vendaJaExiste = repositorioAdm.Busca(new BuscaVendaPelaEcfECoo(vendaPdvAdm.PdvEcfDTO.Id, vendaPdvAdm.Coo));

            if (vendaJaExiste != null)
            {
                SessaoAdm.Evict(vendaJaExiste);
                vendaPdv.IdentificadorRemoto = vendaJaExiste.Id;
                vendaPdvAdm.Id = vendaJaExiste.Id;
            }
            

            var retornoVendaPdvAdm = repositorioAdm.Salva(vendaPdvAdm);

            var repositorioVendaPdv = new VendaEcfRepositorio(SessaoPdv);

            vendaPdv.IdentificadorRemoto = retornoVendaPdvAdm.Id;

            repositorioVendaPdv.Alterar(vendaPdv);
        }

        private void EnviarItens(ICollection<VendaEcfItemDt> itensPdv, PdvVendaDTO vendaAdm)
        {
            if (itensPdv?.Count == 0)
                return;

            itensPdv.ForEach(item => EnviarItem(item, vendaAdm));
        }

        private void EnviarItem(VendaEcfItemDt itemPdv, PdvVendaDTO vendaAdm)
        {
            var produtoCompativel = ObtemProdutoCompativel(itemPdv.ProdutoDt);
            var itemAdm = CriaItemAdm(itemPdv, produtoCompativel, vendaAdm);

            var repositorioAdm = new RepositorioComun<PdvVendaItemDTO>(SessaoAdm);

            if (itemPdv.IdentificadorRemoto > 0)
            {
                repositorioAdm.Altera(itemAdm);
                return;
            }

            repositorioAdm.Persiste(itemAdm);
            itemPdv.IdentificadorRemoto = itemAdm.Id;

            var repositorioPdv = new VendaEcfItemRepositorio(SessaoPdv);
            repositorioPdv.Alterar(itemPdv);
        }

        private ProdutoDTO ObtemProdutoCompativel(ProdutoDt produtoDt)
        {
            var repositorio = new RepositorioProduto(SessaoAdm);
            return repositorio.GetPeloId(produtoDt.Id);
        }

        private static PdvVendaItemDTO CriaItemAdm(VendaEcfItemDt itemPdv, ProdutoDTO produto,
            PdvVendaDTO vendaAdm)
        {
            return new PdvVendaItemDTO
            {
                Id = itemPdv.IdentificadorRemoto ?? 0,
                PdvVendaDTO = vendaAdm,
                PdvEcfDTO = vendaAdm.PdvEcfDTO,
                Coo = itemPdv.Coo,
                Ccf = itemPdv.Ccf,
                SerieEcf = itemPdv.SerieEcf,
                ProdutoDt = produto,
                Cfop = itemPdv.Cfop,
                NomeProduto = itemPdv.NomeProduto,
                SiglaUnidadeProduto = itemPdv.SiglaUnidadeProduto,
                CodigoBarra = itemPdv.CodigoBarra,
                NumeroItem = itemPdv.NumeroItem,
                Quantidade = itemPdv.Quantidade,
                PrecoUnitario = itemPdv.PrecoUnitario,
                Total = itemPdv.Total,
                BaseIcms = itemPdv.BaseIcms,
                TaxaIcms = itemPdv.TaxaIcms,
                Icms = itemPdv.Icms,
                TaxaDesconto = itemPdv.Desconto,
                TaxaIssqn = itemPdv.TaxaIssqn,
                Issqn = itemPdv.Issqn,
                TaxaPis = itemPdv.TaxaPis,
                Pis = itemPdv.Pis,
                TaxaCofins = itemPdv.TaxaCofins,
                Cofins = itemPdv.Cofins,
                TaxaAcrescimo = itemPdv.TaxaAcrescimo,
                Acrescimo = itemPdv.Acrescimo,
                AcrescimoRateio = itemPdv.AcrescimoRateio,
                Desconto = itemPdv.Desconto,
                DescontoRateio = itemPdv.DescontoRateio,
                TotalizadorParcial = itemPdv.TotalizadorParcial,
                Cst = itemPdv.Cst,
                SituacaoTributariaIcms = itemPdv.SituacaoTributariaIcms,
                IcmsEcf = itemPdv.IcmsEcf,
                Cancelado = itemPdv.Cancelado,
                AlteradoEm = itemPdv.AlteradoEm
            };
        }

        private void EnviarPagamentos(ICollection<VendaEcfPagamentoDt> pagamentosPdv, PdvVendaDTO vendaAdm)
        {
            if (pagamentosPdv?.Count == 0)
                return;

            pagamentosPdv.ForEach(pag => EnviarPagamento(pag, vendaAdm));
        }

        private void EnviarPagamento(VendaEcfPagamentoDt pagamento, PdvVendaDTO vendaAdm)
        {
            if (pagamento.IdentificadorRemoto > 0)
                return;

            var formaPagamentoAdm = ObtemPagamentoCompativel(pagamento.FormaPagamentoEcfDt);
            var pagamentoAdm = CriaPagamentoAdm(pagamento, formaPagamentoAdm, vendaAdm);

            var repositorioAdm = new RepositorioComun<PdvVendaPagamentoDTO>(SessaoAdm);
            repositorioAdm.Persiste(pagamentoAdm);
            pagamento.IdentificadorRemoto = pagamentoAdm.Id;

            var repositorioPdv = new VendaEcfPagamentoRepositorio(SessaoPdv);
            repositorioPdv.Alterar(pagamento);
        }

        private PdvFormaPagamentoDTO ObtemPagamentoCompativel(FormaPagamentoEcfDt formaPagamento)
        {
            var repositorio = new RepositorioComun<PdvFormaPagamentoDTO>(SessaoAdm);
            return repositorio.Busca(new PdvPagamentoPorId(formaPagamento.Id));
        }

        private static PdvVendaPagamentoDTO CriaPagamentoAdm(VendaEcfPagamentoDt pagamento,
            PdvFormaPagamentoDTO formaPagamento, PdvVendaDTO vendaPdvAdm)
        {
            return new PdvVendaPagamentoDTO
            {
                PdvVendaDTO = vendaPdvAdm,
                PdvFormaPagamentoDTO = formaPagamento,
                SerieEcf = pagamento.SerieEcf,
                Cco = pagamento.Cco,
                Ccf = pagamento.Ccf,
                Valor = pagamento.Valor,
                Nsu = pagamento.Nsu,
                Estorno = pagamento.Estorno,
                Rede = pagamento.Rede,
                CartaoDebito = pagamento.CartaoDebito,
                CartaoCredito = pagamento.CartaoCredito,
                AlteradoEm = pagamento.AlteradoEm,
                BandeiraCartao = pagamento.BandeiraCartao,
                CodigoAutorizacao = pagamento.CodigoAutorizacao ?? string.Empty,
                ComprovanteEmitidoEm = pagamento.ComprovanteEmitidoEm,
                Desconto = pagamento.Desconto,
                NomeAdministradora = pagamento.NomeAdministradora,
                QuantidadeParcelas = pagamento.QuantidadeParcelas,
                Saque = pagamento.Saque,
                TipoParcelamento = pagamento.TipoParcelamento,
                TipoTransacao = pagamento.TipoTransacao
            };
        }
    }
}