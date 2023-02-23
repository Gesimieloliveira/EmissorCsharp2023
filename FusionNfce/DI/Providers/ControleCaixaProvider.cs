using FusionCore.CadastroEmpresa;
using FusionCore.CadastroUsuario;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Servicos;
using FusionCore.DI;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Repositorio.FusionNfce;
using NHibernate;

namespace FusionNfce.DI.Providers
{
    internal class ControleCaixaProvider : IControleCaixaProvider
    {
        private readonly byte _terminalId;

        public ControleCaixaProvider(byte terminalId)
        {
            _terminalId = terminalId;
        }

        public ELocalEventoCaixa GetLocalEvento() => ELocalEventoCaixa.Terminal;

        public IUsuario GetUsuarioLogado()
        {
            return SessaoSistemaNfce.Usuario;
        }

        public IRepositorioEmpresa GetRepositorioEmpresa(ISession session)
        {
            return new RepositorioEmpresaNfce(session);
        }

        public ServicoContaCaixa CriarServicoContaCaixa(ISession session)
        {
            return new ServicoContaCaixa(session);
        }

        public ServicoCaixaIndividual CriarServicoCaixaIndividual(ISession session)
        {
            return new ServicoCaixaIndividual(session, this, _terminalId);
        }
    }
}