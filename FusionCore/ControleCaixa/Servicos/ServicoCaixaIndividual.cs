using System;
using FusionCore.CadastroUsuario;
using FusionCore.ControleCaixa.Individual;
using FusionCore.ControleCaixa.Repositorios;
using FusionCore.Core.Flags;
using FusionCore.DI;
using FusionCore.Helpers.Basico;
using NHibernate;

namespace FusionCore.ControleCaixa.Servicos
{
    public class ServicoCaixaIndividual
    {
        private readonly ISession _session;
        private readonly IControleCaixaProvider _caixaProvider;
        private readonly byte? _terminalId;
        private RepositorioCaixaIndividual _repositorioCaixaIndividual;

        public ServicoCaixaIndividual(ISession session, IControleCaixaProvider caixaProvider, byte? terminalId = null)
        {
            _session = session;
            _caixaProvider = caixaProvider;
            _terminalId = terminalId;
            _repositorioCaixaIndividual = new RepositorioCaixaIndividual(session);
        }

        public CaixaIndividual AbrirCaixa(IUsuario usuario, DateTime dataAbertura, decimal saldoInicial)
        {
            var localEvento = _caixaProvider.GetLocalEvento();

            if (_repositorioCaixaIndividual.ExisteCaixaAbertoPara(usuario, localEvento))
            {
                throw new InvalidOperationException($"Já existe um caixa para esse usuário: {usuario}");
            }

            var cx = new CaixaIndividual(usuario, dataAbertura, saldoInicial, localEvento, _terminalId);

            _repositorioCaixaIndividual.Persistir(cx);

            RegistrarAberturaFluxo(cx);

            if (localEvento == ELocalEventoCaixa.Gestao)
            {
                var servicoContaCaixa = _caixaProvider.CriarServicoContaCaixa(_session);
                servicoContaCaixa.RegistrarAberturaCaixaLoja(cx);
            }

            return cx;
        }

        private void RegistrarAberturaFluxo(CaixaIndividual cx)
        {
            var fluxo = new Fluxo
            {
                TipoOperacao = TipoOperacao.Entrada,
                Caixa = cx,
                Usuario = cx.Usuario,
                DataOperacao = cx.DataAbertura,
                TipoPagamento = ETipoPagamento.Dinheiro,
                ValorOperacao = cx.SaldoInicial,
                OrigemEvento = EOrigemFluxoCaixaIndividual.AberturaCaixa,
                Historico = "abertura de caixa (saldo inicial)"
            };

            _repositorioCaixaIndividual.Persistir(fluxo);
        }

        public void FecharCaixa(CaixaIndividual caixa, decimal saldoExistente)
        {
            var localEvento = _caixaProvider.GetLocalEvento();

            if (!caixa.Usuario.Equals(_caixaProvider.GetUsuarioLogado()))
            {
                throw new InvalidOperationException("Apenas o dono do caixa pode fecha-lo!");
            }

            if (caixa.LocalEvento != localEvento)
            {
                throw new InvalidOperationException($"Preciso que o caixa seja fechado no local ({localEvento.GetDescription()}) de abertura!");
            }

            var repositorio = new RepositorioCaixaIndividual(_session);
            var saldoCalculado = repositorio.TotalizarSaldoEmDinheiro(caixa);

            caixa.FecharCaixa(saldoCalculado, saldoExistente);

            repositorio.Alterar(caixa);

            if (localEvento == ELocalEventoCaixa.Gestao)
            {
                var servicoContaCaixa = _caixaProvider.CriarServicoContaCaixa(_session);
                servicoContaCaixa.RegistrarFechamentoEmCaiaLoja(caixa);
            }
        }
    }
}