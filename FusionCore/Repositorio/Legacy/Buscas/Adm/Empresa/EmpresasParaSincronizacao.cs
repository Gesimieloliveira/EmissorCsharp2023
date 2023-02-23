using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Empresa
{
    public class EmpresasParaSincronizacao : IBuscaListagem<EmpresaDTO>
    {
        private readonly DateTime _ultimaAlteracao;

        public EmpresasParaSincronizacao(DateTime ultimaAlteracao)
        {
            _ultimaAlteracao = ultimaAlteracao;
        }

        public IList<EmpresaDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<EmpresaDTO>()
                .Where(e => e.AlteradoEm >= _ultimaAlteracao);

            return query.ToList();
        }
    }
}