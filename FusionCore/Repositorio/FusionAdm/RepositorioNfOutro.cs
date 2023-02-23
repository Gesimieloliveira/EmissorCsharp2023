using System.Collections.Generic;
using FusionCore.FusionAdm.EntradaOutras;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Sintegra.Dto;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioNfOutro : Repositorio<NfOutro, int>
    {
        public RepositorioNfOutro(ISession sessao) : base(sessao)
        {
        }

        public void SalvarOuAtualizar(NfOutro nfOutro)
        {
            Sessao.SaveOrUpdate(nfOutro);
        }

        public IList<NfOutroDto> Buscar(string texto)
        {
            NfOutro nfOutro = null;
            Fornecedor fornecedor = null;
            PessoaEntidade pessoaEntidade = null;
            NfOutroDto dto = null;


            var queryOver = Sessao.QueryOver(() => nfOutro)
                .JoinAlias(() => nfOutro.Fornecedor, () => fornecedor)
                .JoinAlias(() => fornecedor.Pessoa, () => pessoaEntidade)
                .SelectList(list => list.Select(() => nfOutro.Id).WithAlias(() => dto.Id)
                    .Select(() => pessoaEntidade.Nome).WithAlias(() => dto.NomeFornecedor)
                    .Select(() => nfOutro.EmissaoEm).WithAlias(() => dto.EmissaoEm)
                    .Select(() => nfOutro.RecebimentoEm).WithAlias(() => dto.RecebimentoEm)
                    .Select(() => nfOutro.ModeloDocumento).WithAlias(() => dto.ModeloDocumentoOutro)
                    .Select(() => nfOutro.Serie).WithAlias(() => dto.Serie)
                    .Select(() => nfOutro.Numero).WithAlias((() => dto.Numero)));

            queryOver.TransformUsing(Transformers.AliasToBean<NfOutroDto>());


            if (texto.IsNotNullOrEmpty())
            {
                var likeNomeFornecedor = Restrictions.Like(Projections.Property(() => pessoaEntidade.Nome), texto, MatchMode.Anywhere);

                queryOver.Where(likeNomeFornecedor);
            }

            var lista = queryOver.OrderBy(() => nfOutro.Id).Desc.List<NfOutroDto>();

            return lista;

        }

        public NfOutro BuscarPorIdLazy(int id)
        {
            var nfOutro = GetPeloId(id);

            if (nfOutro.Fornecedor.Enderecos != null)
            {
                NHibernateUtil.Initialize(nfOutro.Fornecedor.Enderecos);
            }

            if (nfOutro.Fornecedor.Telefones != null)
            {
                NHibernateUtil.Initialize(nfOutro.Fornecedor.Telefones);
            }

            return nfOutro;
        }

        public void Deletar(NfOutro nfOutro)
        {
            Sessao.Delete(nfOutro);
        }
    }
}