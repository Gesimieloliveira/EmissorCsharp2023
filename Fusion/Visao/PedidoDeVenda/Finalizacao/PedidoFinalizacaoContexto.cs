using System;
using System.Collections.ObjectModel;
using System.Linq;
using Fusion.Sessao;
using Fusion.Visao.PedidoDeVenda.Finalizacao.MeioPagamento;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.FusionAdm.PedidoVenda.Fabricas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.PedidoDeVenda.Finalizacao
{
    public class PedidoFinalizacaoContexto : ViewModel
    {
        private readonly PedidoVenda _pedido;
        private readonly ISessaoManager _sessaoManager;
        private readonly UsuarioDTO _usuarioLogado;

        public PedidoFinalizacaoContexto(PedidoVenda pedido, ISessaoManager sessaoManager)
        {
            _pedido = pedido;
            _sessaoManager = sessaoManager;
            _usuarioLogado = SessaoSistema.ObterUsuarioLogado();

            Negociacoes = new ObservableCollection<Negociacao>();
        }

        public IOpcaoPagamento OpcaoPagamento
        {
            get => GetValue<IOpcaoPagamento>();
            set => SetValue(value);
        }

        public ObservableCollection<Negociacao> Negociacoes
        {
            get => GetValue<ObservableCollection<Negociacao>>();
            private set => SetValue(value);
        }

        public decimal TotalSerPago
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

        public string NomePagador
        {
            get => GetValue<string>();
            private set
            {
                SetValue(value);
                SetValue(value?.Length > 0, nameof(TemPagador));
            }
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

        public event EventHandler<PedidoVenda> FinalizacaoConcluida;
        public event EventHandler<PedidoVenda> FinalizacaoRemovida;

        public void CarregarDadosDoPedido()
        {
            Negociacoes.Clear();

            TotalPagamentos = 0.00M;
            TotalSerPago = _pedido.Total;
            TotalRestante = _pedido.Total;
            TemPagamento = _pedido.PossuiNegociacao();
            NomePagador = _pedido?.Destinatario?.GetNome;
            ValorPagamento = TotalRestante;

            foreach (var n in _pedido.Negociacao)
            {
                AdicionarNegociacao(n);
            }
        }

        public void ValidarPagamento()
        {
            if (ValorPagamento <= 0.00M)
            {
                throw new InvalidOperationException("Preciso de um valor maior que 0,00");
            }

            if (ValorPagamento > TotalRestante)
            {
                throw new InvalidOperationException("Valor acima do permitido, este Documento não suporta Troco.");
            }
        }

        public void AdicionarNegociacao(Negociacao negociacao)
        {
            Negociacoes.Add(negociacao);

            TotalPagamentos += negociacao.Valor;
            TotalRestante -= negociacao.Valor;
            ValorPagamento = TotalRestante;
        }

        public bool PossuiSaldoParaPagamento()
        {
            return TotalRestante != 0;
        }

        public void FinalizarPagamentos()
        {
            if (PossuiSaldoParaPagamento())
            {
                throw new InvalidOperationException("Não é possível finalizar com saldo restante");
            }

            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = FactoryRepositorioPedidoVenda.CriaRepositorio(sessao);

                _pedido.Finalizar(Negociacoes);
                repositorio.SalvarAlteracoes(_pedido);

                transacao.Commit();
            }

            FinalizacaoConcluida?.Invoke(this, _pedido);
        }

        public void LimparLancamentos()
        {
            if (_pedido.PossuiNegociacao())
            {
                using (var sessao = _sessaoManager.CriaSessao())
                using (var transacao = sessao.BeginTransaction())
                {
                    var repositorio = FactoryRepositorioPedidoVenda.CriaRepositorio(sessao);

                    _pedido.RemoverFinalizacao();
                    repositorio.SalvarAlteracoes(_pedido);

                    transacao.Commit();
                }
            }

            CarregarDadosDoPedido();
            FinalizacaoRemovida?.Invoke(this, _pedido);
        }

        public bool PossuiLancamentoPrazo()
        {
            return Negociacoes.Any(i => i.Especie == ETipoPagamento.CreditoLoja);
        }
    }
}