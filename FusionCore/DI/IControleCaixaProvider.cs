using FusionCore.CadastroEmpresa;
using FusionCore.CadastroUsuario;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Servicos;
using NHibernate;

namespace FusionCore.DI
{
    public interface IControleCaixaProvider
    {
        ELocalEventoCaixa GetLocalEvento();
        IUsuario GetUsuarioLogado();
        IRepositorioEmpresa GetRepositorioEmpresa(ISession session);
        ServicoContaCaixa CriarServicoContaCaixa(ISession session);
        ServicoCaixaIndividual CriarServicoCaixaIndividual(ISession session);
    }
}