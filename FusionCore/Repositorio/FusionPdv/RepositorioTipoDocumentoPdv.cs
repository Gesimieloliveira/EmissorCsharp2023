using System.Collections.Generic;
using FusionCore.FusionPdv.Financeiro;
using FusionCore.Repositorio.Contratos.FusionPdvContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

// ReSharper disable RedundantBoolCompare

namespace FusionCore.Repositorio.FusionPdv
{
    public class RepositorioTipoDocumentoPdv : Repositorio<TipoDocumentoPdv, short>, IRepositorioTipoDocumentoPdv
    {
        public RepositorioTipoDocumentoPdv(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(TipoDocumentoPdv tipoDocumento)
        {
            Sessao.SaveOrUpdate(tipoDocumento);
        }

        public IEnumerable<TipoDocumentoPdv> BuscaAtivos()
        {
            var queryOver = Sessao.QueryOver<TipoDocumentoPdv>().Where(td => td.EstaAtivo == true);
            var lista = queryOver.List<TipoDocumentoPdv>();

            return lista;
        }
    }
}