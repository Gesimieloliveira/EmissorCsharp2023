using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.EntradaOutras;
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
    public class RepositorioSintegra : Repositorio<ProdutoDTO, int>
    {
        public RepositorioSintegra(ISession sessao) : base(sessao)
        {
        }

        public IList<Registro50OutrasDto> BuscarRegistros50NfOutrosEntrada(DateTime dataInicio,
            DateTime dataFinal,
            EmpresaDTO empresa)
        {
            NfOutro nfOutro = null;
            Fornecedor fornecedor = null;
            PessoaEntidade pessoaEntidade = null;
            CidadeDTO cidade = null;
            PessoaEndereco endereco = null;
            Registro50OutrasDto registro50OutrasDto = null;
            TributacaoIcms tributacaoIcms = null;
            CfopDTO cfop = null;
            
            var subQuerySiglaUf = QueryOver.Of(() => endereco)
                .JoinAlias(() => endereco.Cidade, () => cidade, JoinType.InnerJoin)
                .Where(e => e.Residente.Id == pessoaEntidade.Id)
                .Select(Projections.Property(() => cidade.SiglaUf)).Take(1);

            var queryOver = Sessao.QueryOver(() => nfOutro)
                .JoinAlias(() => nfOutro.Fornecedor, () => fornecedor, JoinType.InnerJoin)
                .JoinAlias(() => fornecedor.Pessoa, () => pessoaEntidade, JoinType.InnerJoin)
                .JoinAlias(() => nfOutro.Cfop, () => cfop, JoinType.InnerJoin)
                .JoinAlias(() => nfOutro.Cst, () => tributacaoIcms, JoinType.InnerJoin)
                    .SelectList(list => list.Select(() => pessoaEntidade.Cnpj.Valor).WithAlias(() => registro50OutrasDto.Cnpj)
                    .Select(() => pessoaEntidade.Cpf.Valor).WithAlias(() => registro50OutrasDto.Cpf)
                    .Select(() => pessoaEntidade.InscricaoEstadual).WithAlias(() => registro50OutrasDto.InscricaoEstadual)
                    .Select(() => nfOutro.EmissaoEm).WithAlias(() => registro50OutrasDto.EmissaoEm)
                    .Select(() => nfOutro.RecebimentoEm).WithAlias(() => registro50OutrasDto.RecebimentoEm)
                    .SelectSubQuery(subQuerySiglaUf).WithAlias(() => registro50OutrasDto.SiglaUf)
                    .Select(() => nfOutro.ModeloDocumento).WithAlias(() => registro50OutrasDto.ModeloDocumento)
                    .Select(() => nfOutro.Serie).WithAlias(() => registro50OutrasDto.Serie)
                    .Select(() => nfOutro.Numero).WithAlias(() => registro50OutrasDto.Numero)
                    .Select(() => cfop.Id).WithAlias(() => registro50OutrasDto.Cfop)
                    .Select(() => nfOutro.TipoEmitente).WithAlias(() => registro50OutrasDto.TipoEmitente)
                    .Select(() => nfOutro.ValorTotal).WithAlias(() => registro50OutrasDto.ValorTotal)
                    .Select(() => nfOutro.BaseCalculoIcms).WithAlias(() => registro50OutrasDto.BaseCalculo)
                    .Select(() => nfOutro.ValorIcms).WithAlias(() => registro50OutrasDto.ValorIcms)
                    .Select(() => nfOutro.ValorFrete).WithAlias(() => registro50OutrasDto.ValorFrete)
                    .Select(() => nfOutro.ValorDespesasAcessorias).WithAlias(() => registro50OutrasDto.ValorDespesasAcessorias)
                    .Select(() => nfOutro.ValorSeguro).WithAlias(() => registro50OutrasDto.ValorSeguro)
                    .Select(() => nfOutro.SituacaoFiscal).WithAlias(() => registro50OutrasDto.Situacao)
                    .Select(() => tributacaoIcms.Codigo).WithAlias(() => registro50OutrasDto.CodigoCst)
                    );


            var periodo = new FiltroPeriodo(dataInicio, dataFinal);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => nfOutro.RecebimentoEm));

            var empresaIgual = Restrictions.Eq(Projections.Property(() => nfOutro.Empresa), empresa);

            var and = Restrictions.Conjunction();
            
            and.Add(intervaloMensal);
            and.Add(empresaIgual);

            queryOver.Where(and);

            queryOver.TransformUsing(Transformers.AliasToBean<Registro50OutrasDto>());

            var lista = queryOver.List<Registro50OutrasDto>();
            
            return lista;
        }

        public IList<Registro53OutrasDto> BuscarRegistros53NfOutrosEntrada(DateTime dataInicio,
            DateTime dataFinal,
            EmpresaDTO empresa)
        {
            NfOutro nfOutro = null;
            Fornecedor fornecedor = null;
            PessoaEntidade pessoaEntidade = null;
            CidadeDTO cidade = null;
            PessoaEndereco endereco = null;
            Registro53OutrasDto registro53OutrasDto = null;
            TributacaoIcms tributacaoIcms = null;
            CfopDTO cfop = null;


            var subQuerySiglaUf = QueryOver.Of(() => endereco)
                .JoinAlias(() => endereco.Cidade, () => cidade, JoinType.InnerJoin)
                .Where(e => e.Residente.Id == pessoaEntidade.Id)
                .Select(Projections.Property(() => cidade.SiglaUf)).Take(1);
            
            var queryOver = Sessao.QueryOver(() => nfOutro)
                .JoinAlias(() => nfOutro.Fornecedor, () => fornecedor, JoinType.InnerJoin)
                .JoinAlias(() => fornecedor.Pessoa, () => pessoaEntidade, JoinType.InnerJoin)
                .JoinAlias(() => nfOutro.Cfop, () => cfop, JoinType.InnerJoin)
                .JoinAlias(() => nfOutro.Cst, () => tributacaoIcms, JoinType.InnerJoin)
                    .SelectList(list => list.Select(() => pessoaEntidade.Cnpj.Valor).WithAlias(() => registro53OutrasDto.Cnpj)
                    .Select(() => pessoaEntidade.Cpf.Valor).WithAlias(() => registro53OutrasDto.Cpf)
                    .Select(() => pessoaEntidade.InscricaoEstadual).WithAlias(() => registro53OutrasDto.InscricaoEstadual)
                    .Select(() => nfOutro.EmissaoEm).WithAlias(() => registro53OutrasDto.EmissaoEm)
                    .Select(() => nfOutro.RecebimentoEm).WithAlias(() => registro53OutrasDto.RecebimentoEm)
                    .SelectSubQuery(subQuerySiglaUf).WithAlias(() => registro53OutrasDto.SiglaUf)
                    .Select(() => nfOutro.ModeloDocumento).WithAlias(() => registro53OutrasDto.ModeloDocumento)
                    .Select(() => nfOutro.Serie).WithAlias(() => registro53OutrasDto.Serie)
                    .Select(() => nfOutro.Numero).WithAlias(() => registro53OutrasDto.Numero)
                    .Select(() => cfop.Id).WithAlias(() => registro53OutrasDto.Cfop)
                    .Select(() => nfOutro.TipoEmitente).WithAlias(() => registro53OutrasDto.TipoEmitente)
                    .Select(() => nfOutro.ValorTotal).WithAlias(() => registro53OutrasDto.ValorTotal)
                    .Select(() => nfOutro.BaseCalculoIcmsSt).WithAlias(() => registro53OutrasDto.BaseCalculoSt)
                    .Select(() => nfOutro.ValorIcmsSt).WithAlias(() => registro53OutrasDto.ValorIcmsSt)
                    .Select(() => nfOutro.ValorFrete).WithAlias(() => registro53OutrasDto.ValorFrete)
                    .Select(() => nfOutro.ValorDespesasAcessorias).WithAlias(() => registro53OutrasDto.ValorDespesasAcessorias)
                    .Select(() => nfOutro.ValorSeguro).WithAlias(() => registro53OutrasDto.ValorSeguro)
                    .Select(() => nfOutro.SituacaoFiscal).WithAlias(() => registro53OutrasDto.Situacao)
                    .Select(() => tributacaoIcms.Codigo).WithAlias(() => registro53OutrasDto.CodigoCst)
                    );

            
            var or = Restrictions.Disjunction();

            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoIcms.Codigo), "10"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoIcms.Codigo), "30"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoIcms.Codigo), "60"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoIcms.Codigo), "70"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoIcms.Codigo), "90"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoIcms.Codigo), "201"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoIcms.Codigo), "202"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoIcms.Codigo), "203"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoIcms.Codigo), "500"));
            or.Add(Restrictions.Eq(Projections.Property(() => tributacaoIcms.Codigo), "900"));


            var periodo = new FiltroPeriodo(dataInicio, dataFinal);
            var intervaloMensal = periodo.Restriction(Projections.Property(() => nfOutro.RecebimentoEm));

            var empresaIgual = Restrictions.Eq(Projections.Property(() => nfOutro.Empresa), empresa);
            var modeloDocumentoIgual = Restrictions.Eq(Projections.Property(() => nfOutro.ModeloDocumento),
                ModeloDocumentoOutro.ContaEnergiaEletrica);

            var and = Restrictions.Conjunction();

            and.Add(intervaloMensal);
            and.Add(empresaIgual);
            and.Add(modeloDocumentoIgual);

            queryOver.Where(and);
            queryOver.Where(or);

            queryOver.TransformUsing(Transformers.AliasToBean<Registro53OutrasDto>());

            var lista = queryOver.List<Registro53OutrasDto>();
            
            return lista;
        }
    }
}