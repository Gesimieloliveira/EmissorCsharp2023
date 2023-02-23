using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Usuario
{
    public class UsuariosParaSincronizacao : IBuscaListagem<UsuarioDTO>
    {
        private readonly DateTime _ultimaAlteracao;

        public UsuariosParaSincronizacao(DateTime ultimaAlteracao)
        {
            _ultimaAlteracao = ultimaAlteracao;
        }

        public IList<UsuarioDTO> Busca(ISession sessao)
        {
            var query = sessao.Query<UsuarioDTO>().Where(u => u.AlteradoEm >= _ultimaAlteracao);
            return query.ToList();
        }
    }
}