using FusionCore.ManifestoSefaz;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioManifestoDfe : Repositorio<ManifestoDfe, int>
    {
        public RepositorioManifestoDfe(ISession sessao) : base(sessao)
        {
        }

        public void Salva(ManifestoDfe manifesto)
        {
            if (manifesto.Id == 0)
            {
                Sessao.Save(manifesto);
                Sessao.Flush();
                return;
            }

            Sessao.Update(manifesto);
            Sessao.Flush();;
        }

        public ManifestoDfe GetManifesto(string chave, TipoManifesto tipo)
        {
            var query = Sessao.QueryOver<ManifestoDfe>()
                .Where(c => c.Chave == chave && c.Tipo == tipo)
                .Take(1);

            return query.SingleOrDefault();
        }
    }
}
