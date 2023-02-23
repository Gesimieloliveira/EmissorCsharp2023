using System;
using System.Data;
using FusionCore.CadastroUsuario;
using FusionCore.ControleCaixa.Repositorios;
using FusionCore.DI;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace FusionWPF.SharedViews.ControleCaixa
{
    public class AberturaDeCaixaContexto : ViewModel
    {
        private readonly IUsuario _usuarioLogado;
        private readonly IControleCaixaProvider _caixaProvider;
        private readonly ISessaoManager _sessaoManager;

        public AberturaDeCaixaContexto(IControleCaixaProvider provider, ISessaoManager sessaoManager)
        {
            _caixaProvider = provider;
            _sessaoManager = sessaoManager;
            _usuarioLogado = provider.GetUsuarioLogado();
        }

        public bool JaExisteCaixaAberto
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        public IUsuario OperadorCaixa
        {
            get => GetValue<IUsuario>();
            private set => SetValue(value);
        }

        public DateTime DataAbertura
        {
            get => GetValue<DateTime>();
            private set => SetValue(value);
        }

        public decimal SaldoInicial
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public void Inicializar()
        {
            OperadorCaixa = _usuarioLogado;

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioCaixaIndividual(sessao);

                JaExisteCaixaAberto = repositorio.ExisteCaixaAbertoPara(_usuarioLogado, _caixaProvider.GetLocalEvento());
            }

            if (JaExisteCaixaAberto)
            {
                return;
            }

            DataAbertura = DateTime.Now;
            SaldoInicial = 0.00M;
        }

        public void FazerAberturaDeCaixa()
        {
            if (SaldoInicial < 0)
            {
                throw new InvalidOperationException("Preciso de um saldo positivo para continuar!");
            }

            using (var sessao = _sessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var servicoAberturaDeCaixa = _caixaProvider.CriarServicoCaixaIndividual(sessao);

                servicoAberturaDeCaixa.AbrirCaixa(_usuarioLogado, DataAbertura, SaldoInicial);

                sessao.Transaction.Commit();
            }
        }
    }
}