using System.Linq;
using FusionCore.Repositorio.Legacy.Base;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;

namespace FusionCore.Repositorio.Legacy.Ativos.Pdv
{
    public class ClienteRepositorio : RepositorioBase<ClienteDt>
    {
        public ClienteRepositorio(ISession sessao) : base(sessao)
        {
        }

        public ClienteDt BuscarPorCpfOuCnpj(string cpfOuCnpj)
        {
            var clientex = (ClienteDt) Sessao.Query<ClienteDt>()
                .Where(cliente => cliente.Cpf.Equals(cpfOuCnpj) || cliente.Cnpj.Equals(cpfOuCnpj))
                .FirstOrNull();

            return clientex;
        }
    }
}