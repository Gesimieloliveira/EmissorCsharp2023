using System;
using FusionCore.FusionNfce.Usuario.Papeis;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioPapelNfce : Repositorio<PapelNfce, Guid>
    {
        public RepositorioPapelNfce(ISession sessao) : base(sessao)
        {
        }

        public void PersistirPapel(PapelNfce papel)
        {
            ThrowExceptionSeNaoExisteTransacao();

            Sessao.Persist(papel);
            Sessao.Flush();
        }

        public void DeletarPapel(Guid gid)
        {
            ThrowExceptionSeNaoExisteTransacao();

            var delete = $"from {nameof(PapelNfce)} p where p.{nameof(PapelNfce.Id)} = '{{0}}'";

            delete = string.Format(delete, gid.ToString());

            Sessao.Delete(delete);
            Sessao.Flush();
        }
    }
}