using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Contratos;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Filtros;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioDocumentoPagar : Repositorio<DocumentoPagar, int>, IRepositorioDocumentoPagar
    {
        public RepositorioDocumentoPagar(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(DocumentoPagar documentoPagar)
        {
            if (documentoPagar.Id == 0)
            {
                Sessao.Persist(documentoPagar);
                Sessao.Flush();

                foreach (var la in documentoPagar.Lancamentos)
                {
                    Sessao.Persist(la);
                }

                Sessao.Flush();
                return;
            }

            Sessao.Update(documentoPagar);
            Sessao.Flush();

            foreach (var la in documentoPagar.Lancamentos)
            {
                if (la.Id == 0)
                {
                    Sessao.Persist(la);
                    continue;
                }

                Sessao.Update(la);
            }

            Sessao.Flush();
        }

        public IList<DocumentoPagar> BuscaRapida(string texto)
        {
            DocumentoPagar documentoPagar = null;
            var queryOver =
                Sessao.QueryOver(() => documentoPagar).WhereRestrictionOn(dR => dR.Descricao).IsLike("%" + texto + "%");

            return queryOver.List();
        }

        public void SalvarLancamento(DocumentoPagarLancamento documentoPagarLancamento)
        {
            Sessao.SaveOrUpdate(documentoPagarLancamento);
        }

        public IList<DocumentoPagarDTO> BuscaParaFaturamento(FiltroDocumentoReceberFaturamento filtro)
        {
            DocumentoPagarDTO resultado = null;
            DocumentoPagar documentoPagar = null;
            PessoaEntidade pessoa = null;
            Cliente fornecedor = null;
            EmpresaDTO empresa = null;

            var queryOver =
                Sessao.QueryOver(() => documentoPagar)
                    .Inner.JoinAlias(() => documentoPagar.Fornecedor, () => fornecedor)
                    .Inner.JoinAlias(() => fornecedor.Pessoa, () => pessoa)
                    .Inner.JoinAlias(() => documentoPagar.Empresa, () => empresa)
                    .SelectList(list => list
                        .Select(() => documentoPagar.Id).WithAlias(() => resultado.Id)
                        .Select(() => documentoPagar.Situacao).WithAlias(() => resultado.Situacao)
                        .Select(() => documentoPagar.NumeroDocumento).WithAlias(() => resultado.Numero)
                        .Select(() => pessoa.Nome).WithAlias(() => resultado.NomeFornecedor)
                        .Select(() => documentoPagar.Vencimento).WithAlias(() => resultado.VencimentoEm)
                        .Select(() => documentoPagar.Parcela).WithAlias(() => resultado.Parcela)
                        .Select(() => documentoPagar.ValorOriginal).WithAlias(() => resultado.ValorOriginal)
                        .Select(() => documentoPagar.ValorAjustado).WithAlias(() => resultado.ValorAjustado)
                        .Select(() => documentoPagar.Juros).WithAlias(() => resultado.Juros)
                        .Select(() => documentoPagar.Desconto).WithAlias(() => resultado.Desconto)
                        .Select(() => documentoPagar.ValorQuitado).WithAlias(() => resultado.ValorQuitado)
                        .Select(() => empresa.RazaoSocial).WithAlias(() => resultado.NomeEmpresa)
                        .Select(() => documentoPagar.Descricao).WithAlias(() => resultado.Descricao)
                    );


            if (filtro.Situacao != null)
            {
                queryOver.Where(Restrictions.Eq(Projections.Property(() => documentoPagar.Situacao), filtro.Situacao));
            }

            if (filtro.PessoaId != 0)
            {
                queryOver.Where(Restrictions.Eq(Projections.Property(() => pessoa.Id), filtro.PessoaId));
            }

            if (filtro.Empresa != null)
            {
                queryOver.Where(Restrictions.Eq(Projections.Property(() => empresa.Id), filtro.Empresa.Id));
            }

            if (filtro.NumeroDocumento.IsNotNullOrEmpty())
            {
                queryOver.Where(Restrictions.Eq(Projections.Property(() => documentoPagar.NumeroDocumento), filtro.NumeroDocumento));
            }


            if (filtro.DataVencimentoInicial != null && filtro.DataVencimentoFinal != null)
            {
                queryOver.Where(Restrictions.Between(Projections.Property(() => documentoPagar.Vencimento),
                    filtro.DataVencimentoInicial,
                    filtro.DataVencimentoFinal));
            }

            if (filtro.DataVencimentoInicial != null && filtro.DataVencimentoFinal == null)
            {
                queryOver.Where(Restrictions.Ge(Projections.Property(() => documentoPagar.Vencimento), filtro.DataVencimentoInicial));
            }


            queryOver.TransformUsing(Transformers.AliasToBean<DocumentoPagarDTO>());


            return queryOver.OrderBy(d => d.Id).Desc.OrderBy(d => d.Parcela).Asc.List<DocumentoPagarDTO>();
        }

        public IList<DocumentoPagar> BuscarDocumentoPagarDeMalote(int maloteId)
        {
            var queryOver = Sessao.QueryOver<DocumentoPagar>().Where(docPag => docPag.Malote.Id == maloteId);
            var documentosAPagar = queryOver.List<DocumentoPagar>();

            return documentosAPagar;
        }

        public IList<DocumentoPagar> BuscarDocumentoPagarDeMalote(Malote malote)
        {
            return BuscarDocumentoPagarDeMalote(malote.Id);
        }

        public DocumentoPagarLancamento BuscarLancamento(int lancamentoId)
        {
            var lancamento = Sessao.Get<DocumentoPagarLancamento>(lancamentoId);
            NHibernateUtil.Initialize(lancamento.DocumentoPagar);
            return lancamento;
        }
    }
}