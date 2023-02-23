using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using NHibernate;
using NHibernate.Linq;
using EmissorClasse = FusionCore.FusionAdm.Emissores.EmissorFiscal;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Emissores
{
    public class BuscaRapidaDeEmissorFiscal : IBuscaListagem<EmissorClasse>
    {
        private readonly string _filtro;

        public BuscaRapidaDeEmissorFiscal(string filtro)
        {
            _filtro = filtro ?? string.Empty;
        }

        public IList<EmissorClasse> Busca(ISession sessao)
        {
            var query = sessao.Query<EmissorClasse>()
                .Where(emissorFiscal =>
                    emissorFiscal.Id.ToString().Equals(_filtro)
                    || emissorFiscal.Descricao.Contains(_filtro));

            return query.ToList();
        }
    }
}