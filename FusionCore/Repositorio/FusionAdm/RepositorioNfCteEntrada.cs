using System;
using System.Collections.Generic;
using FusionCore.Extencoes;
using FusionCore.FusionAdm.EntradaOutras;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sintegra.Dto;
using FusionCore.Tributacoes.Estadual;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioNfCteEntrada : Repositorio<NfCteEntrada, int>
    {
        public RepositorioNfCteEntrada(ISession sessao) : base(sessao)
        {
        }

        public void SalvarOuAtualizar(NfCteEntrada nfCteEntrada)
        {
            Sessao.SaveOrUpdate(nfCteEntrada);
        }

        public IList<NfCteEntradaDto> Buscar(string texto)
        {
            NfCteEntrada nfCteEntrada = null;
            EmpresaDTO empresa = null;
            NfCteEntradaDto cteEntradaDto = null;
            CfopDTO cfop = null;

            var queryOver = Sessao.QueryOver(() => nfCteEntrada)
                .JoinAlias(() => nfCteEntrada.EmpresaTomador, () => empresa)
                .JoinAlias(() => nfCteEntrada.Cfop, () => cfop)
                .SelectList(list => list.Select(() => nfCteEntrada.Id).WithAlias(() => cteEntradaDto.Id)
                    .Select(() => empresa.RazaoSocial).WithAlias(() => cteEntradaDto.NomeEmpresa)
                    .Select(() => empresa.Cnpj).WithAlias(() => cteEntradaDto.CnpjEmpresa)
                    .Select(() => nfCteEntrada.ModeloDocumento).WithAlias(() => cteEntradaDto.ModeloDocumento)
                    .Select(() => nfCteEntrada.SituacaoFiscal).WithAlias(() => cteEntradaDto.SituacaoFiscal)
                    .Select(() => nfCteEntrada.EmissaoEm).WithAlias(() => cteEntradaDto.EmissaoEm)
                    .Select(() => nfCteEntrada.UtilizacaoEm).WithAlias(() => cteEntradaDto.UtilizacaoEm)
                    .Select(() => nfCteEntrada.Serie).WithAlias(() => cteEntradaDto.Serie)
                    .Select(() => nfCteEntrada.Subserie).WithAlias(() => cteEntradaDto.Subserie)
                    .Select(() => nfCteEntrada.Numero).WithAlias(() => cteEntradaDto.Numero)
                    .Select(() => cfop.Id).WithAlias(() => cteEntradaDto.Cfop));


            if (texto.IsNotNullOrEmpty())
            {
                var condicaoOr = Restrictions.Disjunction();

                var likeRazaoSocial = Restrictions.Like(Projections.Property(() => empresa.RazaoSocial),
                    texto,
                    MatchMode.Anywhere);

                var likeNomeFantasia = Restrictions.Like(Projections.Property(() => empresa.NomeFantasia),
                    texto,
                    MatchMode.Anywhere);

                var equalCnpj = Restrictions.Like(Projections.Property(() => empresa.Cnpj),
                    texto,
                    MatchMode.Anywhere);


                condicaoOr.Add(likeRazaoSocial);
                condicaoOr.Add(likeNomeFantasia);
                condicaoOr.Add(equalCnpj);

                queryOver.Where(condicaoOr);
            }

            queryOver.TransformUsing(Transformers.AliasToBean<NfCteEntradaDto>());

            var lista = queryOver.List<NfCteEntradaDto>();

            return lista;
        }

        public IList<Registro70CteEntradaDto> BuscarRegistro70CteEntradaDtos(DateTime filtroDataInicio, DateTime filtroDataFinal, EmpresaDTO filtroEmpresa)
        {
            NfCteEntrada nfCteEntrada = null;
            EmpresaDTO empresa = null;
            CidadeDTO cidade = null;
            Registro70CteEntradaDto cteEntradaDto = null;
            TributacaoCst tributacaoCst = null;

            CfopDTO cfop = null;

            var queryOver = Sessao.QueryOver(() => nfCteEntrada)
                .JoinAlias(() => nfCteEntrada.EmpresaTomador, () => empresa, JoinType.InnerJoin)
                .JoinAlias(() => empresa.CidadeDTO, () => cidade, JoinType.InnerJoin)
                .JoinAlias(() => nfCteEntrada.Cfop, () => cfop, JoinType.InnerJoin)
                .JoinAlias(() => nfCteEntrada.IcmsCst, () => tributacaoCst, JoinType.InnerJoin)
                .SelectList(list => list.Select(() => nfCteEntrada.Id)
                    .Select(() => empresa.Cnpj).WithAlias(() => cteEntradaDto.DocumentoUnico)
                    .Select(() => empresa.InscricaoEstadual).WithAlias(() => cteEntradaDto.InscricaoEstadual)
                    .Select(() => nfCteEntrada.ModeloDocumento).WithAlias(() => cteEntradaDto.ModeloDocumento)
                    .Select(() => nfCteEntrada.EmissaoEm).WithAlias(() => cteEntradaDto.DataEmissao)
                    .Select(() => cidade.SiglaUf).WithAlias(() => cteEntradaDto.SiglaUf)
                    .Select(() => nfCteEntrada.Serie).WithAlias(() => cteEntradaDto.Serie)
                    .Select(() => nfCteEntrada.Subserie).WithAlias(() => cteEntradaDto.Subserie)
                    .Select(() => nfCteEntrada.Numero).WithAlias(() => cteEntradaDto.Numero)
                    .Select(() => cfop.Id).WithAlias(() => cteEntradaDto.Cfop)
                    .Select(() => nfCteEntrada.ValorTotal).WithAlias(() => cteEntradaDto.ValorTotalDocumentoFiscal)
                    .Select(() => nfCteEntrada.BaseCalculoIcms).WithAlias(() => cteEntradaDto.BaseCalculoIcms)
                    .Select(() => nfCteEntrada.ValorIcms).WithAlias(() => cteEntradaDto.ValorIcms)
                    .Select(() => nfCteEntrada.SituacaoFiscal).WithAlias(() => cteEntradaDto.Situacao)
                    .Select(() => tributacaoCst.Id).WithAlias(() => cteEntradaDto.Cst)
                );


            var intervaloMensal =
                Restrictions.Between(Projections.Property(() => nfCteEntrada.EmissaoEm), filtroDataInicio, filtroDataFinal);

            var empresaIgual = Restrictions.Eq(Projections.Property(() => nfCteEntrada.EmpresaTomador), filtroEmpresa);

            var and = Restrictions.Conjunction();

            and.Add(intervaloMensal);
            and.Add(empresaIgual);

            queryOver.Where(and);
            queryOver.TransformUsing(Transformers.AliasToBean<Registro70CteEntradaDto>());

            var lista = queryOver.OrderByAlias(() => nfCteEntrada.Id).Asc.List<Registro70CteEntradaDto>();

            return lista;
        }



        public void Deletar(NfCteEntrada nfCteEntrada)
        {
            Sessao.Delete(nfCteEntrada);
        }
    }
}