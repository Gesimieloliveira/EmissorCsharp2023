using System.Collections.Generic;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.Repositorio.Filtros;
using NHibernate;
using NHibernate.Transform;

namespace FusionCore.FusionAdm.Financeiro.Repositorios
{
    public class RepositorioDocumentoReceber
    {
        private readonly ISession _session;

        public RepositorioDocumentoReceber(ISession session)
        {
            _session = session;
        }

        public void SalvarAlteracoes(DocumentoReceber documento)
        {
            if (documento.Id == 0)
            {
                PersitirMalote(documento);

                _session.Persist(documento);
                _session.Flush();

                return;
            }

            _session.Update(documento);
            _session.Flush();
        }

        private void PersitirMalote(DocumentoReceber documento)
        {
            if (documento.Malote.Id != 0)
            {
                return;
            }

            _session.Persist(documento.Malote);
            _session.Flush();
        }

        public IEnumerable<ResumoDocumentoReceberDTO> BuscarDocumentos(IFiltro filtro)
        {
            DocumentoReceber tbReceber = null;
            PessoaEntidade tbPessoa = null;
            Cliente tbCliente = null;

            ResumoDocumentoReceberDTO ass = null;

            var query = _session.QueryOver(() => tbReceber)
                .Inner.JoinAlias(() => tbReceber.Cliente, () => tbCliente)
                .Inner.JoinAlias(() => tbCliente.Pessoa, () => tbPessoa)
                .SelectList(list => list
                    .Select(() => tbReceber.Id).WithAlias(() => ass.Id)
                    .Select(() => tbReceber.Malote.Id).WithAlias(() => ass.MaloteId)
                    .Select(() => tbReceber.Situacao).WithAlias(() => ass.Situacao)
                    .Select(() => tbPessoa.Id).WithAlias(() => ass.PessoaId)
                    .Select(() => tbPessoa.Nome).WithAlias(() => ass.NomePessoa)
                    .Select(() => tbReceber.EmitidoEm).WithAlias(() => ass.EmitidoEm)
                    .Select(() => tbReceber.Vencimento).WithAlias(() => ass.Vencimento)
                    .Select(() => tbReceber.Parcela).WithAlias(() => ass.Parcela)
                    .Select(() => tbReceber.ValorOriginal).WithAlias(() => ass.ValorOriginal)
                    .Select(() => tbReceber.ValorDocumento).WithAlias(() => ass.ValorDocumento)
                    .Select(() => tbReceber.ValorQuitado).WithAlias(() => ass.ValorRecebido)
                    .Select(() => tbReceber.UltimoCalculoJuros).WithAlias(() => ass.UltimoCalculoJuros)
                    .Select(() => tbReceber.TotalJuros).WithAlias(() => ass.TotalJuros)
                    .Select(() => tbReceber.TotalDesconto).WithAlias(() => ass.TotalDesconto)
                    .Select(() => tbReceber.DataQuitacao).WithAlias(() => ass.DataQuitacao)
                    .Select(() => tbReceber.Descricao).WithAlias(() => ass.Descricao)
                );

            query.TransformUsing(Transformers.AliasToBean<ResumoDocumentoReceberDTO>());

            filtro?.Aplicar(query);

            return query.List<ResumoDocumentoReceberDTO>();
        }

        public DocumentoReceber BuscarPeloId(int id)
        {
            var documento = _session.Get<DocumentoReceber>(id);

            return documento;
        }

        public IList<DocumentoReceber> BuscarPeloMaloteId(int idMalote)
        {
            DocumentoReceber documentoReceber = null;
            Malote malote = null;

            var queryOver = _session.QueryOver(() => documentoReceber).Inner.JoinAlias(() => documentoReceber.Malote, () => malote).Where(dr => malote.Id == idMalote);

            var lista = queryOver.List();

            return lista;
        }

        public IList<DocumentoReceber> BuscarPeloMalote(Malote malote)
        {
            var query = _session.QueryOver<DocumentoReceber>()
                .Where(i => i.Malote == malote);

            return query.List();
        }
    }
}