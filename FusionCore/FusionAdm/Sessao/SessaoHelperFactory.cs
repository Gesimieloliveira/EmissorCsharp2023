using FusionCore.Setup;
using NHibernate;

namespace FusionCore.FusionAdm.Sessao
{
    public static class SessaoHelperFactory
    {
        private static SessaoHelperAdm _adm;
        private static SessaoHelperRelatorioFusion _relatorioFusion;

        public static ISession AbrirSessaoAdm()
        {
            return GetAdm().AbrirSessao();
        }

        public static IStatelessSession SessaoStatelessAdm()
        {
            return GetAdm().AbrStatelessSession();
        }

        public static ISession AbrirSessaoRelatorioFusion()
        {
            return GetRelatorioFusion().AbrirSessao();
        }

        public static string GetConnectionString()
        {
            return GetAdm().ConnectionString;
        }

        private static SessaoHelperAdm GetAdm()
        {
            if (_adm != null && _adm.IsOpen)
            {
                return _adm;
            }

            CarregarAdm();

            return _adm;
        }

        public static IConexaoCfg GetConexaoCfg()
        {
            return GetAdm().GetConexaoCfg();
        }

        public static void CarregarAdm()
        {
            _adm?.Fechar();
            _adm = new SessaoHelperAdm();
        }

        private static SessaoHelperRelatorioFusion GetRelatorioFusion()
        {
            if (_relatorioFusion != null)
            {
                return _relatorioFusion;
            }

            _relatorioFusion?.Fechar();
            _relatorioFusion = new SessaoHelperRelatorioFusion();

            return _relatorioFusion;
        }

        public static void Fechar()
        {
            _adm?.Fechar();
            _relatorioFusion?.Fechar();

            _adm = null;
            _relatorioFusion = null;
        }
    }
}