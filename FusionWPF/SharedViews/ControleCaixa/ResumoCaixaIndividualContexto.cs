using System;
using System.Collections.Generic;
using System.Data;
using FusionCore.ControleCaixa;
using FusionCore.ControleCaixa.Individual;
using FusionCore.ControleCaixa.Repositorios;
using FusionCore.Core.Flags;
using FusionCore.DI;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace FusionWPF.SharedViews.ControleCaixa
{
    public class ResumoCaixaIndividualContexto : ViewModel
    {
        private readonly Guid _caixaId;

        public ResumoCaixaIndividualContexto(
            ISessaoManager sessaoManager,
            IControleCaixaProvider caixaProvider,
            Guid caixaId)
        {
            SessaoManager = sessaoManager;
            CaixaProvider = caixaProvider;
            _caixaId = caixaId;

            ResumoCaixa = new List<FluxoResumoDTO>();
            TotalPorMeioPagamento = new Dictionary<ETipoPagamento, decimal>();
        }

        public ISessaoManager SessaoManager { get; }
        public IControleCaixaProvider CaixaProvider { get; }

        public CaixaIndividual CaixaIndividual
        {
            get => GetValue<CaixaIndividual>();
            private set => SetValue(value);
        }

        public IEnumerable<FluxoResumoDTO> ResumoCaixa
        {
            get => GetValue<IEnumerable<FluxoResumoDTO>>();
            private set => SetValue(value);
        }

        public IDictionary<ETipoPagamento, decimal> TotalPorMeioPagamento
        {
            get => GetValue<IDictionary<ETipoPagamento, decimal>>();
            private set => SetValue(value);
        }

        public void Inicializar()
        {
            using (var sessao = SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioCaixaIndividual(sessao);

                CaixaIndividual = repositorio.BuscarPeloId(_caixaId);
                ResumoCaixa = repositorio.BuscarResumoCaixa(CaixaIndividual);
            }

            CalcularResumoPorMeioDePagamento();
        }

        private void CalcularResumoPorMeioDePagamento()
        {
            TotalPorMeioPagamento.Clear();

            foreach (var i in ResumoCaixa)
            {
                if (!TotalPorMeioPagamento.ContainsKey(i.MeioPagamento))
                {
                    TotalPorMeioPagamento[i.MeioPagamento] = 0;
                }

                TotalPorMeioPagamento[i.MeioPagamento] += i.TotalOperacao;
            }
        }

        public void FecharCaixa(decimal saldoInformado)
        {
            using (var sessao = SessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var servicoCaixa = CaixaProvider.CriarServicoCaixaIndividual(sessao);

                servicoCaixa.FecharCaixa(CaixaIndividual, saldoInformado);

                sessao.Transaction.Commit();
            }
        }

        public decimal TotalizarSaldoEsperadoEmDinheiro()
        {
            using (var sessao = SessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioCaixaIndividual(sessao);
                return repositorio.TotalizarSaldoEmDinheiro(CaixaIndividual);
            }
        }
    }
}