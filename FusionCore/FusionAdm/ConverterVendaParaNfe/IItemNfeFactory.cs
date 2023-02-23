using System.Collections.Generic;
using System.Linq;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Perfil;
using FusionCore.FusionAdm.Fiscal.Tributacoes;
using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.Helpers.Pessoa;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.SelecionarNfce;
using FusionCore.Sessao;
using FusionCore.Tributacoes.Estadual;
using FusionCore.Tributacoes.Flags;
using FusionCore.Tributacoes.Repositorio;
using FusionCore.Vendas.Faturamentos;

namespace FusionCore.FusionAdm.ConverterVendaParaNfe
{
    public class ProdutoBuilderNfe
    {
        public ProdutoBuilderNfe()
        {
            
        }
    }

    public interface IItemNfeFactory<T>
    {
        void Cria(T dado, Nfeletronica nfeletronica, Cliente cliente, PerfilNfe perfilNfe, EmitenteNfe emitenteNfe);
    }

    public class FaturamentoProdutoParaItemNfe : IItemNfeFactory<FaturamentoProduto>
    {
        public void Cria(FaturamentoProduto faturamentoProduto,
            Nfeletronica nfeletronica,
            Cliente cliente,
            PerfilNfe perfilNfe,
            EmitenteNfe emitenteNfe)
        {
            var item = new ItemNfe(nfeletronica)
            {
                AutoAjustarImposto = true,
            };

            item.ComMercadoria(faturamentoProduto.Produto, faturamentoProduto.Quantidade, faturamentoProduto.PrecoUnitario, faturamentoProduto.TotalDesconto);

            SetarCodigoBarras(item, faturamentoProduto.Produto.ProdutosAlias);

            item.ComCodigoUtilizado(faturamentoProduto.Produto.Id.ToString());

            SetCfopItem(item, cliente, perfilNfe, emitenteNfe);
            SetPisCofins(item, faturamentoProduto.Produto);
            SetIpi(item, faturamentoProduto);

            item.CodigoBeneficioFiscal = string.Empty;

            DefineIcms(item, faturamentoProduto, perfilNfe, emitenteNfe);

            item.MovimentaEstoque = false;

            nfeletronica.AdicionarItem(item);
        }

        private void SetarCodigoBarras(ItemNfe itemNfe, IList<ProdutoAlias> produtoProdutosAlias)
        {
            if (produtoProdutosAlias == null || produtoProdutosAlias.Count == 0)
            {
                itemNfe.ComBarras(null);
                return;
            }

            var alias = produtoProdutosAlias.FirstOrDefault(x => x.IsCodigoBarras);
            if (alias == null)
                return;

            itemNfe.ComBarras(alias.Alias);
        }
        private void SetCfopItem(ItemNfe item, Cliente cliente, PerfilNfe perfilNfe, EmitenteNfe emitenteNfe)
        {
            var enderecoCliente = cliente.GetEnderecoPrincipal();

            using (var sessao = new SessaoManagerAdm().CriaSessao())
            {
                var repositorioPerfilCfop = new RepositorioPerfilCfop(sessao);
                var cfopPerfilNfe = perfilNfe.Cfop;

                if (emitenteNfe.Empresa.CidadeDTO.SiglaUf != enderecoCliente.Cidade.SiglaUf)
                {
                    var cfop = cfopPerfilNfe ?? repositorioPerfilCfop.PeloCodigo("6929");

                    item.Cfop = cfop;
                    item.PartilharIcms = true;
                }

                if (emitenteNfe.Empresa.CidadeDTO.SiglaUf == enderecoCliente.Cidade.SiglaUf)
                {
                    var cfop = cfopPerfilNfe ?? repositorioPerfilCfop.PeloCodigo("5929");

                    item.Cfop = cfop;
                    item.PartilharIcms = false;
                }

                if (enderecoCliente.Cidade.SiglaUf == "EX")
                {
                    var cfop = cfopPerfilNfe ?? repositorioPerfilCfop.PeloCodigo("7929");

                    item.Cfop = cfop;
                    item.PartilharIcms = false;
                }
            }
        }
        private void SetPisCofins(ItemNfe item, ProdutoDTO produto)
        {
            item.Pis = new ImpostoPis(item)
            {
                AliquotaPis = produto.AliquotaPis,
                Cst = produto.Pis
            };

            item.Pis.AjustarPis();

            item.Cofins = new ImpostoCofins(item)
            {
                AliquotaCofins = produto.AliquotaCofins,
                Cst = produto.Cofins
            };

            item.Cofins.AjustarCofins();
        }
        private static void SetIpi(ItemNfe item, FaturamentoProduto faturamentoProduto)
        {
            item.Ipi = new ImpostoIpi(item, faturamentoProduto.Produto.SituacaoTributariaIpi)
            {
                AliquotaIpi = 0.0m, 
                TributacaoIpi = faturamentoProduto.Produto.SituacaoTributariaIpi
            };

            item.Ipi.AjustarIpi();
        }
        private void DefineIcms(ItemNfe nfeItem, FaturamentoProduto faturamentoProduto, PerfilNfe perfilNfe, EmitenteNfe emitenteNfe)
        {
            if (perfilNfe.SimplesNacional != null && perfilNfe.SimplesNacional.Csosn != null && emitenteNfe.Empresa.RegimeTributario == RegimeTributario.SimplesNacional)
            {
                nfeItem.ImpostoIcms = new ImpostoIcms(nfeItem, ProcuraTributacaoEquivalente(perfilNfe.SimplesNacional.Csosn))
                {
                    AliquotaCredito = perfilNfe.SimplesNacional.Csosn.PermiteCredito() ? perfilNfe.SimplesNacional.AliquotaCredito : 0.0m
                };

                return;
            }

            var produto = faturamentoProduto.Produto;
            var cst = FindTributacaoSt(produto, emitenteNfe);

            nfeItem.ImpostoIcms = new ImpostoIcms(nfeItem, cst);

            if (cst.PermiteIcms())
            {
                nfeItem.ImpostoIcms.AliquotaIcms = produto.AliquotaIcms;
                nfeItem.ImpostoIcms.ReducaoBcIcms = cst.PermiteReducaoIcms() ? produto.ReducaoIcms : 0;
            }

            if (cst.PermiteSubstituicao())
            {
                nfeItem.ImpostoIcms.AliquotaSt = produto.AliquotaIcms;
                nfeItem.ImpostoIcms.ReducaoBcSt = produto.ReducaoIcms;
                nfeItem.ImpostoIcms.MvaSt = produto.PercentualMva;
            }

            nfeItem.ImpostoIcms.AjustarImposto();
        }

