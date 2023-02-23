using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Fusion.Sessao;
using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Finalizacao.MeioPagamento;
using FusionCore.ControleCaixa.Facades;
using FusionCore.Core.Flags;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionCore.Vendas.Faturamentos;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Finalizacao
{
    public class PagamentoViewModel : ViewModel
    {
        private readonly ISessaoManager _sessaoManager = SessaoSistema.Instancia.SessaoManager;
        private readonly UsuarioDTO _usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;
        private readonly FaturamentoVenda _faturamento;

        public PagamentoViewModel(FaturamentoVenda faturamento)
        {
            _faturamento = faturamento;
            Especies = new ObservableCollection<FPagamento>();
        }

        public IOpcaoPagamento OpcaoPagamento
        {
            get => GetValue<IOpcaoPagamento>();
            set => SetValue(value);
        }

        public ObservableCollection<FPagamento> Especies
        {
            get => GetValue<ObservableCollection<FPagamento>>();
            private set => SetValue(value);
        }

        public decimal TotalFaturamento
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public decimal TotalPagamentos
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public decimal ValorPagamento
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public decimal TotalRestante
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public decimal ValorTroco
        {
            get => GetValue<decimal>();
            private set => SetValue(value);
        }

        public string NomePagador
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public bool TemPagador
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        public bool TemPagamento
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        public event EventHandler<FaturamentoVenda> FinalizacaoConcluida;

        public void Inicializar()
        {
            Especies.Clear();

            TotalFaturamento = _faturamento.Total;
            TotalRestante = _faturamento.Total;
            ValorPagamento = TotalRestante;
            TotalPagamentos = 0.00M;

            foreach (var pg in _faturamento.Pagamentos)
            {
                AdicionarPagamento(pg);
            }

            if (_faturamento.Destinatario != null)
            {
                NomePagador = _faturamento.Destinatario.Cliente.Nome;
                TemPagador = true;
            }
        }

        public bool JaPossuiPagamentoNoPrazo()
        {
            return Especies.Any(i => i.Especie == ETipoPagamento.CreditoLoja);
        }

        public void ValidarPagamento()
        {
            if (ValorPagamento <= 0)
            {
                throw new InvalidOperationException("Preciso de um valor maior que 0,00");
            }

            if (ValorPagamento > TotalRestante && !OpcaoPagamento.PermiteTroco())
            {
                throw new InvalidOperationException("Especie de Pagamento não permite troco!");
            }
        }

        public void PrepararPagamento()
        {
            if (ValorPagamento <= TotalRestante)
            {
                ValorTroco = 0.00M;
                return;
            }

            var troco = decimal.Round(ValorPagamento - TotalRestante, 2);

            ValorPagamento = TotalRestante;
            ValorTroco = troco;
        }

        public void AdicionarPagamento(FPagamento pagamento)
        {
            Especies.Add(pagamento);

            TotalPagamentos += pagamento.Valor;
            TotalRestante -= pagamento.Valor;
            ValorPagamento = TotalRestante;
        }

        public bool PossuiSaldoParaPagamento()
        {
            return TotalRestante != 0;
        }

        public void FinalizarDocumento()
        {
            ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_usuarioLogado);

            if (PossuiSaldoParaPagamento())
            {
                throw new InvalidOperationException("Não é possível finalizar com saldo restante");
            }

            using (var session = _sessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
            {
                var facade = new FaturamentoVendaService(session, _usuarioLogado);
                facade.Finalizar(_faturamento, Especies.ToArray(), decimal.Round(ValorTroco, 2));
                session.Transaction.Commit();
            }

            FinalizacaoConcluida?.Invoke(this, _faturamento);
        }

        public void LimparLancamentos()
        {
            Especies.Clear();

            TotalPagamentos = 0.00M;
            TotalRestante = TotalFaturamento;
            ValorPagamento = TotalFaturamento;
            TemPagamento = false;
        }
    }
}