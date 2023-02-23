using NHibernate;

namespace FusionCore.FusionNfce.Preferencias
{
    public class RepositorioPreferencia
    {
        private readonly ISession _session;

        public RepositorioPreferencia(ISession session)
        {
            _session = session;
        }

        public void SalvarAlteracoes(PreferenciaTerminal preferencia)
        {
            _session.SaveOrUpdate(preferencia);
            _session.Flush();
        }

        public PreferenciaTerminal BuscarExistente()
        {
            var unica = _session.Get<PreferenciaTerminal>(PreferenciaTerminal.StaticGuid);

            return unica;
        }
    }
}