        private TributacaoCst ProcuraTributacaoEquivalente(TributacaoCsosn csosn)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var rcst = new RepositorioTributacaoCst(sessao);
                var cst = rcst.GetPeloId(csosn.Codigo);
                return cst;
            }
        }
        private TributacaoCst FindTributacaoSt(ProdutoDTO produto, EmitenteNfe emitenteNfe)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositoriocst = new RepositorioTributacaoCst(sessao);

                if (emitenteNfe.RegimeTributario != RegimeTributario.SimplesNacional)
                {
                    var cst = produto.RegraTributacaoSaida.Cst;
                    return repositoriocst.GetPeloId(cst.Codigo);
                }

                var csosn = produto.RegraTributacaoSaida.Csosn.Codigo;

                return repositoriocst.GetPeloId(csosn);
            }
        }
    }

    public class NfceAdmItemParaItemNfe : IItemNfeFactory<NfceItemAdm>
    {
        public void Cria(NfceItemAdm nfceItem, Nfeletronica nfeletronica, Cliente cliente, PerfilNfe perfilNfe, EmitenteNfe emitenteNfe)
        {
            var item = new ItemNfe(nfeletronica)
            {
                AutoAjustarImposto = true
            };

            item.ComMercadoria(nfceItem.Produto, nfceItem.Quantidade, nfceItem.ValorUnitario, nfceItem.DescontoAlteraItem);

            item.ComBarras(nfceItem.Gtin);

            item.ComCodigoUtilizado(nfceItem.Produto.Id.ToString());

            SetCfopItem(item, cliente, perfilNfe, emitenteNfe);
            SetPisCofins(item, nfceItem);
            SetIpi(item, nfceItem);

            item.CodigoBeneficioFiscal = string.Empty;

            DefineIcms(item, nfceItem, perfilNfe, emitenteNfe);

            item.MovimentaEstoque = false;

            nfeletronica.AdicionarItem(item);
        }

        private void SetCfopItem(ItemNfe item, Cliente cliente, PerfilNfe perfilNfe, EmitenteNfe emitenteNfe)
        {
            var enderecoCliente = cliente.GetEnderecoPrincipal();

            using (var sessao = new SessaoManagerAdm().CriaSessao())
            {
                var repositorioPerfilCfop = new RepositorioPerfilCfop(sessao);
                var cfopPerfilNfe = perfilNfe.Cfop;

                if (emitenteNfe.Empresa.CidadeDTO.SiglaUf != enderecoCliente.Cidade.SiglaUf)
                {
                    var cfop = cfopPerfilNfe ?? repositorioPerfilCfop.PeloCodigo("6929");

                    item.Cfop = cfop;
                    item.PartilharIcms = true;
                }

                if (emitenteNfe.Empresa.CidadeDTO.SiglaUf == enderecoCliente.Cidade.SiglaUf)
                {
                    var cfop = cfopPerfilNfe ?? repositorioPerfilCfop.PeloCodigo("5929");

                    item.Cfop = cfop;
                    item.PartilharIcms = false;
                }

                if (enderecoCliente.Cidade.SiglaUf == "EX")
                {
                    var cfop = cfopPerfilNfe ?? repositorioPerfilCfop.PeloCodigo("7929");

                    item.Cfop = cfop;
                    item.PartilharIcms = false;
                }
            }
        }
        private void SetPisCofins(ItemNfe item, NfceItemAdm nfceItem)
        {
            var impostoPisNfce = nfceItem.ImpostoPis;
            var impostoCofinsNfce = nfceItem.ImpostoCofins;

            if (impostoCofinsNfce == null && impostoPisNfce == null)
            {
                var produto = nfceItem.Produto;

                item.Pis = new ImpostoPis(item)
                {
                    AliquotaPis = produto.AliquotaPis,
                    Cst = produto.Pis
                };

                item.Pis.AjustarPis();

                item.Cofins = new ImpostoCofins(item)
                {
                    AliquotaCofins = produto.AliquotaCofins,
                    Cst = produto.Cofins
                };

                item.Cofins.AjustarCofins();

                return;
            }

            item.Pis = new ImpostoPis(item)
            {
                AliquotaPis = impostoPisNfce.Aliquota,
                Cst = impostoPisNfce.Pis
            };

            item.Pis.AjustarPis();

            item.Cofins = new ImpostoCofins(item)
            {
                AliquotaCofins = impostoCofinsNfce.Aliquota,
                Cst = impostoCofinsNfce.Cofins
            };

            item.Cofins.AjustarCofins();
        }
        private void SetIpi(ItemNfe item, NfceItemAdm nfceItem)
        {
            item.Ipi = new ImpostoIpi(item, nfceItem.Produto.SituacaoTributariaIpi)
            {
                AliquotaIpi = 0.0m,
                TributacaoIpi = nfceItem.Produto.SituacaoTributariaIpi
            };

            item.Ipi.AjustarIpi();
        }
        private void DefineIcms(ItemNfe nfeItem, NfceItemAdm nfceItem, PerfilNfe perfilNfe, EmitenteNfe emitenteNfe)
        {
            if (perfilNfe.SimplesNacional != null && perfilNfe.SimplesNacional.Csosn != null && emitenteNfe.Empresa.RegimeTributario == RegimeTributario.SimplesNacional)
            {
                nfeItem.ImpostoIcms = new ImpostoIcms(nfeItem, ProcuraTributacaoEquivalente(perfilNfe.SimplesNacional.Csosn))
                {
                    AliquotaCredito = perfilNfe.SimplesNacional.Csosn.PermiteCredito() ? perfilNfe.SimplesNacional.AliquotaCredito : 0.0m
                };

                return;
            }

            var icms = nfceItem.ImpostoIcms;
            var cst = icms.CST;

            nfeItem.ImpostoIcms = new ImpostoIcms(nfeItem, cst);

            if (cst.PermiteIcms())
            {
                nfeItem.ImpostoIcms.AliquotaIcms = icms.AliquotaIcms;
                nfeItem.ImpostoIcms.ReducaoBcIcms = cst.PermiteReducaoIcms() ? icms.ReducaoBcIcms : 0;
            }

            if (cst.PermiteSubstituicao())
            {
                nfeItem.ImpostoIcms.AliquotaSt = icms.AliquotaIcms;
                nfeItem.ImpostoIcms.ReducaoBcSt = icms.ReducaoBcIcms;
                nfeItem.ImpostoIcms.MvaSt = 0.0m;
            }

            nfeItem.ImpostoIcms.AjustarImposto();
        }
        private TributacaoCst ProcuraTributacaoEquivalente(TributacaoCsosn csosn)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var rcst = new RepositorioTributacaoCst(sessao);
                var cst = rcst.GetPeloId(csosn.Codigo);
                return cst;
            }
        }
    }

}