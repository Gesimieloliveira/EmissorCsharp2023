using FusionCore.ControleCaixa.Repositorios;
using NHibernate;

namespace FusionCore.ControleCaixa.Servicos
{
    public class ServicoLancamentoAvulsoNoCaixa
    {
        private readonly RepositorioLancamentoAvulso _repositorio;
        private readonly ServicoRegistroDeCaixa _servicoRegistro;
        private readonly ServicoContaCaixa _servicoContaCaixa;

        public ServicoLancamentoAvulsoNoCaixa(ISession session, ELocalEventoCaixa localEvento)
        {
            _servicoRegistro = new ServicoRegistroDeCaixa(session, localEvento);
            _repositorio = new RepositorioLancamentoAvulso(session);
            _servicoContaCaixa = new ServicoContaCaixa(session);
        }

        public void IncluirNovoLancamento(LancamentoAvulsoCaixa lancamento)
        {
            _repositorio.Persistir(lancamento);

            if (lancamento.TipoLancamentoCaixa == TipoLancamentoCaixa.LancamentoCaixaIndividual)
            {
                _servicoRegistro.RegistrarLancamento(lancamento);
                return;
            }

            _servicoContaCaixa.RegistrarLancamentoCaixaLojaAvulso(lancamento);
        }

        public void AlterarLancamento(LancamentoAvulsoCaixa lancamento)
        {
            _repositorio.Alterar(lancamento);
        }
    }
}