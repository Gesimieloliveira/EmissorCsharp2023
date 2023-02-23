using Fusion.Sessao;
using FusionCore.CadastroEmpresa;
using FusionCore.CadastroUsuario;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Servicos;
using FusionCore.DI;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace Fusion.DI.Providers
{
    public class ControleCaixaProvider : IControleCaixaProvider
    {
        public ELocalEventoCaixa GetLocalEvento() => ELocalEventoCaixa.Gestao;

        public IUsuario GetUsuarioLogado()
        {
            return SessaoSistema.Instancia.UsuarioLogado;
        }

        public IRepositorioEmpresa GetRepositorioEmpresa(ISession session)
        {
            return new RepositorioEmpresa(session);
        }

        public ServicoContaCaixa CriarServicoContaCaixa(ISession session)
        {
            return new ServicoContaCaixa(session);
        }

        public ServicoCaixaIndividual CriarServicoCaixaIndividual(ISession session)
        {
            return new ServicoCaixaIndividual(session, this);
        }
    }
}