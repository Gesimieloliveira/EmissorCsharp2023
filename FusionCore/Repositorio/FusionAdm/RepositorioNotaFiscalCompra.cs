using System;
using System.Collections.Generic;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.FusionAdm.Compras;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sintegra.Dto;
using FusionCore.Tributacoes.Estadual;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioNotaFiscalCompra : Repositorio<NotaFiscalCompra, int>
    {
        public RepositorioNotaFiscalCompra(ISession sessao) : base(sessao)
        {
        }

        public override NotaFiscalCompra GetPeloId(int id)
        {
            var nota = base.GetPeloId(id);

            if (nota == null)
                return null;

            nota.InicializaLazy();
            return nota;
        }

        public void Salvar(NotaFiscalCompra nota)
        {
            if (nota.Id == default(int))
            {
                Sessao.Persist(nota);
                Sessao.Flush();
                return;
            }

            Sessao.Update(nota);
            Sessao.Flush();
        }

        public IList<NotaFiscalCompraGrid> BuscarParaGrid(string filtroRapido = null)
        {
            NotaFiscalCompra nf = null;
            Fornecedor forn = null;
            PessoaEntidade pf = null;
            EmpresaDTO emp = null;
            NotaFiscalCompraGrid alias = null;

            var query = Sessao.QueryOver(() => nf)
                .SelectList(list => list
                    .Select(() => nf.Id).WithAlias(() => alias.Id)
                    .Select(() => nf.NumeroDocumento).WithAlias(() => alias.Numero)
                    .Select(() => nf.Serie).WithAlias(() => alias.Serie)
                    .Select(() => nf.ValorTotalItens).WithAlias(() => alias.TotalItens)
                    .Select(() => nf.ValorTotal).WithAlias(() => alias.ValorTotal)
                    .Select(() => pf.Nome).WithAlias(() => alias.NomeFornecedor)
                    .Select(() => emp.RazaoSocial).WithAlias(() => alias.NomeEmpresa)
                    .Select(() => nf.EmitidaEm).WithAlias(() => alias.EmissaoEm)
                    .Select(() => nf.EntradaSaidaEm).WithAlias(() => alias.EntradaEm)
                    .Select(() => nf.Chave.Chave).WithAlias(() => alias.Chave)
                    .Select(() => nf.TotalBcIcms).WithAlias(() => alias.TotalBcIcms)
                    .Select(() => nf.ValorTotalIcms).WithAlias(() => alias.ValorTotalIcms)
                    .Select(() => nf.TotalBcIcmsSt).WithAlias(() => alias.TotalBcIcmsSt)
                    .Select(() => nf.ValorTotalIcmsSt).WithAlias(() => alias.ValorTotalIcmsSt)
                    .Select(() => nf.ValorTotalIpi).WithAlias(() => alias.ValorTotalIpi)
                    .Select(() => nf.ValorTotalFrete).WithAlias(() => alias.ValorTotalFrete)
                    .Select(() => nf.ValorTotalSeguro).WithAlias(() => alias.ValorTotalSeguro)
                    .Select(() => nf.ValorTotalDesconto).WithAlias(() => alias.ValorTotalDesconto)
                    .Select(() => nf.ValorTotalOutros).WithAlias(() => alias.ValorTotalOutros)
                ).JoinAlias(() => nf.Fornecedor, () => forn, JoinType.InnerJoin)
                .JoinAlias(() => forn.Pessoa, () => pf, JoinType.InnerJoin)
                .JoinAlias(() => nf.Empresa, () => emp, JoinType.InnerJoin)
                .OrderBy(n => n.Id).Desc;

            if (!string.IsNullOrWhiteSpace(filtroRapido))
                if (!string.IsNullOrWhiteSpace(filtroRapido))
                {
                    var typeString = NHibernateUtil.String;

                    var id = Restrictions.Eq(Projections.Cast(typeString, Projections.Property(() => nf.Id)),
                        filtroRapido);
                    var numero =
                        Restrictions.Eq(Projections.Cast(typeString, Projections.Property(() => nf.NumeroDocumento)),
                            filtroRapido);

                    query.Where(id || numero);
                }

            query.TransformUsing(Transformers.AliasToBean<NotaFiscalCompraGrid>());

            return query.List<NotaFiscalCompraGrid>();
        }


        public IList<Registro50ComprasDto> BuscarRegistro50ComprasDtos(DateTime filtroDataInicio,
            DateTime filtroDataFinal,
            EmpresaDTO empresa)
        {
            NotaFiscalCompra nf = null;

            ItemCompra itemCompra = null;
            IcmsCompra icmsCompra = null;
            IpiCompra ipiCompra = null;
            TributacaoCst tributacaoCst = null;
            CfopDTO cfop = null;

            Fornecedor forn = null;
            PessoaEntidade pf = null;
            PessoaEndereco endereco = null;
            CidadeDTO cidade = null;
            Registro50ComprasDto resultado = null;

            var subQuerySiglaUf = QueryOver.Of<PessoaEntidade>()
                .JoinAlias(xE => xE.Enderecos, () => endereco, JoinType.InnerJoin)
                .JoinAlias(() => endereco.Cidade, () => cidade, JoinType.InnerJoin)
                .Where(i => i.Id == pf.Id).SelectList(list =>
                    list.SelectGroup(() => cidade.SiglaUf).WithAlias(() => resultado.SiglaUf)).Take(1);

            var query = Sessao.QueryOver(() => nf)
                .JoinAlias(() => nf.Fornecedor, () => forn, JoinType.InnerJoin)
                .JoinAlias(() => forn.Pessoa, () => pf, JoinType.InnerJoin)
                .JoinAlias(() => NotaFiscalCompra.Expressions.Itens, () => itemCompra, JoinType.InnerJoin)
                .JoinAlias(() => itemCompra.Icms, () => icmsCompra, JoinType.InnerJoin)
                .JoinAlias(() => icmsCompra.Icms, () => tributacaoCst, JoinType.InnerJoin)
                .JoinAlias(() => itemCompra.Ipi, () => ipiCompra, JoinType.InnerJoin)
                .JoinAlias(() => itemCompra.Cfop, () => cfop, JoinType.InnerJoin)
                .SelectList(list => 
                    list
                        .SelectGroup(() => nf.Id)
                        .SelectGroup(() => pf.Id)
                        .SelectGroup(() => pf.Cpf.Valor).WithAlias(() => resultado.Cpf)
                        .SelectGroup(() => pf.Cnpj.Valor).WithAlias(() => resultado.Cnpj)
                        .SelectGroup(() => pf.InscricaoEstadual).WithAlias(() => resultado.InscricaoEstadual)
                        .SelectGroup(() => nf.EntradaSaidaEm).WithAlias(() => resultado.LancamentoEm)
                        .SelectSubQuery(subQuerySiglaUf).WithAlias(() => resultado.SiglaUf)
                        .SelectGroup(() => nf.Serie).WithAlias(() => resultado.Serie)
                        .SelectGroup(() => nf.NumeroDocumento).WithAlias(() => resultado.Numero)
                        .SelectGroup(() => tributacaoCst.Id).WithAlias(() => resultado.Cst)
                        .SelectGroup(() => cfop.Id).WithAlias(() => resultado.Cfop)
                        .SelectSum(() => itemCompra.ValorTotal).WithAlias(() => resultado.ValorTotal)
                        .SelectSum(() => icmsCompra.BaseCalculo).WithAlias(() => resultado.BaseCalculoIcms)
                        .SelectSum(() => icmsCompra.ValorIcms).WithAlias(() => resultado.ValorIcms)
                        .SelectSum(() => icmsCompra.ValorSt).WithAlias(() => resultado.ValorSt)
                        .SelectSum(() => icmsCompra.ValorFcpSt).WithAlias(() => resultado.ValorFcpSt)
                        .SelectSum(() => ipiCompra.ValorIpi).WithAlias(() => resultado.ValorIpi)
                        .SelectSum(() => itemCompra.ValorDespesasRateio).WithAlias(() => resultado.DespesaRateio)
                        .SelectGroup(() => icmsCompra.Aliquota).WithAlias(() => resultado.Aliquota)
                        .SelectGroup(() => nf.Chave.Chave).WithAlias(() => resultado.ChaveNfe));



            var periodo = new FiltroPeriodo(filtroDataInicio, filtroDataFinal);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => nf.EntradaSaidaEm));
            
            var empresaIgual = Restrictions.Eq(Projections.Property(() => nf.Empresa), empresa);

            var and = Restrictions.Conjunction();

            and.Add(intervaloMensal);
            and.Add(empresaIgual);

            query.Where(and);

            query.TransformUsing(Transformers.AliasToBean<Registro50ComprasDto>());

            var lista = query.List<Registro50ComprasDto>();

            return lista;
        }

        public IList<Registro53ComprasDto> BuscarRegistro53ComprasDtos(DateTime filtroDataInicio, DateTime filtroDataFinal, EmpresaDTO empresa)
        {
            NotaFiscalCompra nf = null;

            ItemCompra itemCompra = null;
            IcmsCompra icmsCompra = null;
            TributacaoCst tributacaoCst = null;
            CfopDTO cfop = null;

            Fornecedor forn = null;
            PessoaEntidade pf = null;
            PessoaEndereco endereco = null;
            CidadeDTO cidade = null;
            Registro53ComprasDto resultado = null;

            var subQuerySiglaUf = QueryOver.Of<PessoaEntidade>()
                .JoinAlias(xE => xE.Enderecos, () => endereco, JoinType.InnerJoin)
                .JoinAlias(() => endereco.Cidade, () => cidade, JoinType.InnerJoin)
                .Where(i => i.Id == pf.Id).SelectList(list =>
                    list.SelectGroup(() => cidade.SiglaUf).WithAlias(() => resultado.SiglaUf)).Take(1);

            var query = Sessao.QueryOver(() => nf)
                .JoinAlias(() => nf.Fornecedor, () => forn)
                .JoinAlias(() => forn.Pessoa, () => pf)
                .JoinAlias(() => NotaFiscalCompra.Expressions.Itens, () => itemCompra)
                .JoinAlias(() => itemCompra.Icms, () => icmsCompra)
                .JoinAlias(() => icmsCompra.Icms, () => tributacaoCst)
                .JoinAlias(() => itemCompra.Cfop, () => cfop)
                .SelectList(list =>
                    list
                        .SelectGroup(() => nf.Id)
                        .SelectGroup(() => pf.Id)
                        .SelectGroup(() => pf.Cpf.Valor).WithAlias(() => resultado.Cpf)
                        .SelectGroup(() => pf.Cnpj.Valor).WithAlias(() => resultado.Cnpj)
                        .SelectGroup(() => pf.InscricaoEstadual).WithAlias(() => resultado.InscricaoEstadual)
                        .SelectGroup(() => nf.EntradaSaidaEm).WithAlias(() => resultado.LancamentoEm)
                        .SelectSubQuery(subQuerySiglaUf).WithAlias(() => resultado.SiglaUf)
                        .SelectGroup(() => nf.Serie).WithAlias(() => resultado.Serie)
                        .SelectGroup(() => nf.NumeroDocumento).WithAlias(() => resultado.Numero)
                        .SelectGroup(() => cfop.Id).WithAlias(() => resultado.Cfop)
                        .SelectSum(() => itemCompra.ValorTotal).WithAlias(() => resultado.ValorTotal)
                        .SelectSum(() => icmsCompra.BaseCalculo).WithAlias(() => resultado.BaseCalculoIcmsSt)
                        .SelectGroup(() => tributacaoCst.Id).WithAlias(() => resultado.Cst)
                        .SelectGroup(() => nf.Chave.Chave).WithAlias(() => resultado.ChaveNfe));



            var or = Restrictions.Disjunction();

            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoCst.Id), "10"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoCst.Id), "30"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoCst.Id), "60"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoCst.Id), "70"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoCst.Id), "90"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoCst.Id), "201"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoCst.Id), "202"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoCst.Id), "203"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoCst.Id), "500"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoCst.Id), "900"));

            var periodo = new FiltroPeriodo(filtroDataInicio, filtroDataFinal);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => nf.EntradaSaidaEm));

            var empresaIgual = Restrictions.Eq(Projections.Property(() => nf.Empresa), empresa);

            var and = Restrictions.Conjunction();

            and.Add(intervaloMensal);
            and.Add(empresaIgual);

            query.Where(and);
            query.Where(or);


            query.TransformUsing(Transformers.AliasToBean<Registro53ComprasDto>());

            var lista = query.List<Registro53ComprasDto>();

            return lista;
        }

        public IList<Registro54ComprasDto> BuscarRegistro54ComprasDto(DateTime filtroDataInicio, DateTime filtroDataFinal, EmpresaDTO empresa)
        {
            NotaFiscalCompra nf = null;

            ItemCompra itemCompra = null;
            ProdutoDTO produto = null;
            IcmsCompra icmsCompra = null;
            IpiCompra ipiCompra = null;
            TributacaoCst tributacaoCst = null;
            CfopDTO cfop = null;

            Fornecedor forn = null;
            PessoaEntidade pf = null;
            Registro54ComprasDto resultado = null;

            var query = Sessao.QueryOver(() => nf)
                .JoinAlias(() => nf.Fornecedor, () => forn, JoinType.InnerJoin)
                .JoinAlias(() => forn.Pessoa, () => pf, JoinType.InnerJoin)
                .JoinAlias(() => NotaFiscalCompra.Expressions.Itens, () => itemCompra, JoinType.InnerJoin)
                .JoinAlias(() => itemCompra.Produto, () => produto, JoinType.InnerJoin)
                .JoinAlias(() => itemCompra.Icms, () => icmsCompra, JoinType.InnerJoin)
                .JoinAlias(() => icmsCompra.Icms, () => tributacaoCst, JoinType.InnerJoin)
                .JoinAlias(() => itemCompra.Cfop, () => cfop, JoinType.InnerJoin)
                .JoinAlias(() => itemCompra.Ipi, () => ipiCompra, JoinType.InnerJoin)
                .SelectList(list =>
                    list
                        .Select(() => nf.Id)
                        .Select(() => pf.Cpf.Valor).WithAlias(() => resultado.Cpf)
                        .Select(() => pf.Cnpj.Valor).WithAlias(() => resultado.Cnpj)
                        .Select(() => nf.Serie).WithAlias(() => resultado.Serie)
                        .Select(() => nf.NumeroDocumento).WithAlias(() => resultado.Numero)
                        .Select(() => cfop.Id).WithAlias(() => resultado.Cfop)
                        .Select(() => tributacaoCst.Id).WithAlias(() => resultado.Cst)
                        .Select(() => itemCompra.Id).WithAlias(() => resultado.NumeroItem)
                        .Select(() => itemCompra.Quantidade).WithAlias(() => resultado.Quantidade)
                        .Select(() => itemCompra.ValorTotal).WithAlias(() => resultado.ValorProduto)
                        .Select(() => itemCompra.ValorDescontoTotal).WithAlias(() => resultado.ValorDescontoTotal)
                        .Select(() => icmsCompra.BaseCalculo).WithAlias(() => resultado.BaseCalculoIcms)
                        .Select(() => icmsCompra.BaseCalculoSt).WithAlias(() => resultado.BaseCalculoIcmsSt)
                        .Select(() => ipiCompra.ValorIpi).WithAlias(() => resultado.ValorIpi)
                        .Select(() => icmsCompra.Aliquota).WithAlias(() => resultado.AliquotaIcms)
                        .Select(() => produto.Id).WithAlias(() => resultado.CodigoProdutoOuServico)
                        .Select(() => nf.Chave.Chave).WithAlias(() => resultado.ChaveNfe));


            var periodo = new FiltroPeriodo(filtroDataInicio, filtroDataFinal);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => nf.EntradaSaidaEm));

            var empresaIgual = Restrictions.Eq(Projections.Property(() => nf.Empresa), empresa);

            var and = Restrictions.Conjunction();

            and.Add(intervaloMensal);
            and.Add(empresaIgual);

            query.Where(and);

            query.TransformUsing(Transformers.AliasToBean<Registro54ComprasDto>());

            var lista = query.List<Registro54ComprasDto>();

            return lista;
        }

        public IList<Registro75Dto> BuscarRegistros75(DateTime filtroDataInicio, DateTime filtroDataFinal, EmpresaDTO empresa)
        {
            NotaFiscalCompra notaFiscalCompra = null;
            ItemCompra itemCompra = null;
            ProdutoDTO produto = null;
            ProdutoUnidadeDTO produtoUnidade = null;
            Registro75Dto registro75Dto = null;

            var query = Sessao.QueryOver(() => notaFiscalCompra)
                .JoinAlias(() => NotaFiscalCompra.Expressions.Itens, () => itemCompra, JoinType.InnerJoin)
                .JoinAlias(() => itemCompra.Produto, () => produto, JoinType.InnerJoin)
                .JoinAlias(() => produto.ProdutoUnidadeDTO, () => produtoUnidade, JoinType.InnerJoin)
                .SelectList(
                    list => list.SelectGroup(() => produto.Id).WithAlias(() => registro75Dto.CodigoProdutoServico)
                        .SelectGroup(() => produto.Ncm).WithAlias(() => registro75Dto.CodigoNcm)
                        .SelectGroup(() => produto.Nome).WithAlias(() => registro75Dto.Descricao)
                        .SelectGroup(() => produtoUnidade.Sigla).WithAlias(() => registro75Dto.UnidadeMedida)
                        .SelectGroup(() => produto.AliquotaIpi).WithAlias(() => registro75Dto.AliquotaIpi)
                        .SelectGroup(() => produto.AliquotaIcms).WithAlias(() => registro75Dto.AliquotaIcms)
                        .SelectGroup(() => produto.ReducaoIcms).WithAlias(() => registro75Dto.ReducaoBaseCalculoIcms)
                );

            var periodo = new FiltroPeriodo(filtroDataInicio, filtroDataFinal);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => notaFiscalCompra.EntradaSaidaEm));

            var empresaIgual = Restrictions.Eq(Projections.Property(() => notaFiscalCompra.Empresa), empresa);

            var and = Restrictions.Conjunction();

            and.Add(intervaloMensal);
            and.Add(empresaIgual);

            query.Where(and);


            query.TransformUsing(Transformers.AliasToBean<Registro75Dto>());

            var lista = query.List<Registro75Dto>();

            return lista;
        }

        public bool JaExiste(NotaFiscalCompra nota)
        {
            NotaFiscalCompra nf = null;

            var condicao1 = Restrictions.Conjunction();

            condicao1.Add(Restrictions.Eq(Projections.Property(() => nf.Serie), nota.Serie));
            condicao1.Add(Restrictions.Eq(Projections.Property(() => nf.NumeroDocumento), nota.NumeroDocumento));
            condicao1.Add(Restrictions.Eq(Projections.Property(() => nf.Fornecedor.Id), nota.Fornecedor.Id));

            var condicao2 = Restrictions.Disjunction();

            if (nota.PossuiChave)
                condicao2.Add(Restrictions.Eq(Projections.Property(() => nf.Chave), nota.Chave));

            condicao2.Add(condicao1);

            var condicaoFinal = Restrictions.Conjunction();

            condicaoFinal.Add(condicao2);
            condicaoFinal.Add(Restrictions.Not(Restrictions.Eq(Projections.Property(() => nf.Id), nota.Id)));

            var query = Sessao.QueryOver(() => nf).Where(condicaoFinal);

            return query.RowCount() > 0;
        }

        public bool JaExisteChaveIgual(string chave)
        {
            var query = Sessao.QueryOver<NotaFiscalCompra>().Where(n => n.Chave.Chave == chave);

            return query.RowCount() > 0;
        }

        public void Deleta(NotaFiscalCompra nota)
        {
            Sessao.Delete(nota);
            Sessao.Flush();
        }
    }
}