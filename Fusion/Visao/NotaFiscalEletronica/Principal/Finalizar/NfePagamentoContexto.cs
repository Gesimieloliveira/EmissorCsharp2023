using System;
using System.Collections.ObjectModel;
using System.Linq;
using Fusion.Sessao;
using Fusion.Visao.NotaFiscalEletronica.Principal.Finalizar.MeioPagamento;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.Pagamentos;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Finalizar
{
    public class NfePagamentoContexto : ViewModel
    {
        private readonly Nfeletronica _nfe;
        private readonly ISessaoManager _sessaoManager;
        private readonly UsuarioDTO _usuarioLogado;
        private bool _semPagamento;

        public NfePagamentoContexto(Nfeletronica nfe, ISessaoManager sessaoManager)
        {
            _nfe = nfe;
            _sessaoManager = sessaoManager;
            _usuarioLogado = SessaoSistema.ObterUsuarioLogado();

            Pagamentos = new ObservableCollection<FormaPagamentoNfe>();
        }

        public IOpcaoPagamento OpcaoPagamento
        {
            get => GetValue<IOpcaoPagamento>();
            set => SetValue(value);
        }

        public ObservableCollection<FormaPagamentoNfe> Pagamentos
        {
            get => GetValue<ObservableCollection<FormaPagamentoNfe>>();
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

        public bool IncluirCobrancaNoXml
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool SemPagamento
        {
            get => _semPagamento;
            set
            {
                _semPagamento = value;

                _nfe.SemPagamento = _semPagamento;

                if (_nfe.SemPagamento)
                {
                    LimparLancamentos();
                }

                SalvarAlteracoesNaNfe();
                PropriedadeAlterada();
            }
        }

        public event EventHandler<Nfeletronica> PagamentoConcluido;

        public void CarregarDadosDaNfe()
        {
            Pagamentos.Clear();

            TotalPagamentos = 0.00M;
            TotalSerPago = _nfe.TotalFinal;
            TotalRestante = _nfe.TotalFinal;
            TemPagamento = _nfe.PossuiPagamento();
            NomePagador = _nfe.Destinatario.Nome;
            ValorPagamento = TotalRestante;
            IncluirCobrancaNoXml = _nfe.IncluiCobrancaNoXml;
            _semPagamento = _nfe.SemPagamento;
            PropriedadeAlterada(nameof(SemPagamento));

            foreach (var pg in _nfe.Pagamentos)
            {
                AdicionarPagamento(pg);
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

        public void AdicionarPagamento(FormaPagamentoNfe pagamento)
        {
            Pagamentos.Add(pagamento);

            TotalPagamentos += pagamento.Valor;
            TotalRestante -= pagamento.Valor;
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
                throw new InvalidOperationException("Não é possível finalizar com o Saldo Restante diferente de Zero (0)");
            }

            _nfe.SemPagamento = SemPagamento;
            _nfe.Pagar(Pagamentos);

            SalvarAlteracoesNaNfe();

            PagamentoConcluido?.Invoke(this, _nfe);
        }

        public void LimparLancamentos()
        {
            if (_nfe.PossuiPagamento())
            {
                _nfe.RemoverPagamentos();
                Pagamentos.Clear();
                SalvarAlteracoesNaNfe();
            }
        }

        public bool PossuiLancamentoPrazo()
        {
            return Pagamentos.Any(i => i.Especie == ETipoPagamento.CreditoLoja);
        }

        public void SalvarAlteracaoParaIncluirCobrancaXml()
        {
            if (IncluirCobrancaNoXml == _nfe.IncluiCobrancaNoXml)
            {
                return;
            }

            _nfe.IncluiCobrancaNoXml = IncluirCobrancaNoXml;

            SalvarAlteracoesNaNfe();
        }

        private void SalvarAlteracoesNaNfe()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            using (var transaction = sessao.BeginTransaction())
            {
                new RepositorioNfe(sessao).SalvarAlteracoes(_nfe);
                transaction.Commit();
            }
        }
    }
}