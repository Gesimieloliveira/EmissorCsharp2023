using FusionCore.CadastroUsuario;
using FusionCore.Configuracoes;
using FusionCore.ControleCaixa.Repositorios;
using NHibernate;

namespace FusionCore.ControleCaixa.Servicos
{
    public class ServicoControleCaixaAtivo
    {
        private readonly ISession _session;
        private readonly ELocalEventoCaixa _localEvento;
        private readonly RepositorioCaixaIndividual _repositorio;

        public ServicoControleCaixaAtivo(ISession session, ELocalEventoCaixa localEvento)
        {
            _session = session;
            _localEvento = localEvento;
            _repositorio = new RepositorioCaixaIndividual(session);
        }

        private bool ControleDeCaixaEstaAtivo()
        {
            var repocfg = new RepositorioConfiguracaoCaixa(_session);
            var cfg = repocfg.ObterUnica();

            if (_localEvento == ELocalEventoCaixa.Gestao)
            {
                return cfg.ControlaCaixaNoGestor;
            }

            if (_localEvento == ELocalEventoCaixa.Terminal)
            {
                return cfg.ControlaCaixaNoNfce;
            }

            throw new ControleCaixaException($"Falha ao obter configuração do caixa para Local {_localEvento}");
        }

        public void ThrowExceptionSePrecisarDeCaixaAberto(IUsuario usuario)
        {
            var controleCaixaAtivo = ControleDeCaixaEstaAtivo();
            
            if (!controleCaixaAtivo || _repositorio.ExisteCaixaAbertoPara(usuario, _localEvento))
            {
                return;
            }

            throw new NaoExisteCaixaAbertoException($"Operador {usuario} não possui caixa aberto");
        }
    }
}