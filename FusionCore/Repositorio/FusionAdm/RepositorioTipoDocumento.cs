using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Financeiro;
using FusionCore.FusionPdv.Financeiro;
using NHibernate;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioTipoDocumento : Repositorio<TipoDocumento, short>
    {
        public RepositorioTipoDocumento(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(TipoDocumento tipoDocumento)
        {
            Sessao.SaveOrUpdate(tipoDocumento);
        }

        public void Deletar(TipoDocumento tipoDocumento)
        {
            Sessao.Delete(tipoDocumento);
        }

        public IList<TipoDocumento> BuscarTipoDocumentoGrid()
        {
            TipoDocumento tipoDocumento = null;

            var queryOver = Sessao.QueryOver(() => tipoDocumento)
                .SelectList(list => list.Select(() => tipoDocumento.Id).WithAlias(() => tipoDocumento.Id)
                    .Select(() => tipoDocumento.Descricao).WithAlias(() => tipoDocumento.Descricao)
                    .Select(() => tipoDocumento.EstaAtivo).WithAlias(() => tipoDocumento.EstaAtivo))
                .TransformUsing(Transformers.AliasToBean(typeof(TipoDocumento)));

            var lista = queryOver.List<TipoDocumento>();

            return lista;
        }

        public IList<TipoDocumentoPdv> ListaParaSincronizacao(DateTime ultimaSincronizacao)
        {
            TipoDocumento tipoDocumento = null;
            TipoDocumentoPdv resultado = null;

            var queryOver = Sessao.QueryOver(() => tipoDocumento)
                .SelectList(list => list.Select(() => tipoDocumento.Id).WithAlias(() => resultado.Id)
                    .Select(() => tipoDocumento.Descricao).WithAlias(() => resultado.Descricao)
                    .Select(() => tipoDocumento.EstaAtivo).WithAlias(() => resultado.EstaAtivo));

            queryOver.Where(p => p.AlteradoEm >= ultimaSincronizacao);

            queryOver.TransformUsing(Transformers.AliasToBean(typeof(TipoDocumentoPdv)));

            var lista = queryOver.List<TipoDocumentoPdv>();

            return lista;
        }
    }
}