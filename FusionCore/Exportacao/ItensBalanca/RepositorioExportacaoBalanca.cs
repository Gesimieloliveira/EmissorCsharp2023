using System.Collections.Generic;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Transform;

namespace FusionCore.Exportacao.ItensBalanca
{
    public class RepositorioExportacaoBalanca
    {
        private readonly IStatelessSession _session;
        private readonly ModeloItem _item = null;

        public RepositorioExportacaoBalanca(IStatelessSession session)
        {
            _session = session;
        }

        public IList<PreferenciaExportacao> BuscaPreferencias(string idMaquina, string tag)
        {
            var query = _session.QueryOver<PreferenciaExportacao>()
                .Where(i => i.Identificador == idMaquina && i.Tag == tag);

            return query.List();
        }

        public IEnumerable<ModeloItem> BuscaTodos()
        {
            var query = _session.QueryOver<ProdutoDTO>()
                .SelectList(list => list
                    .Select(p => p.CodigoBalanca).WithAlias(() => _item.Codigo)
                    .Select(p => p.Nome).WithAlias(() => _item.Descricao)
                    .Select(p => p.PrecoVenda).WithAlias(() => _item.Preco)
                );

            query.Where(i => i.CodigoBalanca > 0);

            query.TransformUsing(Transformers.AliasToBean<ModeloItem>());

            return query.List<ModeloItem>();
        }
    }
}